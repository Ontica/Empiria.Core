/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Power type                            *
*  Type     : PartyType                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Power type that describes a Party partitioned type. A party can be a person, an organization,  *
*             a team, a sales region, etc. Parties play one or more roles in parties relationships.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Parties {

  /// <summary>Power type that describes a Party partitioned type. A party can be a person, an organization,
  /// a team, a sales region, etc. Parties play one or more roles in parties relationships.</summary>
  [Powertype(typeof(Party))]
  public class PartyType : Powertype {

    #region Constructors and parsers

    private PartyType() {
      // Empiria powertype types always have this constructor.
    }

    static public new PartyType Parse(int typeId) {
      return ObjectTypeInfo.Parse<PartyType>(typeId);
    }

    static public new PartyType Parse(string typeName) {
      return PartyType.Parse<PartyType>(typeName);
    }

    #endregion Constructors and parsers

  } // class Party

} // namespace Empiria.Parties
