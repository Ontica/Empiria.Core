/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : OpenCloseStatus                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the status of an object that can be opened an closed.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.StateEnums {

  /// <summary>Describes the status of an object that can be opened an closed.</summary>
  public enum OpenCloseStatus {

    Opened = 'O',

    Closed = 'C',

    Deleted = 'X',

  } // OpenCloseStatus

} // namespace Empiria.StateEnums
