/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : AccessMode                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the access level permissions of a single entity.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Describes the access level permissions of a single entity.</summary>
  public enum AccessMode {

    Empty = 'E',

    Public = 'P',

    Internal = 'I',

    Private = 'R',

  }  // enum AccessMode

}  // namespace Empiria
