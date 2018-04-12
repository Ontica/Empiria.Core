/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : ObjectStatus                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary   : General purpose enumeration that serves to describe the status of an object.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  #region Enumerations

  /// <summary>General purpose enumeration that serves to describe the status of an object.</summary>
  public enum ObjectStatus {

    Pending = 'P',

    Active = 'A',

    Closed = 'C',

    Suspended = 'S',

    Obsolete = 'O',

    Deleted = 'X',
  }

  #endregion Enumerations

} // namespace Empiria
