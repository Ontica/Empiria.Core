﻿/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology.Modeler                         Assembly : Empiria.dll                       *
*  Type      : DataMapping                                      Pattern  : Standard class                    *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Mapping rule between a type property or field and a data source element.                      *
*                                                                                                            *
********************************* Copyright (c) 2014-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

using Empiria.Data;
using Empiria.Json;
using Empiria.Reflection;

namespace Empiria.Ontology.Modeler {

  /// <summary>Mapping rule between a type property or field and a data source element.</summary>
  internal abstract class DataMapping {

    #region Abstract members

    internal abstract MemberInfo MemberInfo { get; }
    internal abstract Type MemberType { get; }

    protected abstract object ImplementsGetValue(object instance);
    protected abstract void ImplementsSetValue(object instance, object value);

    #endregion Abstract members

    #region Constructors and parsers

    protected DataMapping() {

    }

    //Returns a data mapping for a type's memberInfo (field or property)
    internal static DataMapping Parse(Type type, MemberInfo memberInfo) {
      DataMapping dataMapping = null;
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
        dataMapping = new DataPropertyMapping((PropertyInfo) declaredMemberInfo);
      } else if (memberInfo is FieldInfo) {
        dataMapping = new DataFieldMapping((FieldInfo) declaredMemberInfo);
      } else {
        Assertion.AssertNoReachThisCode();
      }
      dataMapping.SetRules();

      return dataMapping;
    }

    #endregion Constructors and parsers

    #region Public properties

    internal bool ApplyOnInitialization {
      get;
      private set;
    }

    internal DataColumn DataColumn {
      get;
      private set;
    }

    internal int DataColumnIndex {
      get;
      private set;
    }

    internal DataFieldAttribute DataFieldAttribute {
      get;
      set;
    }

    public string DataFieldAttributeName {
      get {
        return this.DataFieldAttribute.Name;
      }
    }

    internal string DataFieldName {
      get;
      private set;
    }

    internal Type DataFieldType {
      get;
      private set;
    }

    private object _defaultValue = null;
    private Func<object> defaultValueDelegate = null;

    internal object DefaultValue {
      get {
        if (defaultValueDelegate != null) {
          return defaultValueDelegate.Invoke();
        } else {
          return _defaultValue;
        }
      }
      private set {
        _defaultValue = value;
      }
    }

    internal string JsonFieldName {
      get;
      private set;
    }

    internal string JsonInnerFieldName {
      get;
      private set;
    }

    internal bool MapToChar {
      get;
      private set;
    }

    internal bool MapToEmptyObject {
      get;
      private set;
    }

    internal bool MapToEnumeration {
      get;
      private set;
    }

    internal bool MapToJsonItem {
      get;
      private set;
    }

    internal bool MapToLazyInstance {
      get;
      private set;
    }

