/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : ValuesTableStucture                              Pattern  : Data Type                         *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Defines a values table structure.                                                             *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using Empiria.Data;
using Empiria.Ontology;

namespace Empiria.DataTypes {

  public class ValuesTableColumn {
    public bool IsNumeric = false;
    public string FieldName = String.Empty;
    public string Name = String.Empty;
    public FundamentalTypeInfo TypeInfo = FundamentalTypeInfo.Parse("Decimal");
  }

  /// <summary>Defines a values table structure.</summary>
  public class ValuesTableStucture {

    #region Fields

    static private readonly string[] tableFields = new string[] { "RangeDisplayName", "LowerBound", "UpperBound", 
                                                                   "Column0", "Column1", "Column2", "Column3",
                                                                   "Column4", "Column5" };


    private string tableName = String.Empty;
    private bool findByRange = false;

    private ValuesTableColumnCollection columns = new ValuesTableColumnCollection();

    private DataTable valuesDataTable = new DataTable();

    #endregion Fields

    #region Constructors and parsers

    private ValuesTableStucture() {
      // External creation of instances of this class not allowed
    }

    static public ValuesTableStucture Parse(string tableName) {
      return Parse(tableName, DateTime.Today);
    }

    static public ValuesTableStucture Parse(string tableName, DateTime valuesDate) {
      ValuesTableStucture valuesTable = new ValuesTableStucture();
      valuesTable.tableName = tableName;

      valuesTable.LoadColumnsStructure();
      valuesTable.LoadColumnsValues(valuesDate);

      return valuesTable;
    }

    #endregion Constructors and parsers

    #region Public properties

    public ValuesTableColumnCollection Columns {
      get { return this.columns; }
    }

    public DataTable DataTable {
      get { return this.valuesDataTable; }
    }

    public bool FindByRange {
      get { return this.findByRange; }
    }

    public string Name {
      get { return this.tableName; }
    }

    #endregion Public properties

    #region Internal methods

    internal object FindValue(string columnName, decimal onRangeValue) {
      if (this.findByRange) {
        return FindValueOnDecimalRange(columnName, onRangeValue);
      } else {
        return FindValueOnDecimalRow(columnName, onRangeValue);
      }
    }

    internal object FindValue(string columnName, string onRangeValue) {
      if (this.findByRange) {
        return FindValueOnStringRange(columnName, onRangeValue);
      } else {
        return FindValueOnStringRow(columnName, onRangeValue);
      }
    }

    #endregion Internal methods

    #region Private methods

    private object FindValueOnDecimalRange(string columnName, decimal onRangeValue) {
      for (int i = 0; i < valuesDataTable.Rows.Count; i++) {
        decimal lowerBound = Convert.ToDecimal(valuesDataTable.Rows[i]["LowerBound"]);
        decimal upperBound = Convert.ToDecimal(valuesDataTable.Rows[i]["UpperBound"]);
        if (lowerBound <= onRangeValue && onRangeValue <= upperBound) {
          return valuesDataTable.Rows[i][this.columns.GetColumn(columnName).FieldName];
        }
      }
      return null;
    }

    private object FindValueOnDecimalRow(string columnName, decimal rowValue) {
      for (int i = 0; i < valuesDataTable.Rows.Count; i++) {
        decimal value = Convert.ToDecimal(valuesDataTable.Rows[i]["LowerBound"]);
        if (rowValue == value) {
          return valuesDataTable.Rows[i][this.columns.GetColumn(columnName).FieldName];
        }
      }
      return null;
    }

    private object FindValueOnStringRange(string columnName, string onRangeValue) {
      for (int i = 0; i < valuesDataTable.Rows.Count; i++) {
        string lowerBound = Convert.ToString(valuesDataTable.Rows[i]["LowerBound"]);
        string upperBound = Convert.ToString(valuesDataTable.Rows[i]["UpperBound"]);
        if ((lowerBound.CompareTo(onRangeValue) <= 0) && onRangeValue.CompareTo(upperBound) <= 0) {
          return valuesDataTable.Rows[i][this.columns.GetColumn(columnName).FieldName];
        }
      }
      return null;
    }

    private object FindValueOnStringRow(string columnName, string rowValue) {
      for (int i = 0; i < valuesDataTable.Rows.Count; i++) {
        string value = Convert.ToString(valuesDataTable.Rows[i]["LowerBound"]);
        if (rowValue == value) {
          return valuesDataTable.Rows[i][this.columns.GetColumn(columnName).FieldName];
        }
      }
      return null;
    }

    private void LoadColumnsStructure() {
      DataRow tableColumnNames = GeneralDataOperations.GetEntityByKeyFiltered("EOSRangesTables", "RangesTableName",
                                                                              this.tableName, "RangeType = 'N'");

      DataRow tableColumnTypes = GeneralDataOperations.GetEntityByKeyFiltered("EOSRangesTables", "RangesTableName",
                                                                 this.tableName, "RangeType = 'T'");

      if (((string) tableColumnNames["UpperBound"]).Length != 0) {
        findByRange = true;
      }
      for (int i = 0; i < tableFields.Length; i++) {
        string currentFieldName = tableFields[i];
        if (((string) tableColumnNames[currentFieldName]).Length != 0) {
          ValuesTableColumn column = new ValuesTableColumn();
          column.Name = (string) tableColumnNames[currentFieldName];
          column.FieldName = currentFieldName;
          column.TypeInfo = FundamentalTypeInfo.Parse(Convert.ToInt32((string) tableColumnTypes[currentFieldName]));
          column.IsNumeric = (column.TypeInfo.Name == "Decimal" ||
                              column.TypeInfo.Name == "Int32" || column.TypeInfo.Name == "Int16");
          this.columns.AddColumn(column);
        } // if
      } // for
    }

    private void LoadColumnsValues(DateTime valuesDate) {
      valuesDataTable = DataReader.GetDataTable(DataOperation.Parse("qryEOSRangeTableValues", this.tableName, valuesDate));
    }

    #endregion Private methods

  }  // class ValuesTableStucture

} // namespace Empiria.DataTypes