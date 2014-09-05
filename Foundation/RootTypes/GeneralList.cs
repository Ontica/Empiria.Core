/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : GeneralList                                      Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a list type that holds BaseObject instances.                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using Empiria.Ontology;

namespace Empiria {

  /// <summary>Represents a list type that holds BaseObject instances.</summary>
  public class GeneralList : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.GeneralList";

    #endregion Fields

    #region Constructors and parsers

    public GeneralList()
      : base(thisTypeName) {

    }

    protected GeneralList(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public GeneralList Parse(int id) {
      return BaseObject.ParseId<GeneralList>(id);
    }

    static public GeneralList Parse(string listNamedKey) {
      return BaseObject.ParseKey<GeneralList>(listNamedKey);
    }

    static public GeneralList Empty {
      get { return BaseObject.ParseEmpty<GeneralList>(); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string UniqueCode {
      get { return base.NamedKey; }
    }

    #endregion Public properties

    #region Public methods

    //ToDO --- Change THIS !!!!!
    public FixedList<T> GetContacts<T>() where T : Empiria.Contacts.Contact {
      return base.GetLinks<T>("GeneralList_Contacts");
    }

    public FixedList<T> GetItems<T>() where T : BaseObject {
      return base.GetLinks<T>("GeneralList_Objects");
    }

    public FixedList<TypeAssociationInfo> GetTypeRelationItems() {
      FixedList<TypeAssociationInfo> list = 
                            base.GetTypeAssociationLinks("GeneralList_TypeRelations");

      list.Sort((x, y) => x.DisplayName.CompareTo(y.DisplayName));

      return list;
    }

    public FixedList<KeyValuePair> GetKeyValueList() {
      return KeyValuePair.GetList(this.UniqueCode);
    }

    #endregion Public methods

  } // class GeneralList

} // namespace Empiria
