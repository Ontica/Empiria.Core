/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base Types                                   Component : Ontology                              *
*  Assembly : Empiria.Core.dll                             Pattern   : Interface                             *
*  Type     : IAggregateRoot                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Interface that represents an aggregate root.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Interface that represents an aggregate root.</summary>
  public interface IAggregateRoot : IIdentifiable {

    #region Members

    /// <summary>Event that must be triggered when the root entity is saved.</summary>
    event EventHandler SaveAllCalled;

    /// <summary>Method that must be invoked to save the whole aggregate.</summary>
    void SaveAll();

    #endregion Members

  } // interface IAggregateRoot

} // namespace Empiria
