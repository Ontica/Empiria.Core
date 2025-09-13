/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Object-relational mapping         *
*  Namespace : Empiria.ORM                                      License  : Please read LICENSE.txt file      *
*  Type      : DataMappingRules                                 Pattern  : Standard class                    *
*                                                                                                            *
*  Summary   : Holds data mapping rules for a giving type using DataFieldAttribute decorators,               *
*              and performs data loading for those type instances.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Empiria.Json;

namespace Empiria.ORM {

  /// <summary>Holds data mapping rules for a giving type using DataField type attributes decorators,
  /// and performs data loading for those type instances.</summary>
  public class DataMappingRules {

    #region Fields

    private readonly Type mappedType = null;
    private readonly DataMapping[] dataMappingsArray = null;
    private readonly DataObjectMapping[] innerDataObjectsMappingsArray = null;

    private List<string> jsonFieldsNames = null;
    private bool dataColumnsAreMapped = false;

    #endregion Fields

    #region Constructors and parsers

    private DataMappingRules(Type type) {
      this.mappedType = type;
      this.dataMappingsArray = this.GetTypeMappings();
      this.innerDataObjectsMappingsArray = this.GetTypeInnerDataObjectsMappings();
    }

    static public DataMappingRules Parse(Type type) {
      Assertion.Require(type, "type");

      return new DataMappingRules(type);
    }

    static internal bool IsDataBound(Type type) {
      var boundedMembers = DataMappingRules.GetDataboundPropertiesAndFields(type);
      var boundedObjects = DataMappingRules.GetDataboundInnerObjects(type);

      return ((boundedMembers.Length + boundedObjects.Length) != 0);
    }

    #endregion Constructors and parsers

    #region Public methods

    internal void InitializeObject(object instance) {
      this.InitializeDataMappings(instance);
      this.InitializeDataObjects(instance);
    }

    /// <summary>Binds DataRow data into the instance fields and properties marked
    /// with the DataField attribute.</summary>
    public void DataBind(object instance, DataRow dataRow) {
      this.DataBindFieldsAndProperties(instance, dataRow);
      this.DataBindInnerDataObjects(instance, dataRow);
    }

    /// <summary>Binds DataRow data into the instance fields and properties marked
    /// with the DataField attribute.</summary>
    private void DataBindFieldsAndProperties(object instance, DataRow dataRow) {
      if (!dataColumnsAreMapped) {
        lock (dataMappingsArray) {
          this.MapDataColumns(dataRow.Table.Columns);
        }
      }

      DataMapping rule = null;
      try {
        Dictionary<string, JsonObject> jsonObjectsCache = this.CreateJsonObjectsCache();
        for (int i = 0; i < dataMappingsArray.Length; i++) {
          rule = dataMappingsArray[i];

          if (rule.MapToJsonItem) {
            rule.SetJsonValue(instance, EmpiriaString.ToString(dataRow[rule.DataColumnIndex]), jsonObjectsCache);
          } else {
            rule.SetNoJsonValue(instance, dataRow[rule.DataColumnIndex]);
          }
        }
      } catch (Exception e) {
        throw DataMappingException.GetDataValueMappingException(instance, rule, e);
      }
    }

    /// <summary>Binds DataRow data into the instance fields and properties marked
    /// with the DataObject attribute.</summary>
    private void DataBindInnerDataObjects(object instance, DataRow dataRow) {
      DataObjectMapping rule = null;
      try {
        for (int i = 0; i < innerDataObjectsMappingsArray.Length; i++) {
          rule = innerDataObjectsMappingsArray[i];
          rule.DataBind(instance, dataRow);
        }
      } catch (Exception e) {
        throw DataMappingException.GetDataValueMappingException(instance, rule, e);
      }
    }

    private void InitializeDataObjects(object instance) {
      DataObjectMapping rule = null;
      try {
        for (int i = 0; i < innerDataObjectsMappingsArray.Length; i++) {
          rule = innerDataObjectsMappingsArray[i];
          rule.SetDefaultValue(instance);
        }
      } catch (Exception e) {
        throw DataMappingException.GetInitializeObjectException(instance, rule, e);
      }
    }

    private void InitializeDataMappings(object instance) {
      DataMapping rule = null;
      try {
        for (int i = 0; i < dataMappingsArray.Length; i++) {
          rule = dataMappingsArray[i];
          if (rule.ApplyOnInitialization) {
            rule.SetDefaultValue(instance);
          }
        }
      } catch (Exception e) {
        throw DataMappingException.GetInitializeObjectException(instance, rule, e);
      }
    }

    #endregion Public methods

    #region Private methods

    private Dictionary<string, JsonObject> CreateJsonObjectsCache() {
      var comparer = StringComparer.OrdinalIgnoreCase;

      if (jsonFieldsNames == null) {
        return new Dictionary<string, JsonObject>(0, comparer);
      }

      var dictionary = new Dictionary<string, JsonObject>(jsonFieldsNames.Count, comparer);

      foreach (string item in jsonFieldsNames) {
        dictionary.Add(item, null);   // Items are nulls because JsonObject instances are
                                      // created inside the method DataMapping.SetValue
      }
      return dictionary;
    }

