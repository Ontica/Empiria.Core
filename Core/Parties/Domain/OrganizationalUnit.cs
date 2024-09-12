/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : OrganizationalUnit                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an organizational unit that is a part of an organization.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Parties {

  /// <summary>Represents an organizational unit that is a part of an organization.</summary>
  public class OrganizationalUnit : Party {

    #region Constructors and parsers

    protected OrganizationalUnit() {
      // Required by Empiria Framework.
    }

    protected OrganizationalUnit(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public new OrganizationalUnit Parse(int id) {
      return BaseObject.ParseId<OrganizationalUnit>(id);
    }


    static public new OrganizationalUnit Parse(string uid) {
      return BaseObject.ParseKey<OrganizationalUnit>(uid);
    }

    static public new OrganizationalUnit Empty => BaseObject.ParseEmpty<OrganizationalUnit>();

    #endregion Constructors and parsers

    #region Properties

    public string Acronym {
      get {
        return base.ExtendedData.Get("acronym", string.Empty);
      }
      set {
        base.ExtendedData.SetIfValue("acronym", value);
      }
    }


    public string Code {
      get {
        return base.ExtendedData.Get("code", string.Empty);
      }
      set {
        base.ExtendedData.SetIfValue("code", value);
      }
    }

    public string FullName {
      get {
        return $"{Code} - {Name}";
      }
    }

    #endregion Properties

  } // class OrganizationalUnit

} // namespace Empiria.Parties
