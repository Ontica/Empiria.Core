/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : ThreeStateValue                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes a three state value.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.StateEnums {

  /// <summary>Describes a three state value.</summary>
  public enum ThreeStateValue {

    True = 1,

    False = 0,

    Unknown = -1,

  }  // enum ThreeStateValue



  /// <summary>Extension methods for ThreeStateValueExtensions type.</summary>
  static public class ThreeStateValueExtensions {

    static public string GetName(this ThreeStateValue stateValue) {

      switch (stateValue) {

        case ThreeStateValue.True:
          return "Sí";

        case ThreeStateValue.False:
          return "No";

        case ThreeStateValue.Unknown:
          return "No determinado";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized three state value {stateValue}.");
      }

    }


    static public NamedEntityDto MapToNamedEntity(this ThreeStateValue stateValue) {
      return new NamedEntityDto(stateValue.ToString(), stateValue.GetName());
    }

  }  //  class ThreeStateValueExtensions

}  // namespace Empiria.StateEnums
