/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Object-relational mapping         *
*  Namespace : Empiria.ORM                                      Assembly : Empiria.Foundation.dll            *
*  Type      : DataPropertyMapping                              Pattern  : Standard class                    *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Mapping rule between a type property and a data source element.                               *
*                                                                                                            *
********************************* Copyright (c) 2014-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

using Empiria.Reflection;

namespace Empiria.ORM {

  /// <summary>Mapping rule between a type property and a data source element.</summary>
  internal class DataPropertyMapping : DataMapping {

    #region Fields

    private PropertyInfo propertyInfo = null;
    private Type memberType = null;
    private Func<object, object> getValueMethodDelegate;
    private Action<object, object> setValueMethodDelegate;

    #endregion Fields

    #region Constructors and parsers

    protected internal DataPropertyMapping(PropertyInfo propertyInfo) {
      this.propertyInfo = propertyInfo;
      this.memberType = this.propertyInfo.PropertyType;
      this.getValueMethodDelegate = MethodInvoker.GetPropertyValueMethodDelegate(this.propertyInfo);
      this.setValueMethodDelegate = MethodInvoker.SetPropertyValueMethodDelegate(this.propertyInfo);
    }

    #endregion Constructors and parsers

    #region Public properties

    internal override MemberInfo MemberInfo {
      get { return propertyInfo; }
    }

    internal override Type MemberType {
      get { return memberType; }
    }

    #endregion Public properties

    #region Public methods

    protected override object ImplementsGetValue(object instance) {
      return getValueMethodDelegate.Invoke(instance);
    }

    protected override void ImplementsSetValue(object instance, object value) {
      setValueMethodDelegate.Invoke(instance, value);
    }

    #endregion Public methods

  } // class DataPropertyMapping

} // namespace Empiria.ORM
