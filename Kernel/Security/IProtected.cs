/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IProtected                                       Pattern  : Loose coupling interface          *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This interface serves to control the data integrity of a stored entity.                       *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System.Security.Principal;

namespace Empiria.Security {

  /// <summary>This interface serves to control the data integrity of a stored entity.</summary>
  public interface IProtected : IIdentifiable {

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