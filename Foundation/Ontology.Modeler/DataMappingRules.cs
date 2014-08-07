/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology.Modeler                         Assembly : Empiria.dll                       *
*  Type      : DataMappingRules                                 Pattern  : Standard class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Holds data mapping rules for a giving type using DataFieldAttribute decorators.               *
*                                                                                                            *
********************************* Copyright (c) 2014-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

using Empiria.Reflection;

namespace Empiria.Ontology.Modeler {

  internal class DataMappingRules {

    #region Fields

    private DataMapping[] _dataRules = null;

    private DataMappingRules(Type type, DataColumnCollection dataColumns) {
      this.InitializeRules(type, dataColumns);
    }

    #endregion Fields

    #region Constuctors and parsers

    static internal DataMappingRules Parse(Type type, DataColumnCollection dataColumns) {
      return new DataMappingRules(type, dataColumns);
    }

    #endregion Constructors and parsers

    #region Public properties

    public int Count {
      get { return _dataRules.Length; }
    }

    #endregion Public properties

    #region Public methods

    internal void LoadObject(BaseObject instance, DataRow dataRow) {
      for (int i = 0; i < _dataRules.Length; i++) {
        var rule = _dataRules[i];
        if (rule != null) {
          rule.InvokeSetValue(instance, dataRow[rule.DataColumnIndex]);
        }
      }
    }

    #endregion Public methods

    #region Private methods

    private static MemberInfo[] GetDataboundMembers(Type type) {
      return type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                 .Where(x => Attribute.IsDefined(x, typeof(DataFieldAttribute))).ToArray();
    }

    private void InitializeRules(Type type, DataColumnCollection dataColumns) {
      var databoundMembers = DataMappingRules.GetDataboundMembers(type);

      _dataRules = new DataMapping[databoundMembers.Length];

      for (int i = 0; i < databoundMembers.Length; i++) {
        MemberInfo memberInfo = databoundMembers[i];
        string dataFieldName = memberInfo.GetCustomAttribute<DataFieldAttribute>().Name;
        int index = dataColumns.IndexOf(dataFieldName);
        if (index != -1) {
          _dataRules[i] = new DataMapping(memberInfo, dataColumns[index], index);
        }
      }  // for
    }

    #endregion Private methods

  } // class DataMappingRules

} // namespace Empiria.Ontology.Modeler
