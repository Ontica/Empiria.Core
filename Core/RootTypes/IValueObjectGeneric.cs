/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Fundamental Types                 *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : IValueObject                                     Pattern  : Loose coupling interface          *
*                                                                                                            *
*  Summary   : Interface that represents an Empiria Framework value object. Could be used with value         *
*              types (C# struct) or with immutable reference types. Value objects are well described in      *
*              Domain-Driven Design.                                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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
