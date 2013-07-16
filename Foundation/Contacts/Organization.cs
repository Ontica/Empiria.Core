/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 Assembly : Empiria.dll                       *
*  Type      : Organization                                     Pattern  : Ontology Object Type              *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a government entity or agency, an enterprise or a non-profit organization.         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System.Data;

namespace Empiria.Contacts {

  public class Organization : Contact {

    #region Fields

    private const string thisTypeName = "ObjectType.Contact.Organization";

    #endregion Fields

    #region Constructors and parsers

    protected Organization()
      : base(thisTypeName) {

    }

    protected Organization(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public Organization Empty {
      get { return BaseObject.ParseEmpty<Organization>(thisTypeName); }
    }

    static public new Organization Parse(int id) {
      return BaseObject.Parse<Organization>(thisTypeName, id);
    }

    #endregion Constructors and parsers

    #region Public properties

    #endregion Public properties

    #region Public methods

    protected override void ImplementsLoadObjectData(DataRow row) {
      base.ImplementsLoadObjectData(row);
    }

    protected override void ImplementsSave() {
      base.ImplementsSave();
    }

    #endregion Public methods

  } // class Organization

} // namespace Empiria.Contacts