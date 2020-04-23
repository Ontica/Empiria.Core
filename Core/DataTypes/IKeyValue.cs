/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Types                                 Component : Kernel Types                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Interface                               *
*  Type     : IKeyValue<T>                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a key value pair, when key is a string and value is of a generic type.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Describes a key value pair, when key is a string and value is of a generic type.</summary>
  /// <typeparam name="T">The type of value in the key-value pair.</typeparam>
  public interface IKeyValue<T> {

    string Key {
      get;
    }

    T Value {
      get;
    }

  }

} //namespace Empiria.DataTypes
