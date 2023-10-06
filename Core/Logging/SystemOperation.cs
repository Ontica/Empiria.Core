/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : SystemOperation                                  Pattern  : Service provider                  *
*                                                                                                            *
*  Summary   : Represents a System Operation.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Collections;
using Empiria.Data;

namespace Empiria {

  /// <summary>Represents a System Operation.</summary>
  internal class SystemOperation {

    static EmpiriaHashTable<SystemOperation> _cache = new EmpiriaHashTable<SystemOperation>(1024);

    static SystemOperation() {
      LoadCache();
    }

    static internal SystemOperation TryParse(string operation) {
      if (_cache.TryGetValue(operation, out SystemOperation value)) {
        return value;
      } else {
        return null;
      }
    }


    [DataField("SystemOperationCode")]
    internal string Code {
      get;
      private set;
    }

    [DataField("Description")]
    internal string Description {
      get;
      private set;
    }

    static private void LoadCache() {
      string sql = "SELECT * FROM SystemOperations";

      var op = DataOperation.Parse(sql);

      _cache = DataReader.GetPlainObjectHashTable<SystemOperation>(op, x => x.Code);

    }

  }  // class SystemOperation

}  // namespace Empiria
