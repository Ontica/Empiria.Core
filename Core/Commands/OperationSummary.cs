/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Commands                           Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : OperationSummary                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds summary information about a command or operation already executed or invoked as dry-run. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.Commands {

  /// <summary>Holds summary information about a command or operation already
  /// executed or invoked as dry-run.</summary>
  public class OperationSummary {

    private readonly List<string> _items = new List<string>(32);
    private readonly List<string> _errors = new List<string>(32);

    public string Operation {
      get; set;
    }

    public int Count {
      get; set;
    }

    public int Errors {
      get {
        return _errors.Count;
      }
    }

    public FixedList<string> ItemsList {
      get {
        return _items.ToFixedList();
      }
    }


    public FixedList<string> ErrorsList {
      get {
        return _errors.ToFixedList();
      }
    }


    public void AddErrors(IEnumerable<string> issues) {
      Assertion.Require(issues, nameof(issues));

      _errors.AddRange(issues);
    }


    public void AddItem(string item) {
      Assertion.Require(item, nameof(item));

      _items.Add(item);
    }

  }  // class OperationSummary

}  // namespace Empiria.Commands
