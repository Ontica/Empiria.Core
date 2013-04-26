/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : Multiple General Purpose Enumerations            Pattern  : None                              *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/

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
    Inactive = 'I',
    Active = 'A',
    Pending = 'P',
    Suspended = 'S',
    Deprecated = 'D',
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