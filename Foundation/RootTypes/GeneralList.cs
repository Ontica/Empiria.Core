/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : GeneralList                                      Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a list type that holds BaseObject instances.                                       *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
      return BaseObject.Parse<GeneralList>(thisTypeName, id);
    }

    static public GeneralList Parse(string listNamedKey) {
      return BaseObject.Parse<GeneralList>(thisTypeName, listNamedKey);
    }

    static public GeneralList Empty {
      get { return BaseObject.ParseEmpty<GeneralList>(thisTypeName); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public new string NamedKey {
      get { return base.NamedKey; }
    }

    #endregion Public properties

    #region Public methods

    //ToDO --- Change THIS !!!!!
    public ObjectList<T> GetContacts<T>() where T : Empiria.Contacts.Contact {
      return base.GetLinks<T>("GeneralList_Contacts");
    }

    public ObjectList<T> GetItems<T>() where T : BaseObject {
      return base.GetLinks<T>("GeneralList_Objects");
    }

    public ObjectList<TypeAssociationInfo> GetTypeRelationItems() {
      ObjectList<TypeAssociationInfo> list = 
                            base.GetTypeAssociationLinks("GeneralList_TypeRelations");

      list.Sort((x, y) => x.DisplayName.CompareTo(y.DisplayName));

      return list;
    }

    public ObjectList<KeyValuePair> GetKeyValueList() {
      return KeyValuePair.GetList(this.NamedKey);
    }

    #endregion Public methods

  } // class GeneralList

} // namespace Empiria