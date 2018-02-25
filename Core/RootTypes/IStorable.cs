/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Storage Services                  *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : IStorable                                        Pattern  : Loose coupling interface          *
*                                                                                                            *
*  Summary   : Interface that represents a storable identifiable object.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Interface that represents a storable identifiable object.</summary>
  public interface IStorable : IIdentifiable {

    #region Members definition

    //StorageVersion Version { get; }

    //void Save();

    #endregion Members definition

  } // interface IStorable

} // namespace Empiria
