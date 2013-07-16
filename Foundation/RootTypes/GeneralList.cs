/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : GeneralList                                      Pattern  : Storage Item                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a list type that holds BaseObject instances.                                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
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

    public ObjectList<T> GetTypeRelationItems<T>() where T : TypeRelationInfo {
      ObjectList<T> list = base.GetTypeRelationLinks<T>("GeneralList_TypeRelations");

      list.Sort((x, y) => x.DisplayName.CompareTo(y.DisplayName));

      return list;
    }

    public ObjectList<KeyValuePair> GetKeyValueList() {
      return KeyValuePair.GetList(this.NamedKey);
    }

    #endregion Public methods

  } // class GeneralList

} // namespace Empiria