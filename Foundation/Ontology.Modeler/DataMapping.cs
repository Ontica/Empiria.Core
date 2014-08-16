/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology.Modeler                         Assembly : Empiria.dll                       *
*  Type      : DataMapping                                      Pattern  : Standard class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Mapping rule between a type property or field and a data source element.                      *
*                                                                                                            *
********************************* Copyright (c) 2014-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

using Empiria.Data;
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

    #region Constuctors and parsers

    protected DataMapping() {

    }

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
      dataMapping.LoadMemberRules();

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
    internal object DefaultValue {
      get {
        if (_defaultValue is PropertyInfo) {
          return ((PropertyInfo) _defaultValue).GetMethod.Invoke(null, null);
        }
        return _defaultValue;
      }
      private set {
        _defaultValue = value;
      }
    }

    internal string JsonFieldName {
      get;
      private set;
    }

    internal string JsonSourceFieldName {
      get;
      private set;
    }

    internal bool MapToChar {
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

    internal bool MapToLazyObject {
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
      if (this.JsonFieldName.Length != 0) {
        str += String.Format("Mapped Json source field: {0}\n", this.JsonSourceFieldName);
        str += String.Format("Mapped Json item: {0}\n", this.JsonFieldName);
      }
      return str;
    }

    internal void MapDataColumn(DataColumn dataColumn) {
      this.DataColumn = dataColumn;
      this.DataColumnIndex = this.DataColumn.Ordinal;
      this.DataFieldName = this.DataColumn.ColumnName;
      this.DataFieldType = this.DataColumn.DataType;

      if (this.MapToLazyObject) {
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

    /// <summary>Set instance member value according to this DataMapping rule.
    /// Only for use when MapToJsonItem is false.</summary>
    internal void SetValue(object instance, object value) {
      //Assertion.Assert(this.MapToJsonItem == false, "Method for use only when this.MapToJsonItem is false.");
      object invokeValue = this.TransformDataStoredValueBeforeAssignToMember(value);
      this.ImplementsSetValue(instance, invokeValue);
    }

    /// <summary>Set instance member value from a Json string according to this DataMapping rule.
    ///  Only for use when MapToJsonItem is true.</summary>
    internal void SetValue(object instance, object jsonString, 
                           Dictionary<string, JsonObject> jsonObjectsCache) {
      //Assertion.Assert(this.MapToJsonItem, "Method for use only when this.MapToJsonItem is true.");
      object invokeValue = this.ExtractJsonFieldValue((string) jsonString, jsonObjectsCache);
      invokeValue = this.TransformDataStoredValueBeforeAssignToMember(invokeValue);
      this.ImplementsSetValue(instance, invokeValue);
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
    private MethodInfo JsonGetItemMethodWDefault {
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
    private object ExtractJsonFieldValue(string jsonString, 
                                         Dictionary<string, JsonObject> jsonObjectsCache) {
      var jsonObject = jsonObjectsCache[this.DataFieldName];
      if (jsonObject == null) {
        jsonObject = JsonObject.Parse(jsonString);
        jsonObjectsCache[this.DataFieldName] = jsonObject;
      }
      if (this.DataFieldAttribute.IsOptional) {
        var parameters = new object[] { this.JsonFieldName, this.DefaultValue };
        return this.JsonGetItemMethodWDefault.Invoke(jsonObject, parameters);
      } else {
        var parameter = new object[] { this.JsonFieldName };
        return this.JsonGetItemMethod.Invoke(jsonObject, parameter);
      }
    }

    private object GetDataFieldDefaultValue() {
      if (this.DataFieldAttribute.Default == null) {
        // If there is not a default value, then simply return the predefined value for instances
        // of type this.MemberType.
        return DataMapping.GetTypeDefaultValue(this.MemberType);
      }

      Type defaultValueType = this.DataFieldAttribute.Default.GetType();
      if (defaultValueType != this.MemberType && defaultValueType == typeof(string)) {
        //Returns a PropertyInfo in order to execute it when this.DefaultValue has been invoked.
        return this.GetPropertyInfoForDefaultValue();
      } else if (defaultValueType != this.MemberType && defaultValueType != typeof(string)) {
        //Try to convert the default value to member type
        if (ObjectFactory.IsConvertible(defaultValueType, this.MemberType)) {
          return System.Convert.ChangeType(this.DataFieldAttribute.Default, this.MemberType);
        } else {
          throw new OntologyException(OntologyException.Msg.WrongDefaultValueType, 
                                      this.MemberInfo.DeclaringType, this.MemberInfo.Name,
                                      defaultValueType.Name, this.MemberType.Name);
        }
      } else {
        return this.DataFieldAttribute.Default;
      }
    }

    private PropertyInfo GetPropertyInfoForDefaultValue() {
      string defaultValueCode = (string) this.DataFieldAttribute.Default;
      try {
        string typeName = defaultValueCode.Substring(0, defaultValueCode.LastIndexOf('.'));
        string propertyName = defaultValueCode.Substring(defaultValueCode.LastIndexOf('.') + 1);

        Type type = MetaModelType.TryGetSystemType(typeName);
        Assertion.AssertObject(type, "type");
        return MethodInvoker.GetStaticProperty(type, propertyName);
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
      } else if (ObjectFactory.IsLazy(type)) {
        return ObjectFactory.LazyEmptyObject(type);
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

    private void LoadMemberRules() {
      this.DataColumnIndex = -1;
      this.DataFieldName = String.Empty;
      this.JsonSourceFieldName = String.Empty;
      this.JsonFieldName = String.Empty;

      this.ApplyOnInitialization = (this.MemberInfo is PropertyInfo);
      this.DataFieldAttribute = this.MemberInfo.GetCustomAttribute<DataFieldAttribute>();
      this.MapToLazyObject = ObjectFactory.IsLazy(this.MemberType);
      this.MapToParsableObject = ObjectFactory.HasParseWithIdMethod(this.MemberType);
      this.MapToEnumeration = this.MemberType.IsEnum;
      this.MapToChar = this.MemberType == typeof(char);

      this.DefaultValue = this.GetDataFieldDefaultValue();

      if (this.DataFieldAttribute.Name.Contains('.')) {
        string dataFieldAttrName = this.DataFieldAttribute.Name;

        this.JsonSourceFieldName = dataFieldAttrName.Substring(0, dataFieldAttrName.IndexOf('.'));
        this.JsonFieldName = dataFieldAttrName.Substring(dataFieldAttrName.IndexOf('.') + 1);
        this.MapToJsonItem = true;
      }
    }

    private object TransformDataStoredValueBeforeAssignToMember(object value) {
      if (this.MapToParsableObject || this.MapToLazyObject) {
        return ObjectFactory.ParseObject(this.MemberType, (int) value);
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

    #endregion Private members

  } // class DataMapping

} // namespace Empiria.Ontology.Modeler
