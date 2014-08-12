/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology.Modeler                         Assembly : Empiria.dll                       *
*  Type      : DataMappingRules                                 Pattern  : Standard class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Holds data mapping rules for a giving type using DataFieldAttribute decorators,               *
*              and performs those type instances data loading.                                               *
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

  /// <summary>Holds data mapping rules for a giving type using DataField type attributes decorators,
  /// and performs those type instances data loading.</summary>
  internal class DataMappingRules {

    #region Fields

    private DataMapping[] dataRulesArray = null;
    private List<string> jsonFieldsNames = null;

    #endregion Fields

    #region Constuctors and parsers

    private DataMappingRules(Type type, DataColumnCollection dataColumns) {
      dataRulesArray = this.GetTypeDataRules(type, dataColumns);
    }

    static internal DataMappingRules Parse(Type type, DataColumnCollection dataColumns) {
      return new DataMappingRules(type, dataColumns);
    }

    #endregion Constructors and parsers

    #region Public methods

    /// <summary>Loads the dataRow values into the instance fields and properties marked
    /// with the DataField attribute.</summary>
    internal void LoadObject(object instance, DataRow dataRow) {
      DataMapping rule = null;
      try {
        Dictionary<string, JsonObject> jsonObjectsCache = this.CreateJsonObjectsCache();
        for (int i = 0; i < dataRulesArray.Length; i++) {
          rule = dataRulesArray[i];
          if (rule.MapToJsonItem) {
            rule.SetValue(instance, dataRow[rule.DataColumnIndex], jsonObjectsCache);
          } else {
            rule.SetValue(instance, dataRow[rule.DataColumnIndex]);
          }
        }
      } catch (Exception e) {
        throw this.GetCannotMapDataValueException(instance, rule, e);
        //
      }
    }

    private OntologyException GetCannotMapDataValueException(object instance, DataMapping rule,
                                                             Exception innerException) {
      string str = rule.GetExecutionData();
      str += String.Format("Instance Type: {0}\n", instance.GetType().FullName);
      if (instance is IIdentifiable) {
        str += String.Format("Instance Id: {0}\n", ((IIdentifiable) instance).Id);
      }
      throw new OntologyException(OntologyException.Msg.CannotMapDataValue, innerException, str);
    }

    #endregion Public methods

    #region Private methods

    private Dictionary<string, JsonObject> CreateJsonObjectsCache() {
      if (jsonFieldsNames == null) {
        return new Dictionary<string, JsonObject>(0);
      }
      var dictionary = new Dictionary<string, JsonObject>(jsonFieldsNames.Count);
      foreach (string item in jsonFieldsNames) {
        dictionary.Add(item, null);   // JsonObject instances are created inside DataMapping.SetValue method
      }
      return dictionary;
    }

    /// <summary>Gets an array with all type properties and fields that have defined
    ///a DataField attribute, ordered by DataField attribute's name.</summary>
    static private MemberInfo[] GetDataboundPropertiesAndFields(Type type) {
      return type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 .Where(x => Attribute.IsDefined(x, typeof(DataFieldAttribute)))
                 .OrderBy((x) => x.GetCustomAttribute<DataFieldAttribute>().Name).ToArray();
    }

    /// <summary>Gets a list of DataMapping items, mapping each of the type's databound members
    /// (those with the DataField attribute) with elements of the data columns collection.</summary>
    private DataMapping[] GetTypeDataRules(Type type, DataColumnCollection dataColumns) {
      var databoundMembers = DataMappingRules.GetDataboundPropertiesAndFields(type);

      var dataRules = new List<DataMapping>(databoundMembers.Length);
      for (int i = 0; i < databoundMembers.Length; i++) {
        MemberInfo memberInfo = databoundMembers[i];
        
        //Allows access to sets/gets of private properties defined in base types of 'type'
        if (memberInfo.DeclaringType != type && memberInfo is PropertyInfo) {
          memberInfo = memberInfo.DeclaringType.GetProperty(memberInfo.Name,
                                                            BindingFlags.Instance | BindingFlags.Public |
                                                            BindingFlags.NonPublic);
        }

        // Accept only those types properties and fields with a matching data column,
        // otherwise throws an exception.
        // That implies that there is only one dataRow source per data-mapped type, so types
        // mapped from multiple data sources are not supported in this product version.
        string dataFieldName = memberInfo.GetCustomAttribute<DataFieldAttribute>().Name;
        int columnIndex = dataColumns.IndexOf(dataFieldName);

        if (columnIndex != -1) {
          dataRules.Add(DataMapping.MapDataColumn(memberInfo, dataColumns[columnIndex]));
          continue;
        } else if (dataFieldName.Contains('.')) {
          string baseFieldName = dataFieldName.Substring(0, dataFieldName.IndexOf('.'));
          string jsonFieldName = dataFieldName.Substring(dataFieldName.IndexOf('.') + 1);
          columnIndex = dataColumns.IndexOf(baseFieldName);
          if (columnIndex != -1) {
            dataRules.Add(DataMapping.MapDataColumn(memberInfo, dataColumns[columnIndex], jsonFieldName));
            this.TryToAddFieldNameToJsonCacheKeys(baseFieldName);
          } else {
            throw new OntologyException(OntologyException.Msg.MappingDataColumnNotFound,
                                        type.Name, memberInfo.Name, baseFieldName);
          } 
        } else {
          throw new OntologyException(OntologyException.Msg.MappingDataColumnNotFound,
                                      type.Name, memberInfo.Name, dataFieldName);
        }
      }  // for
      return dataRules.ToArray();
    }

    private void TryToAddFieldNameToJsonCacheKeys(string jsonBaseFieldName) {
      if (jsonFieldsNames == null) {
        jsonFieldsNames = new List<string>(1);
      }
      lock (jsonFieldsNames) {
        if (!jsonFieldsNames.Contains(jsonBaseFieldName)) {
          jsonFieldsNames.Add(jsonBaseFieldName);
        }
      }
    }

    #endregion Private methods

  } // class DataMappingRules

} // namespace Empiria.Ontology.Modeler
