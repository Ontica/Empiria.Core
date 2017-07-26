/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : KeyValueList                                     Pattern  : Storage Item                      *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a list that holds KeyValue instances.                                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
