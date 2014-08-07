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
      this.MappedMemberInfo = memberInfo;
      this.DataColumn = dataColumn;
      this.DataColumnIndex = dataColumnIndex;

      this.DataFieldName = DataColumn.ColumnName;
      if (this.MappedMemberInfo is PropertyInfo) {
        this.IsProperty = true;
        this.DataType = ((PropertyInfo) this.MappedMemberInfo).PropertyType;
      } else {
        this.IsProperty = false;
        this.DataType = ((FieldInfo) this.MappedMemberInfo).FieldType;
      }
      this.SupportsLazyLoad = (this.DataType.IsGenericType &&
                               this.DataType.GetGenericTypeDefinition() == typeof(LazyObject<>));
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

    internal Type DataType {
      get;
      private set;
    }

    internal bool IsProperty {
      get;
      private set;
    }

    internal MemberInfo MappedMemberInfo {
      get;
      private set;
    }

    internal bool SupportsLazyLoad {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    internal void InvokeSetValue(object instance, object value) {
      if (this.IsProperty) {
        ((PropertyInfo) this.MappedMemberInfo).SetMethod.Invoke(instance, new[] { value });
      } else {      // IsField
        if (this.SupportsLazyLoad) {
          ((FieldInfo) this.MappedMemberInfo).SetValue(instance,
                                              ObjectFactory.ParseObject(this.DataType, (int) value));
        } else {
          ((FieldInfo) this.MappedMemberInfo).SetValue(instance, value);
        }
      }
    }

    #endregion Public methods

  } // class DataMappingRules

} // namespace Empiria.Ontology.Modeler
