/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : FixedListComparer                                Pattern  : Empiria List Class                *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Performs comparison of two FixedList objects.                                                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

namespace Empiria {

  public enum MergeResultType {
    Undefined = 'U',
    ExactMatch = 'M',
    ConditionFails = 'C',
    UnmatchedObjectA = 'A',
    UnmatchedObjectB = 'B',
  }

  public class Merge<T> where T : IIdentifiable {

    private string mergeKey = String.Empty;
    private T objectA = default(T);
    private T objectB = default(T);
    private MergeResultType result = MergeResultType.Undefined;

    internal Merge(string mergeKey, T objectA, T objectB, MergeResultType result) {
      this.mergeKey = mergeKey;
      this.objectA = objectA;
      this.objectB = objectB;
      this.result = result;
    }

    public string MergeKey {
      get { return mergeKey; }
    }

    public T ObjectA {
      get { return objectA; }
    }

    public T ObjectB {
      get { return objectB; }
    }

    public MergeResultType Result {
      get { return result; }
    }

  } // Merge

} // namespace Empiria
