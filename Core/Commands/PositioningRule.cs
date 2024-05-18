/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Commands                           Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Enumeration                             *
*  Type     : PositioningRule                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumeration that describes a positioning rule of an object in an ordered list.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Commands {

  /// <summary>Enumeration that describes a positioning rule of an object in an ordered list.</summary>
  public enum PositioningRule {

    AtStart,

    BeforeOffset,

    AfterOffset,

    AtEnd,

    ByPositionValue,

    Undefined

  }  // enum PositioningRule



  /// <summary>Extension methods for PositioningRule enumeration.</summary>
  public static class PositioningRuleExtensions {

    static public bool IsAbsolute(this PositioningRule rule) {
      return rule == PositioningRule.AtStart || rule == PositioningRule.AtEnd;
    }

    static public bool UsesOffset(this PositioningRule rule) {
      return rule == PositioningRule.AfterOffset || rule == PositioningRule.BeforeOffset;
    }

    static public bool UsesPosition(this PositioningRule rule) {
      return rule == PositioningRule.ByPositionValue;
    }

  }  // class PositioningRuleExtensions

}  // namespace Empiria.Commands
