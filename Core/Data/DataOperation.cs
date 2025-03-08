/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Data                               Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : DataOperation                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a database read or write operation that can be executed using                       *
*             DataReader or DataWriter services.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Data;

using Empiria.Collections;

using Empiria.Data.Handlers;

namespace Empiria.Data {

  /// <summary>Represents a database read or write operation that can be executed using
  /// DataReader or DataWriter services.</summary>
  public sealed class DataOperation : OperationBase {

    #region Fields

    static readonly private EmpiriaDictionary<string, string> _textCommandCache =
                                                                  new EmpiriaDictionary<string, string>(16);

    private readonly DataSource _dataSource;
    private readonly string _sourceText;

    #endregion Fields

    #region Constructors and parsers

    private DataOperation(DataSource dataSource, string dataOperation) : base(dataOperation) {
      _dataSource = dataSource;
      _sourceText = GetSourceText(dataOperation);
    }

    private DataOperation(DataSource dataSource, string dataOperation, object[] parameters)
                                                                : base(dataOperation, parameters) {
      _dataSource = dataSource;
      _sourceText = GetSourceText(dataOperation);
    }


    static public DataOperation Parse(string dataOperation) {
      Assertion.Require(dataOperation, nameof(dataOperation));

      return new DataOperation(DataSource.Parse(dataOperation), dataOperation);
    }


    static public DataOperation Parse(string dataOperation, params object[] parameters) {
      Assertion.Require(dataOperation, nameof(dataOperation));

      if (parameters != null && parameters.Length == 1 && parameters[0] is Array) {
        return new DataOperation(DataSource.Parse(dataOperation), dataOperation, (object[]) parameters[0]);
      } else {
        return new DataOperation(DataSource.Parse(dataOperation), dataOperation, parameters);
      }
    }


    static public DataOperation Parse(DataSource dataSource, string dataOperation) {
      Assertion.Require(dataSource, nameof(dataSource));
      Assertion.Require(dataOperation, nameof(dataOperation));

      return new DataOperation(dataSource, dataOperation);
    }


    static public DataOperation Parse(DataSource dataSource, string dataOperation,
                                      params object[] parameters) {
      Assertion.Require(dataSource, nameof(dataSource));
      Assertion.Require(dataOperation, nameof(dataOperation));

      return new DataOperation(dataSource, dataOperation, parameters);
    }

    #endregion Constructors and parsers

    #region Properties

    public CommandType CommandType {
      get {
        return GetCommandType(base.Name);
      }
    }


    public DataSource DataSource {
      get {
        return _dataSource;
      }
    }


    public int ExecutionTimeout {
      get; set;
    }


    public string SourceName {
      get {
        if (base.Name.StartsWith("@")) {
          return _sourceText;
        } else {
          return base.Name;
        }
      }
    }


    public bool IsSystemOperation {
      get {
        return EmpiriaString.IsInList(this.Name,
                                      "apdDataLog", "apdAuditTrail", "apdLogEntry",
                                      "apdUserSession", "doCloseUserSession",
                                      "doOptimization");
      }
    }

    #endregion Properties

    #region Methods

    /// <summary>Gets the data operation source text with its parameters values parsed.</summary>
    public string AsText() {
      if (base.Parameters.Length != 0) {
        return string.Format(_sourceText, base.Parameters);
      } else {
        return _sourceText;
      }
    }

    public void PrepareCommand(IDbCommand command) {
      command.CommandType = this.CommandType;
      if (this.ExecutionTimeout != 0) {
        command.CommandTimeout = this.ExecutionTimeout;
      }

      this.FillParameters(command);
    }

    #endregion Methods

    #region Helpers

    private void FillParameters(IDbCommand command) {
      if (base.Parameters.Length == 0) {
        return;
      }

      if (command.CommandType == CommandType.Text) {
        command.CommandText = string.Format(_sourceText, base.Parameters);
        return;
      }

      IDataHandler handler = this.DataSource.GetDataHandler();

      IDataParameter[] pars = handler.GetParameters(_dataSource.Source, base.Name, base.Parameters);

      if (pars == null) {
        return;
      }

      for (int i = 0; i < pars.Length; i++) {
        command.Parameters.Add(pars[i]);
      }
    }


    static private CommandType GetCommandType(string sourceName) {
      if (sourceName.StartsWith("@") || sourceName.IndexOf(' ') != -1) {

        return CommandType.Text;

      } else if (EmpiriaString.StartsWith(sourceName, "qry", "write", "get", "set",
                                                      "apd", "upd", "del", "do", "rpt", "val")) {
        return CommandType.StoredProcedure;

      } else {
        return CommandType.TableDirect;
      }
    }


    static private string GetSourceText(string sourceName) {
      if (!sourceName.StartsWith("@")) {
        return sourceName;
      }

      string textCommand = string.Empty;

      if (!_textCommandCache.ContainsKey(sourceName)) {

        textCommand = ReadDataSourceText(sourceName);
        _textCommandCache.Insert(sourceName, textCommand);

      } else {
        textCommand = _textCommandCache[sourceName];
      }

      return textCommand;
    }


    static private string ReadDataSourceText(string sourceName) {
      try {
        var sql = $"SELECT * FROM DBQueryStrings " +
                  $"WHERE QueryName = '{sourceName}'";

        var operation = DataOperation.Parse(sql);

        string text = string.Empty;

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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataSourceNotDefined,
                                       innerException, sourceName);
      }
    }

    #endregion Helpers

  } // class DataOperation

} //namespace Empiria.Data
