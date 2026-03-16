/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Commands                           Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Layer supertype                         *
*  Type     : CommandResult                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO used to return the results of a command execution.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Commands {

  /// <summary>Output DTO used to return the results of a command execution.</summary>
  public class CommandResult<T> {

    public CommandResult(T entity, FixedList<CommandTotals> totals,
                         bool isRunning = false,
                         int transactionsCount = 1,
                         FixedList<NamedEntityDto> errors = null,
                         FixedList<NamedEntityDto> warnings = null) {
      Assertion.Require(entity, nameof(entity));
      Assertion.Require(totals, nameof(totals));

      Entity = entity;
      TransactionTotals = totals ?? new FixedList<CommandTotals>();
      IsRunning = isRunning;
      TransactionsCount = transactionsCount;
      Errors = errors ?? new FixedList<NamedEntityDto>();
      Warnings = warnings ?? new FixedList<NamedEntityDto>();
    }

    [Newtonsoft.Json.JsonIgnore]
    public T Entity {
      get; internal set;
    }


    public bool HasErrors {
      get {
        return Errors.Count != 0;
      }
    }


    public bool IsRunning {
      get;
    }


    public FixedList<NamedEntityDto> Errors {
      get;
    }


    public FixedList<NamedEntityDto> Warnings {
      get;
    }


    public int TransactionsCount {
      get;
    }


    public FixedList<CommandTotals> TransactionTotals {
      get;
    }

  }  // class CommandResult



  /// <summary>Contains command totals data identified with some unique id.</summary>
  public class CommandTotals {

    public CommandTotals(string uid, string description,
                         int transactionsCount = 1,
                         int processedCount = 1,
                         int errorsCount = 0, int warningsCount = 0) {

      Assertion.Require(uid, nameof(uid));
      Assertion.Require(description, nameof(description));

      UID = uid;
      Description = description;
      TransactionsCount = transactionsCount;
      ProcessedCount = processedCount;
      ErrorsCount = errorsCount;
      WarningsCount = warningsCount;
    }

    public string UID {
      get;
    }


    public string Description {
      get;
    }


    public int TransactionsCount {
      get;
    }


    public int ProcessedCount {
      get;
    }


    public int ErrorsCount {
      get;
    }


    public int WarningsCount {
      get;
    }

  }  // class CommandTotals

}  // namespace Empiria.Commands
