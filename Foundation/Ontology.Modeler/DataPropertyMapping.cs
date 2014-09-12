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

namespace Empiria.Ontology.Modeler {

  /// <summary>Mapping rule between a type property and a data source element.</summary>
  internal class DataPropertyMapping : DataMapping {

    #region Fields

    private PropertyInfo propertyInfo = null;
    private Type memberType = null;
    private MethodInfo getMethod = null;
    private MethodInfo setMethod = null;

    #endregion Fields

    #region Constuctors and parsers

    protected internal DataPropertyMapping(PropertyInfo propertyInfo) {
      this.propertyInfo = propertyInfo;
      this.memberType = this.propertyInfo.PropertyType;
      this.getMethod = this.propertyInfo.GetMethod;
      this.setMethod = this.propertyInfo.SetMethod;
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
      return getMethod.Invoke(instance, null);
    }

    protected override void ImplementsSetValue(object instance, object value) {
     // ParseSetMethodDelegate.Invoke(instance, value);

      setMethod.Invoke(instance, new[] { value });
    }

    //Action<int> valueSetter = (Action<object>) 
    //  Delegate.CreateDelegate(typeof(Action<int>), tc, tc.GetType().GetProperty("Value").GetSetMethod());


    //private delegate void SetMethodDelegate(object instance, object value);
    //SetMethodDelegate _setMethodDelegate = null;
    //private SetMethodDelegate ParseSetMethodDelegate {
    //  get {
    //    if (_setMethodDelegate == null) {
    //      _setMethodDelegate = (SetMethodDelegate) Delegate.CreateDelegate(typeof(SetMethodDelegate),
    //                                                                       this.propertyInfo.GetSetMethod());
    //    }
    //    return _setMethodDelegate;
    //  }
    //}

    #endregion Public methods

  } // class DataPropertyMapping

} // namespace Empiria.Ontology.Modeler
