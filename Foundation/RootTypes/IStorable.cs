﻿/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : IStorable                                        Pattern  : Loose coupling interface          *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Interface that represents a storable identifiable object.                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
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