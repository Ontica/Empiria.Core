/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : DataObjectAttribute                              Pattern  : Attribute class                   *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Marks an object property or field for data loading.                                           *
*                                                                                                            *
********************************* Copyright (c) 2014-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
