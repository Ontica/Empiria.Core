/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Object-relational mapping         *
*  Namespace : Empiria.ORM                                      License  : Please read LICENSE.txt file      *
*  Type      : DataFieldMapping                                 Pattern  : Standard class                    *
*                                                                                                            *
*  Summary   : Mapping rule between a type field and a data source element.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

using Empiria.Reflection;

namespace Empiria.ORM {

  /// <summary>Mapping rule between a type field and a data source element.</summary>
  internal class DataFieldMapping : DataMapping {

    #region Fields

    private FieldInfo fieldInfo = null;
    private Type memberType = null;
    private Action<object, object> setValueMethodDelegate;

    #endregion Fields

    #region Constructors and parsers

    internal DataFieldMapping(FieldInfo fieldInfo) {
      this.fieldInfo = fieldInfo;
      this.memberType = fieldInfo.FieldType;
      this.setValueMethodDelegate = MethodInvoker.GetFieldValueSetMethodDelegate(this.fieldInfo);
    }

    #endregion Constructors and parsers

    #region Public properties

    internal override MemberInfo MemberInfo {
      get { return fieldInfo; }
    }

    internal override Type MemberType {
      get { return memberType; }
    }

    #endregion Public properties

    #region Public methods

    protected override object ImplementsGetValue(object instance) {
      return fieldInfo.GetValue(instance);
    }

    protected override void ImplementsSetValue(object instance, object value) {
      setValueMethodDelegate.Invoke(instance, value);
    }

    #endregion Public methods

  } // class DataFieldMapping

} // namespace Empiria.ORM
