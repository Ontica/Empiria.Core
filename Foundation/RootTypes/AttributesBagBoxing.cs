//
//* Simple attributes bag that use an object array (boxing/unboxing). See AttributesBag.
//
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Reflection;

namespace Empiria {

  /// <summary>Simple attributes bag that use an object array (boxing/unboxing). 
  /// See AttributesBag.</summary>
  public class AttributesBagBoxing {

    #region Fields

    private object[] objectValues = null;

    #endregion Fields

    #region Constuctors and parsers

    internal AttributesBagBoxing(BaseObject instance, DataRow row) {
      if (attributeIndexes == null) {
        AttributesBagBoxing.InitializeRules(instance, row);
      }
      LoadAttributes(row);
    }

    #endregion Constructors and parsers

    #region Public properties

    #endregion Public properties
   
    #region Public methods

    public DateTime GetDateTime(string attributeName) {
      int index = attributeIndexes[attributeName];

      return (DateTime) objectValues[index];
    }

    public decimal GetDecimal(string attributeName) {
      int index = attributeIndexes[attributeName];

      return (decimal) objectValues[index];
    }

    public Int32 GetInt32(string attributeName) {
      int index = attributeIndexes[attributeName];

      return (Int32) objectValues[index];
    }

    public T GetObject<T>(string attributeName) where T : BaseObject {
      int index = attributeIndexes[attributeName];

      if (objectValues[index] is Int32) {
        lock (this) {
          objectValues[index] = ObjectFactory.ParseObject<T>((int) objectValues[index]);
        }
      }
      return (T) objectValues[index];
    }

    public string GetString(string attributeName) {
      int index = attributeIndexes[attributeName];

      return (string) objectValues[index];
    }

    internal void SetObject<T>(string attributeName, T value) {
      int index = attributeIndexes[attributeName];

      objectValues[index] = value;
    }

    #endregion Public methods

    #region Private methods

    static private Dictionary<string, int> attributeIndexes = null;

    private static void InitializeRules(BaseObject instance, DataRow row) {
      DataColumnCollection columns = row.Table.Columns;

      attributeIndexes = new Dictionary<string, int>(columns.Count);
      for (int i = 0; i < columns.Count; i++) {
        attributeIndexes.Add(columns[i].ColumnName, i);
      }  // for

    }

    private void LoadAttributes(DataRow row) {
      objectValues = new object[attributeIndexes.Count];
      for (int i = 0; i < attributeIndexes.Count; i++) {
        objectValues[i] = row[i];
      }  // for
    }

    #endregion Private methods

  } // class AttributesBag

} // namespace Empiria
