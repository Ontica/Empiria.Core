/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : ValuesTableColumnCollection                      Pattern  : Data Type                         *
*                                                                                                            *
*  Summary   : Defines a list of columns used in a general purpose ValuesTable.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Collections;

namespace Empiria.DataTypes {

  /// <summary>Defines a list of columns used in a general purpose ValuesTable.</summary>
  public class ValuesTableColumnCollection {

    #region Fields

    private EmpiriaDictionary<string, ValuesTableColumn> columns = new EmpiriaDictionary<string, ValuesTableColumn>(16);

    #endregion Fields

    #region Constructors and parsers

    public ValuesTableColumnCollection() {

    }

    #endregion Constructors and parsers

    #region Public methods

    public bool Contains(string columName) {
      return columns.ContainsKey(columName);
    }

    public ValuesTableColumn GetColumn(string columName) {
      Assertion.AssertObject(columName, "columName");

      return columns[columName];
    }

    public ValuesTableColumn[] GetColumnsArray() {
      ValuesTableColumn[] array = new ValuesTableColumn[columns.Count];

      columns.Values.CopyTo(array, 0);

      return array;
    }

    public string[] GetColumnsNameArray() {
      string[] array = new string[columns.Count];

      columns.Keys.CopyTo(array, 0);

      return array;
    }

    #endregion Public methods

    #region Internal methods

    internal void AddColumn(ValuesTableColumn column) {
      columns.Insert(column.Name, column);
    }

    internal void Clear() {
      columns.Clear();
    }

    #endregion Internal methods

  }  // class ValuesTableColumnCollection

} // namespace Empiria.DataTypes
