/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     License  : Please read LICENSE.txt file      *
*  Type      : DataLog                                          Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Logs data changes.                                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Data {

  internal class DataLog {

    #region Constructors and parsers

    internal DataLog(DataOperation operation) {
      Assertion.Require(operation, "operation");

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
      if (this.DataOperation.IsSystemOperation) {
        return;
      }

      if (!EmpiriaString.StartsWith(this.DataOperation.Name, "write", "apd", "do", "set")) {
        return;
      }

      var op = DataOperation.Parse("apdDataLog", this.SessionId, this.Timestamp,
                                   this.DataOperation.DataSource.Name, this.DataOperation.Name,
                                   this.DataOperation.ParametersAsJson(), this.ObjectId);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class DataLog

} // namespace Empiria.Data
