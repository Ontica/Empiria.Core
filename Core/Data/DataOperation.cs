/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     License  : Please read LICENSE.txt file      *
*  Type      : DataOperation                                    Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Type that represents a database read or write operation with or without parameters.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Collections;
using Empiria.Data.Handlers;

namespace Empiria.Data {

  /// <summary>Type that represents a database read or write operation with or without parameters.</summary>
  public sealed class DataOperation : OperationBase {

    #region Fields

    static private EmpiriaDictionary<string, string> textCommandCache = new EmpiriaDictionary<string, string>(16);

    private readonly DataSource dataSource;
    private readonly string sourceText;
    private int executionTimeout = 0;

    #endregion Fields

    #region Constructors and parsers

    private DataOperation(DataSource dataSource, string sourceName) : base(sourceName) {
      this.dataSource = dataSource;
      this.sourceText = GetSourceText(sourceName);
    }

    private DataOperation(DataSource dataSource, string sourceName, object[] parameters)
      : base(sourceName, parameters) {
      this.dataSource = dataSource;
      this.sourceText = GetSourceText(sourceName);
    }

    static public DataOperation Parse(string sourceName) {
      return new DataOperation(DataSource.Parse(sourceName), sourceName);
    }

    static public DataOperation Parse(string sourceName, params object[] parameters) {
      if (parameters != null && parameters.Length == 1 && parameters[0] is Array) {
        return new DataOperation(DataSource.Parse(sourceName), sourceName, (object[]) parameters[0]);
      } else {
        return new DataOperation(DataSource.Parse(sourceName), sourceName, parameters);
      }
    }

    static internal DataOperation Parse(DataSource dataSource, string sourceName,
                                        params object[] parameters) {
      Assertion.AssertObject(dataSource, "dataSource");

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

    public CommandType CommandType {
      get {
        return GetCommandType(base.Name);
      }
    }

    public DataSource DataSource {
      get {
        return dataSource;
      }
    }


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

    public bool IsSystemOperation {
      get {
        if (EmpiriaString.IsInList(this.Name,
                                   "apdDataLog", "apdAuditTrail", "apdLogEntry",
                                   "apdUserSession", "doCloseUserSession",
                                   "doOptimization")) {
          return true;
        }
        return false;
      }
    }

    #endregion Public properties

    #region Public methods

    /// <summary>Gets the data operation source text with its parameters values parsed.</summary>
    public string AsText() {
      if (base.Parameters.Length != 0) {
        return String.Format(sourceText, base.Parameters);
      } else {
        return sourceText;
      }
    }

    public void PrepareCommand(IDbCommand command) {
      command.CommandType = this.CommandType;
      if (this.ExecutionTimeout != 0) {
        command.CommandTimeout = this.ExecutionTimeout;
      }

      this.FillParameters(command);
    }

    #endregion Public methods

    #region Private methods


    private void FillParameters(IDbCommand command) {
      if (base.Parameters.Length == 0) {
        return;
      }
      IDataHandler handler = this.DataSource.GetDataHandler();

      IDataParameter[] pars = handler.GetParameters(dataSource.Source, base.Name, base.Parameters);

      if ((pars != null) && (command.CommandType != CommandType.Text)) {
        for (int i = 0, j = pars.Length; i < j; i++) {
          command.Parameters.Add(pars[i]);
        }
      } else if (command.CommandType == CommandType.Text) {
        command.CommandText = String.Format(sourceText, base.Parameters);
      }
    }


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
        if (!textCommandCache.ContainsKey(sourceName)) {
          textCommand = ReadDataSourceText(sourceName);
          textCommandCache.Insert(sourceName, textCommand);
        } else {
          textCommand = textCommandCache[sourceName];
        }
        return textCommand;
      } else {
        return sourceName;
      }
    }

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
          case DataTechnology.PostgreSql:
            text = DataReader.GetFieldValue(operation, "PostgreSQLQueryString") as string;
            break;
          default:
            text = DataReader.GetFieldValue(operation, "OleDbQueryString") as string;
            break;
        }
        if (!String.IsNullOrWhiteSpace(text)) {
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
