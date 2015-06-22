/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : IFilter                                          Pattern  : Loose coupling interface          *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a filter condition or criteria for entities.                                       *
*                                                                                                            *
********************************* Copyright (c) 1999-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
