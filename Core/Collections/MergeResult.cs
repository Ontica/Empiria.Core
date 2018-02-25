/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              License  : Please read LICENSE.txt file      *
*  Type      : MergeResult                                      Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Holds information about two merged objects and the result of their comparision.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Collections {

  /// <summary>Holds information about two merged objects and the result of their comparision.</summary>
  /// <typeparam name="T">The type of the objects to be compared.</typeparam>
  public class MergeResult<T> where T : IIdentifiable {

    private string mergeKey = String.Empty;

    internal MergeResult(string mergeKey, T leftObject, T rightObject, MergeResultType result) {
      this.MergeKey = mergeKey;
      this.LeftObject = leftObject;
      this.RightObject = rightObject;
      this.Result = result;
    }

    public string MergeKey {
      get;
      private set;
    }

    public T LeftObject {
      get;
      private set;
    }

    public T RightObject {
      get;
      private set;
    }

    public MergeResultType Result {
      get;
      private set;
    }

  } // class MergeResult

} // namespace Empiria.Collections
