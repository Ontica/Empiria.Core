/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : ValuesTableColumnCollection                      Pattern  : Data Type                         *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Defines a list of columns used in a general purpose ValuesTable.                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using Empiria.Collections;

namespace Empiria.DataTypes {

  /// <summary>Defines a list of columns used in a general purpose ValuesTable.</summary>
  public class ValuesTableColumnCollection : EmpiriaHashList<ValuesTableColumn> {

    #region Constructors and parsers

    public ValuesTableColumnCollection()
      : base(true) {

    }

    #endregion Constructors and parsers

    #region Public methods

    public bool Contains(string columName) {
      return base.ContainsKey(columName);
    }

    public ValuesTableColumn GetColumn(string columName) {
      return base[columName];
    }

    public ValuesTableColumn[] GetColumnsArray() {
      ValuesTableColumn[] array = new ValuesTableColumn[base.Count];

      base.Values.CopyTo(array, 0);

      return array;
    }

    public string[] GetColumnsNameArray() {
      string[] array = new string[base.Count];

      base.Keys.CopyTo(array, 0);

      return array;
    }

    #endregion Public methods

    #region Internal methods

    internal void AddColumn(ValuesTableColumn column) {
      base.Add(column.Name, column);
    }

    internal new void Clear() {
      base.Clear();
    }

    #endregion Internal methods

  }  // class ValuesTableColumnCollection

} // namespace Empiria.DataTypes
