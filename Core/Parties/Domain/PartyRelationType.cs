/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Power type                              *
*  Type     : PartyRelationType                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a relation between parties.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria.Parties {

  /// <summary>Power type that describes a relation between parties.</summary>
  [Powertype(typeof(PartyRelation))]
  public class PartyRelationType : Powertype {

    #region Constructors and parsers

    private PartyRelationType() {
      // Empiria powertype types always have this constructor.
    }

    static public new PartyRelationType Parse(int typeId) {
      return ObjectTypeInfo.Parse<PartyRelationType>(typeId);
    }

    static public new PartyRelationType Parse(string typeName) {
      return PartyType.Parse<PartyRelationType>(typeName);
    }

    #endregion Constructors and parsers

  } // class PartyRelationType

} // namespace Empiria.Parties
