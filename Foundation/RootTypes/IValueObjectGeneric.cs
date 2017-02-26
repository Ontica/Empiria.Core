/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : IValueObject                                     Pattern  : Loose coupling interface          *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Interface that represents an Empiria Framework value object. Could be used with value         *
*              types (C# struct) or with immutable reference types. Value objects are well described in      *
*              Domain-Driven Design.                                                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  /// <summary>Interface that represents an Empiria Framework value object. Could be used with value
  /// types (C# struct) or with immutable reference types. Value objects are well described in
  /// Domain-Driven Design.</summary>
  public interface IValueObject<T> {

    T Value {
      get;
    }

  } // interface IValueObject<T>

}  // namespace Empiria
