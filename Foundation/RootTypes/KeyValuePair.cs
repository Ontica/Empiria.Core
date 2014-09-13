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

    #region Constructors and parsers

    private KeyValuePair() {
      // Required by Empiria Framework.
    }

    static public KeyValuePair Parse(int id) {
      return BaseObject.ParseId<KeyValuePair>(id);
    }

    static public KeyValuePair Empty {
      get { return BaseObject.ParseEmpty<KeyValuePair>(); }
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

    public string Value {
      get { return base.ValueField; }
      private set { base.ValueField = value; }
    }

    #endregion Public properties

  } // class KeyValuePair

} // namespace Empiria
