/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataOperation                                    Pattern  : Standard Class                    *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Type that represents a database read or write operation with or without parameters.           *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;

using Empiria.Data.Handlers;

namespace Empiria.Data {

  /// <summary>Type that represents a database read or write operation with or without parameters.</summary>
  public sealed class DataOperation : OperationBase {

    #region Fields

    static public Dictionary<string, int> PerformanceTable = new Dictionary<string, int>();

    static private Dictionary<string, string> textCommandDictionary = new Dictionary<string, string>();

    private readonly DataSource dataSource;
    private string sourceText = String.Empty;
    private int executionTimeout = 0;

    #endregion Fields

    #region Constructors and parsers

    private DataOperation(DataSource dataSource, string sourceName)
      : base(sourceName) {
      this.dataSource = dataSource;
      this.sourceText = GetSourceText(sourceName);
      IncrementPerformanceItem(sourceName);
    }

    private DataOperation(DataSource dataSource, string sourceName, object[] parameters)
      : base(sourceName, parameters) {
      this.dataSource = dataSource;
      this.sourceText = GetSourceText(sourceName);
      IncrementPerformanceItem(sourceName);
    }

    static private void IncrementPerformanceItem(string sourceName) {
      if (!PerformanceTable.ContainsKey(sourceName)) {
        PerformanceTable[sourceName] = 0;
      }
      PerformanceTable[sourceName]++;
    }

    static public DataOperation Parse(string sourceName) {
      return new DataOperation(DataSource.Parse(sourceName), sourceName);
    }

    static public DataOperation Parse(string sourceName, params object[] parameters) {
      return new DataOperation(DataSource.Parse(sourceName), sourceName, parameters);
    }

    static internal DataOperation Parse(DataSource dataSource, string sourceName,
                                        params object[] parameters) {
      Empiria.Assertion.EnsureObject(dataSource, "dataSource");

      return new DataOperation(dataSource, sourceName, parameters);
    }

    static public DataOperation ParseFromMessage(string message) {
      string sourceName = String.Empty;
      object[] parameters = null;

      OperationBase.ExtractFromMessage(message, out sourceName, out parameters);

      if (parameters != null) {
        return new DataOperation(DataSource.Parse(sourceName), sourceName, parameters);
      } else {
        return new DataOperation(DataSource.Parse(sourceName), sourceName);
      }
    }

