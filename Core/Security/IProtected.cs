/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : IProtected                                       Pattern  : Loose coupling interface          *
*                                                                                                            *
*  Summary   : This interface serves to control the data integrity of a stored entity.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>This interface serves to control the data integrity of a stored entity.</summary>
  public interface IProtected {

    #region Members definition

    object[] GetDataIntegrityFieldValues(int version);

    int CurrentDataIntegrityVersion {
      get;
    }

    IntegrityValidator Integrity {
      get;
    }

    #endregion Members definition

  }  // interface IProtected

} //namespace Empiria.Security
