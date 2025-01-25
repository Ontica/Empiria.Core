/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : System                                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a system.                                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Represents a system.</summary>
  public class System : Party {

    #region Constructors and parsers

    protected System() {
      // Required by Empiria Framework.
    }

    protected System(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new System Parse(int id) => ParseId<System>(id);

    static public new System Parse(string uid) => ParseKey<System>(uid);

    static public new System Empty => ParseEmpty<System>();

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

    #endregion Properties

  } // class System

} // namespace Empiria.Parties
