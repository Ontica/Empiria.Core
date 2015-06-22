/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : FixedListComparer                                Pattern  : Empiria List Class                *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Performs comparison of two FixedList objects.                                                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

namespace Empiria {

  public delegate string MergeKeyBuilder<in T>(T obj);

  public delegate bool Predicate2<in T>(T obj1, T obj2);

  /// <summary>Performs comparison of two FixedList objects.</summary>
  public class FixedListComparer<T> where T : IIdentifiable {

    #region Fields

    private FixedList<T> listA = null;
    private FixedList<T> listB = null;
    private T nullObject = default(T);

    private List<Merge<T>> mergeResult = new List<Merge<T>>();

    #endregion Fields

    #region Constructors and parsers

    private FixedListComparer() {
      //no-op
    }

    static public FixedListComparer<T> Parse(FixedList<T> listA, FixedList<T> listB, T nullObject) {
      Assertion.AssertObject(listA, "listA");
      Assertion.AssertObject(listB, "listB");

      FixedListComparer<T> comparer = new FixedListComparer<T>();

      comparer.listA = listA;
      comparer.listB = listB;
      comparer.nullObject = nullObject;

      return comparer;
    }

    #endregion Constructors and parsers

    #region Public properties

    public FixedList<T> ListA {
      get { return listA; }
    }

    public FixedList<T> ListB {
      get { return listB; }
    }

    public T NullObject {
      get { return nullObject; }
    }

    public List<Merge<T>> GetByMatchResult(MergeResultType result) {
      return mergeResult.FindAll((x) => x.Result == result);
    }

    public List<Merge<T>> GetMatched() {
      return mergeResult.FindAll((x) => (x.ObjectA != null && x.ObjectB != null) &&
                                        (!x.ObjectA.Equals(nullObject) && !x.ObjectB.Equals(nullObject)));
    }

    public List<Merge<T>> GetMerge() {
      return mergeResult;
    }

    public List<T> GetUnmatchedInListA() {
      List<Merge<T>> unmatched = mergeResult.FindAll((x) => x.ObjectB == null || x.ObjectB.Equals(nullObject));
      List<T> temp = new List<T>(unmatched.Count);
      for (int i = 0; i < unmatched.Count; i++) {
        temp.Add(unmatched[i].ObjectA);
      }
      return temp;
    }

    public List<T> GetUnmatchedInListB() {
      List<Merge<T>> unmatched = mergeResult.FindAll((x) => x.ObjectA == null || x.ObjectA.Equals(nullObject));
      List<T> temp = new List<T>(unmatched.Count);
      for (int i = 0; i < unmatched.Count; i++) {
        temp.Add(unmatched[i].ObjectB);
      }
      return temp;
    }

    #endregion Public properties

    #region Public methods

    public void Merge(MergeKeyBuilder<T> mergeKeyBuilder, Predicate2<T> match, Predicate2<T> condition) {
      mergeResult = new List<Merge<T>>((listA.Count + this.listB.Count) / 2);

      List<T> listTemp = this.listB.FindAll((x) => true);
      for (int i = 0; i < listA.Count; i++) {
        string mergeKey = mergeKeyBuilder.Invoke(listA[i]);
        T item = listTemp.Find((y) => match.Invoke(listA[i], y));
        if (item == null || item.Equals(default(T))) {     // A not found in B
          var merge = new Merge<T>(mergeKey, listA[i], this.nullObject, MergeResultType.UnmatchedObjectB);
          mergeResult.Add(merge);
        } else if (condition.Invoke(listA[i], item)) {     // A found in B and condition = true
          var merge = new Merge<T>(mergeKey, listA[i], item, MergeResultType.ExactMatch);
          mergeResult.Add(merge);
          listTemp.Remove(item);
        } else {                                          // A found in B but condition = false
          var merge = new Merge<T>(mergeKey, listA[i], item, MergeResultType.ConditionFails);
          mergeResult.Add(merge);
          listTemp.Remove(item);
        }
      }
      for (int j = 0; j < listTemp.Count; j++) {
        string mergeKey = mergeKeyBuilder.Invoke(listTemp[j]);
        Merge<T> merge = new Merge<T>(mergeKey, this.nullObject, listTemp[j], MergeResultType.UnmatchedObjectA);
        mergeResult.Add(merge);
      }
    }

    #endregion Public methods

  } // class FixedListComparer

} // namespace Empiria
