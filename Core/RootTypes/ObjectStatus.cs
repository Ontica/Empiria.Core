/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : Multiple General Purpose Enumerations            Pattern  : None                              *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  #region Enumerations

  public enum ObjectStatus {
    Pending = 'P',
    Active = 'A',
    Suspended = 'S',
    Obsolete = 'O',
    Deleted = 'X',
  }

  #endregion Enumerations

} // namespace Empiria
