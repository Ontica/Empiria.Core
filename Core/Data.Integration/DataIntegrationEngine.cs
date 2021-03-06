﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Integration Services         *
*  Namespace : Empiria.Data.Integration                         License  : Please read LICENSE.txt file      *
*  Type      : DataIntegrationEngine                            Pattern  : Singleton Class                   *
*                                                                                                            *
*  Summary   : Executes async data operations published on queues throw web services invocation.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Timers;

using Empiria.Security;

namespace Empiria.Data.Integration {

  /// <summary>Executes async data operations published on queues throw web services invocation.</summary>
  public class DataIntegrationEngine {

    #region Fields

    static private DataIntegrationEngine instance = null;
    static private Mutex mutex = null;

    static private readonly string mutexName = "{Empiria.DataIntegrationEngine}.{" + Guid.NewGuid().ToString() + "}";

    private System.Timers.Timer timer = null;
    private int currentTasks = -1;
    private bool executingFlag = false;
    private bool stopFlag = false;
    private IAsyncResult asyncResult = null;
    private delegate void DataIntegrationDelegate();

    #endregion Fields

    #region Constructors and parsers

    private DataIntegrationEngine() {
      // Object creation of singleton classes not allowed
    }

    static public DataIntegrationEngine Instance {
      get {
        try {
          if (mutex == null) {
            CreateMutex();
          }
          if (mutex.WaitOne()) {
            if (instance == null) {
              instance = new DataIntegrationEngine();
            }
            GC.KeepAlive(instance);
            return instance;
          } else {
            throw new EmpiriaDataException(EmpiriaDataException.Msg.DuplicateDataIntegrationEngine);
          }
        } catch {
          throw;
        } finally {
          mutex.ReleaseMutex();
        } // try
      } // get
    }

    #endregion Constructors and parsers

    #region Public properties

    public int CurrentTasks {
      get { return currentTasks; }
    }

    public bool IsExecuting {
      get { return executingFlag; }
    }

    public bool IsStopped {
      get { return stopFlag; }
    }

    #endregion Public properties

    #region Public methods

    public void Start() {
      string msg = String.Empty;
      int interval = ConfigurationData.GetInteger("DataIntegration.Execution.Seconds") * 1000;
      if (executingFlag) {
        if (stopFlag) {
          stopFlag = false;
          msg = "Se programó la inicialización automática del servicio de integración de datos a las " +
                DateTime.Now.ToLongTimeString();
          EmpiriaLog.Info(msg);
        }
        return;
      }
      stopFlag = false;
      if (timer == null) {
        timer = new System.Timers.Timer();
        timer.Interval = 1000;
        msg = "Se inicializó el servicio de integración de datos a las " + DateTime.Now.ToLongTimeString();
        msg += ", mismo que se estará ejecutando cada " + (interval / 1000).ToString() + " segundos.";
        EmpiriaLog.Info(msg);
      } else {
        timer = new System.Timers.Timer();
        timer.Interval = interval;
      }
      timer.AutoReset = false;
      timer.Elapsed += new ElapsedEventHandler(ExecuteEngineEvent);
      timer.Start();
    }

    public void Stop() {
      if (stopFlag) {
        return;
      }
      stopFlag = true;
      string msg = String.Empty;
      if (executingFlag) {
        msg = "Se envió el comando 'Stop' al servicio de integración de datos a las " + DateTime.Now.ToLongTimeString();
        EmpiriaLog.Info(msg);
      } else {
        timer.Stop();
        timer = null;
        msg = "Se detuvo el servicio de integración de datos a las " + DateTime.Now.ToLongTimeString();
        EmpiriaLog.Info(msg);
      }
    }

    #endregion Public methods

    #region Private methods

    static private void CreateMutex() {
      try {
        bool createdNew = false;
        mutex = new Mutex(false, mutexName, out createdNew);
        if (!createdNew) {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.DuplicateDataIntegrationEngine);
        }
        GC.KeepAlive(mutex);
      } catch (EmpiriaDataException) {
        GC.Collect();
        throw;
      } catch (Exception e) {
        GC.Collect();
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DuplicateDataIntegrationEngine, e);
      }
    }

    private void ExecuteEngineEvent(object source, ElapsedEventArgs e) {
      if (executingFlag) {
        return;
      }
      executingFlag = true;
      DataIntegrationDelegate dataIntegrationDelegate = new DataIntegrationDelegate(DoIntegration);
      asyncResult = dataIntegrationDelegate.BeginInvoke(EndConvertData, null);
    }

    private void DoIntegration() {
      DataTable table = GetPendingIntegrationTasks();

      currentTasks = table.Rows.Count;

      for (int i = 0; i < table.Rows.Count; i++) {
        DataRow taskRow = table.Rows[i];
        long integrationTaskId = (long) taskRow["IntegrationTaskId"];
        int targetServerId = (int) taskRow["TargetServerId"];
        string ssoToken = (string) taskRow["SSOToken"];

        string dataOperationMessage = (string) taskRow["OperationName"] + '§' + (string) taskRow["OperationParameters"];

        try {
          WebServer targetServer = DataIntegratorWSProxy.GetIntegrationServer(targetServerId);
          using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
            int result = proxy.Execute(ssoToken, dataOperationMessage);
            if (result != 0) {
              UpdateIntegrationTask(integrationTaskId, true);
            } else {
              UpdateIntegrationTask(integrationTaskId, false);
            }
          }
        } finally {
          // no-op
        }
      }
      currentTasks = -1;
    }

    private void EndConvertData(IAsyncResult asyncResult) {
      DataIntegrationDelegate dataIntegrationDelegate = (DataIntegrationDelegate) ((AsyncResult) asyncResult).AsyncDelegate;

      dataIntegrationDelegate.EndInvoke(asyncResult);

      executingFlag = false;
      if (!stopFlag) {
        this.Start();
      } else {
        timer = null;
        string msg = "Se detuvo el servicio de integración de datos a las " + DateTime.Now.ToLongTimeString();
        EmpiriaLog.Info(msg);
      }
    }

    private DataTable GetPendingIntegrationTasks() {
      return DataReader.GetDataTable(DataOperation.Parse("qryDBPendingIntegrationTasks"));
    }

    private void UpdateIntegrationTask(long integrationTaskId, bool integrated) {
      char executionMode = 'A';

      if (integrated) {
        executionMode = 'A';
      } else {
        executionMode = 'V';
      }
      DataWriter.Execute(DataOperation.Parse("setDBIntegrationTaskOk", integrationTaskId, DateTime.Now, executionMode, -1));
    }

    #endregion Private methods

  } //class DataIntegrationEngine

} // namespace Empiria.Data.Integration
