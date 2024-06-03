/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Partitioned Type                      *
*  Type     : PartyRelationRole                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a played role in a relation between parties.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Parties {

  /// <summary>Represents a played role in a relation between parties.</summary>
  public class PartyRelationRole : GeneralObject {

    #region Constructors and parsers

    private PartyRelationRole() {
      // Required by Empiria Framework.
    }

    static public PartyRelationRole Parse(int id) {
      return BaseObject.ParseId<PartyRelationRole>(id);
    }

    static public PartyRelationRole Parse(string uid) {
      return BaseObject.ParseKey<PartyRelationRole>(uid);
    }

    static public PartyRelationRole Empty {
      get {
        return BaseObject.ParseEmpty<PartyRelationRole>();
      }
    }

    #endregion Constructors and parsers

  } // class PartyRelationRole

} // namespace Empiria.Parties
