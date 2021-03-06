﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : IExtensible<T>                                   Pattern  : Loose coupling interface          *
*                                                                                                            *
*  Summary   : Interface that represents an object with an extensible set of data attributes.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Interface that represents an object with an extensible set of data attributes.</summary>
  public interface IExtensible<T> where T : IExtensibleData {

    #region Members definition
    T ExtensionData { get; }

    #endregion Members definition

  }  // interface IExtensible<T>

  /// <summary>Decorator that represents the data set of attributes used by a IExtensible type.</summary>
  public interface IExtensibleData {

    #region Members definition

    string ToJson();

    //void ParseJson(string json);

    //void IExtensibleData.FromJson(string json) {
    //  throw new NotImplementedException();
    //}

    //static internal BillStamp Parse(string json) {
    //  return Empiria.Data.JsonConverter.ToObject<BillStamp>(json);
    //}

    #endregion Members definition

  } // interface IExtensibleData

} // namespace Empiria
