/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : MergeResult                                      Pattern  : Standard Class                    *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Holds information about two merged objects and the result of their comparision.              *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
