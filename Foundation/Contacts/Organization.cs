/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 Assembly : Empiria.dll                       *
*  Type      : Organization                                     Pattern  : Ontology Object Type              *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a government entity or agency, an enterprise or a non-profit organization.         *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System.Data;

namespace Empiria.Contacts {

  public class Organization : Contact {

    #region Fields

    private const string thisTypeName = "ObjectType.Contact.Organization";

    #endregion Fields

    #region Constructors and parsers

    protected Organization() : base(thisTypeName) {

    }

    protected Organization(string typeName) : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public Organization Empty {
      get { return BaseObject.ParseEmpty<Organization>(thisTypeName); }
    }

    static public new Organization Parse(int id) {
      return BaseObject.Parse<Organization>(thisTypeName, id);
    }

    #endregion Constructors and parsers

  } // class Organization

} // namespace Empiria.Contacts
