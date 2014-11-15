/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : IFilter                                          Pattern  : Loose coupling interface          *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a filter condition or criteria for entities.                                       *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