    /// <summary>Gets an array with all type properties and fields that have defined
    ///a DataField attribute, ordered by DataField attribute's name.</summary>
    static private MemberInfo[] GetDataboundPropertiesAndFields(Type type) {
      Type searchType = type;

      List<MemberInfo> list = new List<MemberInfo>();
      while (!searchType.Equals(typeof(object))) {
        var members = searchType.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                            BindingFlags.Public | BindingFlags.NonPublic)
                                .Where(x => Attribute.IsDefined(x, typeof(DataFieldAttribute)))
                                .OrderBy((x) => x.GetCustomAttribute<DataFieldAttribute>().Name).ToArray();
        list.AddRange(members);
        searchType = searchType.BaseType;
      }
      return list.ToArray();
    }

    /// <summary>Gets an array with all type's data bounded objects described
    /// using a DataObject attribute.</summary>
    static private MemberInfo[] GetDataboundInnerObjects(Type type) {
      Type searchType = type;

      List<MemberInfo> list = new List<MemberInfo>();
      while (!searchType.Equals(typeof(object))) {
        var members = searchType.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                            BindingFlags.Public | BindingFlags.NonPublic)
                                .Where(x => Attribute.IsDefined(x, typeof(DataObjectAttribute))).ToArray();
        list.AddRange(members);
        searchType = searchType.BaseType;
      }
      return list.ToArray();
    }

    /// <summary>Gets an array of DataMapping objects, derived from those type members
    /// that have a DataField attribute.</summary>
    private DataMapping[] GetTypeMappings() {
      MemberInfo memberInfo = null;
      try {
        var databoundMembers = DataMappingRules.GetDataboundPropertiesAndFields(mappedType);

        var dataMappingsList = new List<DataMapping>(databoundMembers.Length);
        for (int i = 0; i < databoundMembers.Length; i++) {
          memberInfo = databoundMembers[i];

          var dataMapping = DataMapping.Parse(mappedType, memberInfo);
          dataMappingsList.Add(dataMapping);

          if (dataMapping.MapToJsonItem) {
            this.TryToAddFieldNameToJsonCacheKeys(dataMapping.JsonFieldName);
          }
        }  // for
        return dataMappingsList.ToArray();
      } catch (Exception e) {
        throw new DataMappingException(DataMappingException.Msg.TypeMemberMappingFails, e,
                                       mappedType.FullName, memberInfo.Name);
      }
    }

    private DataObjectMapping[] GetTypeInnerDataObjectsMappings() {
      MemberInfo memberInfo = null;
      try {
        var databoundMembers = DataMappingRules.GetDataboundInnerObjects(mappedType);

        var dataObjectMappings = new List<DataObjectMapping>(databoundMembers.Length);

        for (int i = 0; i < databoundMembers.Length; i++) {
          memberInfo = databoundMembers[i];

          var dataMapping = DataObjectMapping.Parse(mappedType, memberInfo);
          dataObjectMappings.Add(dataMapping);
        }  // for
        return dataObjectMappings.ToArray();
      } catch (Exception e) {
        throw new DataMappingException(DataMappingException.Msg.TypeMemberMappingFails, e,
                                       mappedType.FullName, memberInfo.Name);
      }
    }

    ///// <summary>Gets a list of DataMapping items, mapping each of the type's databound members
    ///// (those with the DataField attribute) with elements of the data columns collection.</summary>
    private void MapDataColumns(DataColumnCollection dataColumns) {
      if (dataColumnsAreMapped) {
        return;
      }

      foreach (DataMapping mapping in dataMappingsArray) {
        int columnIndex = dataColumns.IndexOf(mapping.DataFieldAttributeName);

        if (columnIndex == -1 && mapping.DataFieldAttribute.IsOptional) {

          DataColumn column = new DataColumn(mapping.DataFieldAttributeName,
                                            mapping.DefaultValue.GetType());
          dataColumns.Add(column);
          mapping.MapDataColumn(column);

          continue;
        }

        if (columnIndex != -1) {
          mapping.MapDataColumn(dataColumns[columnIndex]);

        } else if (mapping.MapToJsonItem) {
          columnIndex = dataColumns.IndexOf(mapping.JsonFieldName);
          if (columnIndex != -1) {
            mapping.MapDataColumn(dataColumns[columnIndex]);
          } else {
            throw new DataMappingException(DataMappingException.Msg.MappingDataColumnNotFound,
                                           mappedType.Name, mapping.MemberInfo.Name,
                                           mapping.JsonFieldName);
          }  // inner if

        } else {
          throw new DataMappingException(DataMappingException.Msg.MappingDataColumnNotFound,
                                         mappedType.Name, mapping.MemberInfo.Name,
                                         mapping.DataFieldAttributeName);
        }  // main if
      } // foreach
      this.dataColumnsAreMapped = true;
    }

    private void TryToAddFieldNameToJsonCacheKeys(string jsonBaseFieldName) {
      if (jsonFieldsNames == null) {
        jsonFieldsNames = new List<string>(1);
      }
      if (!jsonFieldsNames.Contains(jsonBaseFieldName)) {
        lock (jsonFieldsNames) {
          if (!jsonFieldsNames.Contains(jsonBaseFieldName)) {
            jsonFieldsNames.Add(jsonBaseFieldName);
          }
        }
      }
    }

    #endregion Private methods

  } // class DataMappingRules

} // namespace Empiria.ORM
