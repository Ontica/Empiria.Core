/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : ValuesTableReader                                Pattern  : Data Type                         *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Defines a list of columns used in a general purpose ValuesTable.                              *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

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