/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : IFilter                                          Pattern  : Loose coupling interface          *
*                                                                                                            *
*  Summary   : Represents a filter condition or criteria for entities.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Represents a filter condition or criteria for entities.</summary>
  public interface IFilter {

    #region Members

    string Value {
      get;
    }

    #endregion Members

  }  // interface IFilter

}  // namespace Empiria
