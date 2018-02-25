/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : Multiple General Purpose Enumerations            Pattern  : None                              *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

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
