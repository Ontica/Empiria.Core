/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology.Modeler                         Assembly : Empiria.dll                       *
*  Type      : DataPropertyMapping                              Pattern  : Standard class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Mapping rule between a type property and a data source element.                               *
*                                                                                                            *
********************************* Copyright (c) 2014-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

using Empiria.Reflection;

namespace Empiria.Ontology.Modeler {

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

} // namespace Empiria.Ontology.Modeler
