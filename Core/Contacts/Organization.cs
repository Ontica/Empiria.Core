/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 License  : Please read LICENSE.txt file      *
*  Type      : Organization                                     Pattern  : Ontology Object Type              *
*                                                                                                            *
*  Summary   : Represents a government entity or agency, an enterprise or a non-profit organization.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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
