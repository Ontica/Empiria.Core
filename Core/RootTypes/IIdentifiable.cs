/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : IIdentifiable                                    Pattern  : Loose coupling interface          *
*                                                                                                            *
*  Summary   : Interface that represents an identifiable object throw an integer Id.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Interface that represents an identifiable object throw an integer Id.</summary>
  public interface IIdentifiable {

    #region Members definition

    int Id { get; }

    #endregion Members definition

  } // interface IIdentifiable

} // namespace Empiria
