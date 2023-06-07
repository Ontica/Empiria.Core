/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Types                                 Component : Base Object Management                  *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Interface                          *
*  Type     : IIdentifiable                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an entity with an integer Id and a unique ID string.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary> Represents an entity with an integer Id and a unique ID string.</summary>
  public interface IIdentifiable: IUniqueID {

    int Id {
      get;
    }

  } // interface IIdentifiable

} // namespace Empiria
