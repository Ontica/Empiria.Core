/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : Group                                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a group of parties.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Represents a group of parties.</summary>
  public class Group : Party {

    #region Constructors and parsers

    protected Group() {
      // Required by Empiria Framework.
    }


    protected Group(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public new Group Parse(int id) => ParseId<Group>(id);

    static public new Group Parse(string uid) => ParseKey<Group>(uid);

    public Group(PartyFields fields) {
      Assertion.Require(fields, nameof(fields));

      Update(fields);
    }

    static public new Group Empty => ParseEmpty<Group>();

    #endregion Constructors and parsers

    #region Properties

    public FixedList<Party> Members {
      get {
        return ExtendedData.GetFixedList<Party>("groupMembers");
      }
    }


    public override string Keywords {
      get {
        var temp = string.Empty;

        foreach (var member in Members) {
          temp += member.Keywords;
        }

        return EmpiriaString.BuildKeywords(temp);
      }
    }

    #endregion Properties

  } // class Group

} // namespace Empiria.Parties
