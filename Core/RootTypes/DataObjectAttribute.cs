/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : DataObjectAttribute                              Pattern  : Attribute class                   *
*                                                                                                            *
*  Summary   : Marks an object property or field for data loading.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Marks an object property or field for data loading.</summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class DataObjectAttribute : Attribute {

    /// <summary>Initializes a new instance of the <see cref="DataObjectAttribute"/> class.</summary>
    public DataObjectAttribute() {

    }

  }  // class DataObjectAttribute

}  // namespace Empiria
