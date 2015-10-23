/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 Assembly : Empiria.Foundation.dll            *
*  Type      : Organization                                     Pattern  : Ontology Object Type              *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a government entity or agency, an enterprise or a non-profit organization.         *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Contacts {

  public class Organization : Contact {

    #region Constructors and parsers

    protected Organization() {
      // Required by Empiria Framework.
    }

    static public new Organization Parse(int id) {
      return BaseObject.ParseId<Organization>(id);
    }

    static private readonly Organization _empty = BaseObject.ParseEmpty<Organization>();
    static public new Organization Empty {
      get {
        return _empty.Clone<Organization>();
      }
    }

    static private readonly Organization _unknown = BaseObject.ParseUnknown<Organization>();
    static public Organization Unknown {
      get {
        return _unknown.Clone<Organization>();
      }
    }

    #endregion Constructors and parsers

  } // class Organization

} // namespace Empiria.Contacts
