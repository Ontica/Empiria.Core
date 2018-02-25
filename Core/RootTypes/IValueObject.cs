/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : IValueObject                                     Pattern  : Separated Interface               *
*                                                                                                            *
*  Summary   : Interface that represents an Empiria Framework value object. Could be used with value types   *
*              (struct) or with immutable reference types. Value objects are a central concept in            *
*              Domain-Driven Design (look for DDD and Eric Evans' book).                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Interface that represents an Empiria Framework value object. Could be used with C# value types
  /// or with immutable reference types. Value objects are a central concept in Domain-Driven Design.</summary>
  public interface IValueObject {

    /// <summary>True if the value corresponds to the empty or null instance, false otherwise.</summary>
    bool IsEmptyValue { get; }

    /// <summary>True if the value was already registered in a catalogue, false otherwise.</summary>
    bool IsRegistered { get; }

  } // interface IValueObject

}  // namespace Empiria
