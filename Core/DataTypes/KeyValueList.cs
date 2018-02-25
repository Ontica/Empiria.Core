/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : KeyValueList                                     Pattern  : Storage Item                      *
*                                                                                                            *
*  Summary   : Represents a list that holds KeyValue instances.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Represents a list that holds KeyValue instances.</summary>
  public class KeyValueList : GeneralObject {

    #region Constructors and parsers

    private KeyValueList() {
      // Required by Empiria Framework.
    }

    static public KeyValueList Parse(string listUniqueName) {
      return BaseObject.ParseKey<KeyValueList>(listUniqueName);
    }

    #endregion Constructors and parsers

    #region Public properties

    public string UniqueName {
      get { return base.NamedKey; }
    }

    #endregion Public properties

    #region Public methods

    public FixedList<KeyValue> GetItems(bool sortByValue = false) {
      var list = base.ExtendedDataField.GetList<KeyValue>("ListItems");

      if (sortByValue) {
        list.Sort( (x, y) => x.Value.CompareTo(y.Value) );
      }

      return list.ToFixedList();
    }

    #endregion Public methods

  } // class KeyValueList

} // namespace Empiria.DataTypes