    static public DataOperation ParseFromMessageProtected(string message) {
      string sourceName = String.Empty;
      object[] parameters = null;

      OperationBase.ExtractFromMessageProtected(message, out sourceName, out parameters);

      if (parameters != null) {
        return new DataOperation(DataSource.Parse(sourceName), sourceName, parameters);
      } else {
        return new DataOperation(DataSource.Parse(sourceName), sourceName);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public int ExecutionTimeout {
      get { return executionTimeout; }
      set { executionTimeout = value; }
    }

    public string SourceName {
      get {
        if (base.Name.StartsWith("@")) {
          return sourceText;
        } else {
          return base.Name;
        }
      }
    }

    static public void DumpPerformanceTable() {
      string temp = "Análisis de desempeño para el servidor " + ExecutionServer.Name + Environment.NewLine + Environment.NewLine;

      string[] keys = new string[PerformanceTable.Count];
      PerformanceTable.Keys.CopyTo(keys, 0);
      int counter = 0;
      for (int i = 0; i < keys.Length; i++) {
        temp += i.ToString("000") + " " + keys[i] + " = " + PerformanceTable[keys[i]] + Environment.NewLine;
        counter += PerformanceTable[keys[i]];
      }
      temp += Environment.NewLine + "Total de accesos = " + counter.ToString("N0");

      Empiria.Messaging.Publisher.Publish(new Messaging.Message(temp));
    }

    #endregion Public properties

    #region Internal properties

    internal CommandType CommandType {
      get { return GetCommandType(base.Name); }
    }

    internal DataSource DataSource {
      get { return dataSource; }
    }

    #endregion Internal properties

    #region Internal methods

    public string GetSourceFull() {
      if (base.Parameters == null || base.Parameters.Length == 0) {
        return sourceText;
      } else {
        return String.Format(sourceText, base.Parameters);
      }
    }

    internal void FillParameters(IDbCommand command) {
      if (base.Parameters == null || base.Parameters.Length == 0) {
        return;
      }
      IDataParameter[] pars = null;
      switch (DataSource.Technology) {
        case DataTechnology.SqlServer:
          pars = SqlParameterCache.GetParameters(dataSource.Source, base.Name, base.Parameters);
          break;
        case DataTechnology.MySql:
          pars = MySqlParameterCache.GetParameters(dataSource.Source, base.Name, base.Parameters);
          break;
        case DataTechnology.Oracle:
          pars = OracleParameterCache.GetParameters(dataSource.Source, base.Name, base.Parameters);
          break;
        case DataTechnology.PostgreSQL:
          pars = PostgreSqlParameterCache.GetParameters(dataSource.Source, base.Name, base.Parameters);
          break;
        default:
          pars = OleDbParameterCache.GetParameters(dataSource.Source, base.Name, base.Parameters);
          break;
      }
      if ((pars != null) && (command.CommandType != CommandType.Text)) {
        for (int i = 0, j = pars.Length; i < j; i++) {
          command.Parameters.Add(pars[i]);
        }
      } else if (command.CommandType == CommandType.Text) {
        command.CommandText = String.Format(sourceText, base.Parameters);
      }
    }

    #endregion Internal methods

    #region Private methods

    static private CommandType GetCommandType(string sourceName) {
      if (sourceName.StartsWith("@") || sourceName.IndexOf(' ') != -1) {
        return CommandType.Text;
      } else if (sourceName.StartsWith("qry") || sourceName.StartsWith("write") ||
                 sourceName.StartsWith("get") || sourceName.StartsWith("set") ||
                 sourceName.StartsWith("apd") || sourceName.StartsWith("upd") || sourceName.StartsWith("del") ||
                 sourceName.StartsWith("do") || sourceName.StartsWith("rpt") || sourceName.StartsWith("val")) {
        return CommandType.StoredProcedure;
      } else {
        return CommandType.TableDirect;
      }
    }

    static private string GetSourceText(string sourceName) {
      if (sourceName.StartsWith("@")) {
        string textCommand = String.Empty;
        if (!textCommandDictionary.TryGetValue(sourceName, out textCommand)) {
          textCommand = ReadDataSourceText(sourceName);
          textCommandDictionary[sourceName] = textCommand;
        }
        return textCommand;
      } else {
        return sourceName;
      }
    }

    [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "8b7fe9c60c0f43bd")]
    static private string ReadDataSourceText(string sourceName) {
      try {
        DataOperation operation = DataOperation.Parse("qryDBQueryString", sourceName);
        string text = String.Empty;
        switch (operation.DataSource.Technology) {
          case DataTechnology.SqlServer:
            text = DataReader.GetFieldValue(operation, "SqlQueryString") as string;
            break;
          case DataTechnology.MySql:
            text = DataReader.GetFieldValue(operation, "MySqlQueryString") as string;
            break;
          case DataTechnology.Oracle:
            text = DataReader.GetFieldValue(operation, "OracleQueryString") as string;
            break;
          case DataTechnology.PostgreSQL:
            text = DataReader.GetFieldValue(operation, "PostgreSQLQueryString") as string;
            break;
          default:
            text = DataReader.GetFieldValue(operation, "OleDbQueryString") as string;
            break;
        }
        if (text.Length != 0) {
          return text;
        } else {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.DataSourceNotDefined, sourceName);
        }
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataSourceNotDefined, innerException, sourceName);
      }
    }

    #endregion Private methods

  } // struct DataOperation

} //namespace Empiria.Data