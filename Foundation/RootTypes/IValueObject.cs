/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : IValueObject                                     Pattern  : Separated Interface               *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Interface that represents an Empiria Framework value object. Could be used with value types   *
*              (struct) or with immutable reference types. Value objects are a central concept in            *
*              Domain-Driven Design (look for DDD and Eric Evans' book).                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