    public bool MapToParsableObject {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    internal string GetExecutionData() {
      string str = String.Empty;

      str = String.Format("Mapped type member: {0}\n", this.MemberInfo.Name);
      str += String.Format("Mapped data field: {0}\n", this.DataFieldName);
      if (this.JsonInnerFieldName.Length != 0) {
        str += String.Format("Mapped Json source field: {0}\n", this.JsonFieldName);
        str += String.Format("Mapped Json item: {0}\n", this.JsonInnerFieldName);
      }
      return str;
    }

    internal void MapDataColumn(DataColumn dataColumn) {
      this.DataColumn = dataColumn;
      this.DataColumnIndex = this.DataColumn.Ordinal;
      this.DataFieldName = this.DataColumn.ColumnName;
      this.DataFieldType = this.DataColumn.DataType;

      if (this.MapToLazyInstance) {
        Assertion.Assert(this.DataFieldType == typeof(int),
                         "LazyObjects can only be parsed from integer type data columns.");
      }
      if (this.MapToParsableObject) {
        Assertion.Assert(this.DataFieldType == typeof(int),
                         this.MemberInfo.Name + " can only be parsed from an integer type data column.");
      }

      if (this.MapToJsonItem) {
        Assertion.Assert(this.DataFieldType == typeof(string),
                         "Json items can only be parsed from string type data columns.");
      }
      if (this.MapToEnumeration) {
        Assertion.Assert(this.DataFieldType == typeof(string) || this.DataFieldType == typeof(char),
                         "Enumeration items can only be parsed from char(1) or string type data columns.");
      }
      if (this.MapToChar) {
        Assertion.Assert(this.DataFieldType == typeof(string) || this.DataFieldType == typeof(char),
                         "Char value items can only be parsed from char(1) or string type data columns.");
      }
    }

    /// <summary>Set instance member value according to implicit or explicit default values rules.</summary>
    internal void SetDefaultValue(object instance) {
      this.ImplementsSetValue(instance, this.DefaultValue);
    }

    /// <summary>Set instance member value from a Json string according to this DataMapping rule.
    ///  Only for use when MapToJsonItem is true.</summary>
    internal void SetJsonValue(object instance, string jsonStringValue,
                               Dictionary<string, JsonObject> jsonObjectsCache) {
      //Assert(this.MapToJsonItem, "Method for use only when this.MapToJsonItem is true.");
      if (jsonStringValue.Length == 0) {
        return;   // If not value, do nothing?
      }
      JsonObject jsonObject = GetJsonObject(jsonObjectsCache, jsonStringValue);

      if (this.JsonInnerFieldName.Length != 0) {
        object memberValue = this.ExtractJsonInnerFieldValue(jsonObject);
        memberValue = this.TransformDataStoredValueBeforeAssignToMember(memberValue);
        this.ImplementsSetValue(instance, memberValue);
      } else {
        this.ImplementsSetValue(instance, jsonObject);
      }
    }

    /// <summary>Set instance member value according to this DataMapping rule.
    /// Only for use when MapToJsonItem is false.</summary>
    internal void SetValue(object instance, object value) {
      //Assert(this.MapToJsonItem == false, "Method for use only when this.MapToJsonItem is false.");
      object memberValue = this.TransformDataStoredValueBeforeAssignToMember(value);
      this.ImplementsSetValue(instance, memberValue);
    }

    #endregion Public methods

    #region Private members

    MethodInfo _jsonGetItemMethod = null;
    /// <summary>Returns method Empiria.Data.JsonObject.Get<T>(string itemPath)</summary>
    private MethodInfo JsonGetItemMethod {
      get {
        if (_jsonGetItemMethod == null) {
          var method = typeof(JsonObject).GetMethod("Get", new Type[] { typeof(string) });
          Assertion.AssertObject(method, "Expected generic 'Get<T>(string)' method is not " +
                                         "defined in type JsonObject.");
          _jsonGetItemMethod = method.MakeGenericMethod(this.MemberType);

        }
        return _jsonGetItemMethod;
      }
    }

    MethodInfo _jsonGetItemMethodWDefault = null;
    /// <summary>Returns method Empiria.Data.JsonObject.Get<T>(string itemPath, T defaultValue)</summary>
    private MethodInfo JsonGetItemMethodWithDefault {
      get {
        if (_jsonGetItemMethodWDefault == null) {
          MethodInfo method = typeof(JsonObject).GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                                .Where(x => x.IsGenericMethod && x.Name == "Get" &&
                                                            x.GetParameters().Length == 2)
                                                .Single();
          Assertion.AssertObject(method, "Expected generic 'Get<T>(string, T)' method is not " +
                                         "defined in type JsonObject.");
          _jsonGetItemMethodWDefault = method.MakeGenericMethod(this.MemberType);
        }
        return _jsonGetItemMethodWDefault;
      }
    }

    /// <summary>Gets a Json item from a jsonString using a JsonObject's cache to avoid
    /// unnecessary parsing.</summary>
    private object ExtractJsonInnerFieldValue(JsonObject jsonObject) {
      if (this.DataFieldAttribute.IsOptional) {
        var parameters = new object[] { this.JsonInnerFieldName, this.DefaultValue };
        return this.JsonGetItemMethodWithDefault.Invoke(jsonObject, parameters);
      } else {
        var parameter = new object[] { this.JsonInnerFieldName };
        return this.JsonGetItemMethod.Invoke(jsonObject, parameter);
      }
    }

    /// <summary>Gets a JsonObject from a cache. If the object is null then this method
    /// initalizes it using the jsonString value.</summary>
    private JsonObject GetJsonObject(Dictionary<string, JsonObject> jsonObjectsCache,
                                     string jsonStringValue) {
      var jsonObject = jsonObjectsCache[this.DataFieldName];
      if (jsonObject == null) {
        jsonObject = JsonObject.Parse(jsonStringValue);
        jsonObjectsCache[this.DataFieldName] = jsonObject;
      }
      return jsonObject;
    }

    private object GetDataFieldDefaultValue() {
      if (this.DataFieldAttribute.Default == null) {
        // If there is not defined a default value, then simply return the predefined value
        // for instances of type this.MemberType.
        return DataMapping.GetTypeDefaultValue(this.MemberType);
      }

      Type defaultValueType = this.DataFieldAttribute.Default.GetType();
      if (defaultValueType != this.MemberType && defaultValueType == typeof(string)) {
        //Builds a delegate in order to execute it when this.DefaultValue has been invoked.
        defaultValueDelegate = GetDelegateForDefaultValue();
        return defaultValueDelegate.Invoke();
      } else if (defaultValueType != this.MemberType && defaultValueType != typeof(string)) {
        //Convert the default value to member type
        return System.Convert.ChangeType(this.DataFieldAttribute.Default, this.MemberType);
      } else {
        return this.DataFieldAttribute.Default;
      }
    }

    private Func<object> GetDelegateForDefaultValue() {
      string defaultValueCode = (string) this.DataFieldAttribute.Default;
      try {
        string typeName = defaultValueCode.Substring(0, defaultValueCode.LastIndexOf('.'));
        string propertyName = defaultValueCode.Substring(defaultValueCode.LastIndexOf('.') + 1);

        Type type = MetaModelType.TryGetSystemType(typeName);
        Assertion.AssertObject(type, "type");

        PropertyInfo propertyInfo = MethodInvoker.GetStaticProperty(type, propertyName);

        return MethodInvoker.GetStaticPropertyValueMethodDelegate(propertyInfo);
      } catch (Exception e) {
        throw new OntologyException(OntologyException.Msg.CannotParsePropertyForDefaultValue, e,
                                    this.MemberInfo.DeclaringType, this.MemberInfo.Name,
                                    defaultValueCode, this.GetExecutionData());
      }
    }

    static private object GetTypeDefaultValue(Type type) {
      if (type == typeof(string)) {
        return String.Empty;
      } else if (type == typeof(int)) {
        return (int) 0;
      } else if (ObjectFactory.HasEmptyInstance(type)) {
        return ObjectFactory.EmptyInstance(type);
      } else if (type == typeof(DateTime)) {
        return ExecutionServer.DateMaxValue;
      } else if (type == typeof(bool)) {
        return false;
      } else if (type == typeof(decimal)) {
        return decimal.Zero;
      } else {
        throw new OntologyException(OntologyException.Msg.CannotGetDefaultValueforType, type.FullName);
      }
    }

    private void SetRules() {
      // Initialize fields with default vlaues;
      this.DataColumnIndex = -1;
      this.DataFieldName = String.Empty;
      this.JsonFieldName = String.Empty;
      this.JsonInnerFieldName = String.Empty;

      // Set rules
      this.ApplyOnInitialization = (this.MemberInfo is PropertyInfo);
      this.DataFieldAttribute = this.MemberInfo.GetCustomAttribute<DataFieldAttribute>();
      this.MapToLazyInstance = (this.MemberType.IsGenericType &&
                                this.MemberType.GetGenericTypeDefinition() == typeof(LazyInstance<>));
      this.MapToParsableObject = ObjectFactory.HasParseWithIdMethod(this.MemberType);
      this.MapToEmptyObject = ObjectFactory.HasEmptyInstance(this.MemberType);
      this.MapToEnumeration = this.MemberType.IsEnum;
      this.MapToChar = (this.MemberType == typeof(char));

      this.DefaultValue = this.GetDataFieldDefaultValue();

      if (this.MemberType == typeof(JsonObject) || this.DataFieldAttribute.Name.Contains('.')) {
        string dataFieldAttrName = this.DataFieldAttribute.Name;

        int indexOfPeriod = dataFieldAttrName.IndexOf('.');
        if (indexOfPeriod != -1) {
          // [DataField("ContactExtData.Address")] <- returns a a string or Address object
          this.JsonFieldName = dataFieldAttrName.Substring(0, dataFieldAttrName.IndexOf('.'));
          this.JsonInnerFieldName = dataFieldAttrName.Substring(dataFieldAttrName.IndexOf('.') + 1);
        } else {
          // [DataField("ContactExtData")] <- returns a JsonObject, JsonInnerFieldName is empty.
          this.JsonFieldName = dataFieldAttrName;
          this.JsonInnerFieldName = String.Empty;
        }
        this.MapToJsonItem = true;
      }
    }

    private object TransformDataStoredValueBeforeAssignToMember(object value) {
      if (this.MapToParsableObject || this.MapToLazyInstance) {
        int objectId = (int) value;
        if (objectId == -1 && this.MapToEmptyObject) {
          return this.GetEmptyInstanceDelegate();   ///ObjectFactory.EmptyInstance(this.MemberType);
        } else {
          return this.GetParseInstanceDelegate(objectId);  //ParseMemberTypeDelegate.DynamicInvoke(objectId);
        }
      } else if (this.MapToEnumeration) {
        if (((string) value).Length == 1) {
          return Enum.ToObject(this.MemberType, Convert.ToChar(value));
        } else {
          return Enum.Parse(this.MemberType, (string) value);
        }
      } else if (this.MapToChar) {
        return Convert.ToChar((string) value);
      } else {
        return value;
      }
    }

    private delegate object EmptyMethodDelegate();
    EmptyMethodDelegate _emptyMethodDelegate = null;
    private EmptyMethodDelegate GetEmptyInstanceDelegate {
      get {
        if (_emptyMethodDelegate == null) {
          _emptyMethodDelegate =
              (EmptyMethodDelegate) Delegate.CreateDelegate(typeof(EmptyMethodDelegate),
                                    ObjectFactory.GetEmptyInstanceProperty(this.MemberType).GetGetMethod());
        }
        return _emptyMethodDelegate;
      }
    }

    private delegate object ParseMethodDelegate(int id);
    ParseMethodDelegate _parseMethodDelegate = null;
    private ParseMethodDelegate GetParseInstanceDelegate {
      get {
        if (_parseMethodDelegate == null) {
          _parseMethodDelegate =
              (ParseMethodDelegate) Delegate.CreateDelegate(typeof(ParseMethodDelegate),
                                                            ObjectFactory.GetParseMethod(this.MemberType));
        }
        return _parseMethodDelegate;
      }
    }

    #endregion Private members

  } // class DataMapping

} // namespace Empiria.Ontology.Modeler
