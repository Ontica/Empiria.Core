/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Contacts Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : System                                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an information system.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Contacts {

  /// <summary>Represents a government entity or agency, an enterprise or a non-profit organization.</summary>
  public class System : Contact {

    #region Constructors and parsers

    protected System() {
      // Required by Empiria Framework.
    }


    static public new System Parse(int id) {
      return BaseObject.ParseId<System>(id);
    }

    static public new System Parse(string uid) {
      return BaseObject.ParseKey<System>(uid);
    }

    static public new System Empty => BaseObject.ParseEmpty<System>();


    #endregion Constructors and parsers

  } // class Organization

} // namespace Empiria.Contacts
