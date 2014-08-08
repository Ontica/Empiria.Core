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

using Empiria.Reflection;

namespace Empiria.Ontology.Modeler {

  /// <summary>Mapping rule between a type property or field and a data source element.</summary>
  internal class DataMapping {

    #region Constuctors and parsers

    internal DataMapping(MemberInfo memberInfo, DataColumn dataColumn, int dataColumnIndex) {
      this.MemberInfo = memberInfo;
      this.DataColumn = dataColumn;
      this.DataColumnIndex = dataColumnIndex;
      this.DataFieldName = DataColumn.ColumnName;
      this.DataFieldType = DataColumn.DataType;

      if (this.MemberInfo is PropertyInfo) {
        this.MapToProperty = true;
        this.MemberType = ((PropertyInfo) this.MemberInfo).PropertyType;
      } else {
        this.MapToProperty = false;
        this.MemberType = ((FieldInfo) this.MemberInfo).FieldType;
      }
      if (this.DataFieldType == typeof(int)) {
        this.MapToLazyObject = (this.MemberType.IsGenericType &&
                                   this.MemberType.GetGenericTypeDefinition() == typeof(LazyObject<>));
        this.MapToBaseObject = (!this.MemberType.IsGenericType &&
                                    this.MemberType.IsSubclassOf(typeof(Empiria.BaseObject)));
      }

      this.MapCharToEnum = (this.MemberType.IsEnum && this.DataFieldType == typeof(string));
    }

    #endregion Constructors and parsers

    #region Public properties

    internal DataColumn DataColumn {
      get;
      private set;
    }

    internal int DataColumnIndex {
      get;
      private set;
    }

    internal string DataFieldName {
      get;
      private set;
    }

    internal Type DataFieldType {
      get;
      private set;
    }

    internal bool MapCharToEnum {
      get;
      private set;
    }

    internal bool MapToProperty {
      get;
      private set;
    }

    public bool MapToBaseObject {
      get;
      private set;
    }

    internal bool MapToLazyObject {
      get;
      private set;
    }

    internal MemberInfo MemberInfo {
      get;
      private set;
    }

    internal Type MemberType {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    internal void InvokeSetValue(object instance, object value) {
      object setValue = null;

      if (this.MapToBaseObject || this.MapToLazyObject) {
        setValue = ObjectFactory.ParseObject(this.MemberType, (int) value);
      } else  if (this.MapCharToEnum) {
        if (((string) value).Length == 1) {
          setValue = Enum.ToObject(this.MemberType, Convert.ToChar(value));
        } else {
          setValue = Enum.Parse(this.MemberType, (string) value);
        }
      } else {
        setValue = value;
      }
      if (this.MapToProperty) {
        ((PropertyInfo) this.MemberInfo).SetMethod.Invoke(instance, new[] { setValue });
      } else {      // IsMemberField = true
        ((FieldInfo) this.MemberInfo).SetValue(instance, setValue);
      }
    }

    #endregion Public methods

  } // class DataMappingRules

} // namespace Empiria.Ontology.Modeler
