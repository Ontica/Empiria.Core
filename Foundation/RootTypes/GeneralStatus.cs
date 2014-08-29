/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : Multiple General Purpose Enumerations            Pattern  : None                              *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria {

  #region Enumerations

  public enum GeneralActivityStatus {
    Inactive = 'I',
    Active = 'A',
    Suspended = 'S',
    Completed = 'C',
    Canceled = 'L',
    Deleted = 'X',
  }

  public enum GeneralDocumentStatus {
    Inactive = 'I',
    OnProcess = 'O',
    Pending = 'P',
    Suspended = 'S',
    Completed = 'C',
    Canceled = 'L',
    Archived = 'H',
    Deleted = 'X',
  }

  public enum GeneralObjectStatus {
    Pending = 'P',
    Active = 'A',
    Suspended = 'S',
    Obsolete = 'O',
    Deleted = 'X',
  }

  public enum GeneralProcessStatus {
    Initiated = 'I',
    Running = 'R',
    Active = 'A',
    Suspended = 'S',
    Complete = 'C',
    Terminated = 'T',
    Archived = 'H',
    Canceled = 'L',
    Deleted = 'X',
  }

  #endregion Enumerations

} // namespace Empiria
