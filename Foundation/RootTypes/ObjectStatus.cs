﻿/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : Multiple General Purpose Enumerations            Pattern  : None                              *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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