/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : MergeResultType                                  Pattern  : Enumeration type                  *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Describes the result of a merging operation of an object that belongs to one or two lists.    *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
