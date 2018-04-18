/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : RAGStatus                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : States something with a traffic light color.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.StateEnums {

  /// <summary>States something with a traffic light color.</summary>
  public enum RAGStatus {

    NoColor = 'N',

    Red = 'R',

    Ambar = 'A',

    Green = 'G',

  }  // enum RAGStatus

}  // namespace Empiria.StateEnums
