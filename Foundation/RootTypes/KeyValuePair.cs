/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : KeyValuePair                                     Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a stored key-value pair that can be used as list items.                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria {

  /// <summary>Represents a stored key-value pair that can be used as list items.</summary>
  public class KeyValuePair : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.KeyValuePair";

    #endregion Fields

    #region Constructors and parsers

    public KeyValuePair()
      : base(thisTypeName) {

    }

    protected KeyValuePair(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public KeyValuePair Parse(int id) {
      return BaseObject.Parse<KeyValuePair>(thisTypeName, id);
    }

    static internal KeyValuePair Parse(DataRow row) {
      return BaseObject.Parse<KeyValuePair>(thisTypeName, row);
    }

    static public KeyValuePair Empty {
      get { return BaseObject.ParseEmpty<KeyValuePair>(thisTypeName); }
    }

    static public FixedList<KeyValuePair> GetList(string listNamedKey) {
      GeneralList list = GeneralList.Parse(listNamedKey);

      return Ontology.OntologyData.GetKeyValueListItems(list);
    }

    #endregion Constructors and parsers

    #region Public properties

    public string Key {
      get { return base.NamedKey; }
      private set { base.NamedKey = value; }
    }

    public new string Value {
      get { return base.Value; }
      private set { base.Value = value; }
    }

    #endregion Public properties

  } // class KeyValuePair

} // namespace Empiria
