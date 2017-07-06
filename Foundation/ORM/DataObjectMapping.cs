/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Object-relational mapping         *
*  Namespace : Empiria.ORM                                      Assembly : Empiria.Foundation.dll            *
*  Type      : DataFieldMapping                                 Pattern  : Standard class                    *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Mapping rule between a type field and a data source element.                                  *
*                                                                                                            *
********************************* Copyright (c) 2014-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using System.Reflection;

using Empiria.Reflection;

namespace Empiria.ORM {

  /// <summary>Mapping rule between a type property and a data source element.</summary>
  internal class DataObjectMapping {

    #region Fields

    private PropertyInfo propertyInfo = null;
    private Type memberType = null;
    private Func<object, object> getValueMethodDelegate;
    private Action<object, object> setValueMethodDelegate;
    private DataMappingRules dataMappingRules = null;

    #endregion Fields

    #region Constructors and parsers

    protected internal DataObjectMapping(PropertyInfo propertyInfo) {
      this.propertyInfo = propertyInfo;
      this.memberType = this.propertyInfo.PropertyType;
      this.getValueMethodDelegate = MethodInvoker.GetPropertyValueMethodDelegate(this.propertyInfo);
      this.setValueMethodDelegate = MethodInvoker.SetPropertyValueMethodDelegate(this.propertyInfo);

      dataMappingRules = DataMappingRules.Parse(memberType);
    }

    //Returns a data mapping for a type's memberInfo (field or property)
    internal static DataObjectMapping Parse(Type type, MemberInfo memberInfo) {
      DataObjectMapping dataMapping = null;
      MemberInfo declaredMemberInfo = null;

      //Use DeclaringType for access to sets/gets of private properties defined in base types of 'type'
      if (memberInfo.DeclaringType != type && memberInfo is PropertyInfo) {
        declaredMemberInfo =
                  memberInfo.DeclaringType.GetProperty(memberInfo.Name, BindingFlags.Instance |
                                                                        BindingFlags.Public |
                                                                        BindingFlags.NonPublic);
      } else {
        declaredMemberInfo = memberInfo;
      }

      if (memberInfo is PropertyInfo) {
        dataMapping = new DataObjectMapping((PropertyInfo) declaredMemberInfo);
      } else if (memberInfo is FieldInfo) {
        throw new NotImplementedException();
      } else {
        Assertion.AssertNoReachThisCode();
      }
      return dataMapping;
    }

    #endregion Constructors and parsers

    #region Public properties

    internal MemberInfo MemberInfo {
      get {
        return propertyInfo;
      }
    }

    internal Type MemberType {
      get {
        return memberType;
      }
    }

    #endregion Public properties

    #region Public methods

    internal void DataBind(object instance, DataRow dataRow) {
      var value = ObjectFactory.CreateObject(memberType);

      dataMappingRules.DataBind(value, dataRow);

      ImplementsSetValue(instance, value);
    }

    internal string GetExecutionData() {
      return String.Format("Mapped type member: {0}\n", this.MemberInfo.Name);
    }

    /// <summary>Set instance member value according to implicit or explicit default values rules.</summary>
    internal void SetDefaultValue(object instance) {
      var defaultValue = ObjectFactory.CreateObject(memberType);

      this.ImplementsSetValue(instance, defaultValue);
    }

    protected object ImplementsGetValue(object instance) {
      return getValueMethodDelegate.Invoke(instance);
    }

    protected void ImplementsSetValue(object instance, object value) {
      setValueMethodDelegate.Invoke(instance, value);
    }

    #endregion Public methods

  } // class DataObjectMapping

} // namespace Empiria.ORM
