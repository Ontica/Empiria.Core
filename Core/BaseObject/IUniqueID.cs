/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Types                                 Component : Base Object Management                  *
*  Assembly : Empiria.Core.dll                           Pattern   : Data interface                          *
*  Type     : IUniqueID                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Identifies an entity with a unique (usually a GUID) ID string.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Identifies an entity with a unique (usually a GUID) ID string.</summary>
  public interface IUniqueID {

    string UID {
      get;
    }

  } // interface IUniqueID

} // namespace Empiria
