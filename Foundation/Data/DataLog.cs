/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataLog                                          Pattern  : Standard Class                    *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Logs data changes.                                                                            *
*                                                                                                            *
********************************* Copyright (c) 1999-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Data {

  internal class DataLog {

    #region Constructors and parsers

    internal DataLog(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      this.DataOperation = operation;

      if (ExecutionServer.IsAuthenticated) {
        this.SessionId = ExecutionServer.CurrentPrincipal.Session.Id;
      }

    }

    #endregion Constructors and parsers

    #region Properties

    public int SessionId {
      get;
      private set;
    } = -1;


    public DateTime Timestamp {
      get;
      private set;
    } = DateTime.Now;


    public DataOperation DataOperation {
      get;
      private set;
    }

    public int ObjectId {
      get {
        if (this.DataOperation.Parameters.Length > 0 &&
            this.DataOperation.Parameters[0] is Int32) {
          return (int) this.DataOperation.Parameters[0];
        } else {
          return -1;
        }
      }
    }

    #endregion Properties

    #region Methods

    internal void Write() {
      if (EmpiriaString.IsInList(this.DataOperation.Name,
                                 "apdDataLog", "apdAuditTrail", "apdLogEntry",
                                 "apdUserSession", "doCloseUserSession",
                                 "doOptimization")) {
        return;
      }

      if (!EmpiriaString.StartsWith(this.DataOperation.Name, "write", "apd", "do")) {
        return;
      }

      var op = Data.DataOperation.Parse("apdDataLog", this.SessionId, this.Timestamp,
                                   this.DataOperation.DataSource.Name, this.DataOperation.Name,
                                   this.DataOperation.ParametersAsJson(), this.ObjectId);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class DataLog

} // namespace Empiria.Data
