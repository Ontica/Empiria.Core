/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              License  : Please read LICENSE.txt file      *
*  Type      : MergeResultType                                  Pattern  : Enumeration type                  *
*                                                                                                            *
*  Summary   : Describes the result of a merging operation of an object that belongs to one or two lists.    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Collections {

  /// <summary>Describes the result of a merging operation of an object
  /// that belongs to one or two lists.</summary>
  public enum MergeResultType {

    /// <summary>The item belongs to both lists.</summary>
    ExactMatch = 'M',

    /// <summary>Condition to check failed. It was not possible to determinate if the item
    /// belongs to either list.</summary>
    ConditionFails = 'C',

    /// <summary>The item is in the right list but it is not in the left one.</summary>
    LeftObjectUnmatched = 'L',

    /// <summary>The item is in the left list but it is not in the right one.</summary>
    RightObjectUnmatched = 'R',

  }  // enum MergeResultType

} // namespace Empiria.Collections
