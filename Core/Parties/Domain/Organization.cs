/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : Organization                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a government entity or agency, an enterprise or a non-profit organization.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Parties {

  /// <summary>Represents a government entity or agency, an organizational unit an enterprise or
  /// a non-profit organization.</summary>
  public class Organization : Party {

    #region Constructors and parsers

    protected Organization() {
      // Required by Empiria Framework.
    }


    protected Organization(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Organization Parse(int id) {
      return BaseObject.ParseId<Organization>(id);
    }

    static public new Organization Parse(string uid) {
      return BaseObject.ParseKey<Organization>(uid);
    }

    static public new Organization Empty => BaseObject.ParseEmpty<Organization>();

    #endregion Constructors and parsers

  } // class Organization

} // namespace Empiria.Parties
