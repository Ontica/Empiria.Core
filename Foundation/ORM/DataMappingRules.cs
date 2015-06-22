/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Object-relational mapping         *
*  Namespace : Empiria.ORM                                      Assembly : Empiria.Foundation.dll            *
*  Type      : DataMappingRules                                 Pattern  : Standard class                    *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Holds data mapping rules for a giving type using DataFieldAttribute decorators,               *
*              and performs those type instances data loading.                                               *
*                                                                                                            *
********************************* Copyright (c) 2014-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

using Empiria.Data;
using Empiria.Json;
using Empiria.Reflection;

namespace Empiria.ORM {

  /// <summary>Holds data mapping rules for a giving type using DataField type attributes decorators,
  /// and performs those type instances data loading.</summary>
  internal class DataMappingRules {

    #region Fields

    private Type mappedType = null;
    private DataMapping[] dataMappingsArray = null;
    private List<string> jsonFieldsNames = null;
    private bool dataColumnsAreMapped = false;

    #endregion Fields

    #region Constructors and parsers

    private DataMappingRules(Type type) {
      this.mappedType = type;
      this.dataMappingsArray = this.GetTypeMappings();
    }

    static internal DataMappingRules Parse(Type type) {
      return new DataMappingRules(type);
    }

    static internal bool IsDataBound(Type type) {
      var boundedMembers = DataMappingRules.GetDataboundPropertiesAndFields(type);

      return (boundedMembers.Length != 0);
    }

    #endregion Constructors and parsers

    #region Public methods

    internal void InitializeObject(object instance) {
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

    /// <summary>Binds DataRow data into the instance fields and properties marked
    /// with the DataField attribute.</summary>
    internal void DataBind(object instance, DataRow dataRow) {
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
            rule.SetJsonValue(instance, (string) dataRow[rule.DataColumnIndex], jsonObjectsCache);
          } else {
            rule.SetValue(instance, dataRow[rule.DataColumnIndex]);
          }
        }
      } catch (Exception e) {
        throw DataMappingException.GetDataValueMappingException(instance, rule, e);
      }
    }

    #endregion Public methods

    #region Private methods

    private Dictionary<string, JsonObject> CreateJsonObjectsCache() {
      if (jsonFieldsNames == null) {
        return new Dictionary<string, JsonObject>(0);
      }
      var dictionary = new Dictionary<string, JsonObject>(jsonFieldsNames.Count);
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

    ///// <summary>Gets a list of DataMapping items, mapping each of the type's databound members
    ///// (those with the DataField attribute) with elements of the data columns collection.</summary>
    private void MapDataColumns(DataColumnCollection dataColumns) {
      if (dataColumnsAreMapped) {
        return;
      }

      foreach(DataMapping mapping in dataMappingsArray) {
        int columnIndex = dataColumns.IndexOf(mapping.DataFieldAttributeName);
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
