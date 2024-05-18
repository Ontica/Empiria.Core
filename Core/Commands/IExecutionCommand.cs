/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Commands                           Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Interface                               *
*  Type     : IExecutionCommand                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface that represents an executable command that can be invoked using dry-run.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Commands {

  /// <summary>Interface that represents an executable command that can be invoked using dry-run.</summary>
  public interface IExecutionCommand {

    string Type { get; }

    bool DryRun { get; }

  }  // interface IExecutionCommand

}  // Empiria.Commands
