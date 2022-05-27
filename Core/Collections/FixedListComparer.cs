/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              License  : Please read LICENSE.txt file      *
*  Type      : FixedListComparer                                Pattern  : Standard class                    *
*                                                                                                            *
*  Summary   : Performs an item  by item comparison of two FixedList objects.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.Collections {

  public delegate string MergeKeyBuilder<in T>(T obj);

  public delegate bool Predicate2<in T>(T obj1, T obj2);

  /// <summary>Performs an item  by item comparison of two FixedList objects.</summary>
  public class FixedListComparer<T> where T : IIdentifiable {

    #region Fields

    private List<MergeResult<T>> mergeResultList = new List<MergeResult<T>>();

    #endregion Fields

    #region Constructors and parsers

    private FixedListComparer() {
      //no-op
    }

    static public FixedListComparer<T> Parse(FixedList<T> leftList,
                                             FixedList<T> rightList, T nullObject) {
      Assertion.Require(leftList, "leftList");
      Assertion.Require(rightList, "rightList");

      FixedListComparer<T> comparer = new FixedListComparer<T>();

      comparer.LeftList = leftList;
      comparer.RightList = rightList;
      comparer.NullObject = nullObject;

      return comparer;
    }

    #endregion Constructors and parsers

    #region Public properties

    public FixedList<T> LeftList {
      get;
      private set;
    }

    public FixedList<T> RightList {
      get;
      private set;
    }

    public T NullObject {
      get;
      private set;
    }

    public List<MergeResult<T>> GetByMatchResult(MergeResultType result) {
      return mergeResultList.FindAll((x) => x.Result == result);
    }

    public List<MergeResult<T>> GetMatched() {
      return mergeResultList.FindAll((x) => (x.LeftObject != null && x.RightObject != null) &&
                             (!x.LeftObject.Equals(NullObject) && !x.RightObject.Equals(NullObject)));
    }

    public List<MergeResult<T>> GetMerge() {
      return mergeResultList;
    }

    public List<T> GetUnmatchedInListA() {
      List<MergeResult<T>> unmatched = mergeResultList.FindAll((x) => x.RightObject == null ||
                                                                      x.RightObject.Equals(NullObject));
      List<T> temp = new List<T>(unmatched.Count);
      for (int i = 0; i < unmatched.Count; i++) {
        temp.Add(unmatched[i].LeftObject);
      }
      return temp;
    }

    public List<T> GetUnmatchedInListB() {
      List<MergeResult<T>> unmatched = mergeResultList.FindAll((x) => x.LeftObject == null ||
                                                                      x.LeftObject.Equals(NullObject));
      List<T> temp = new List<T>(unmatched.Count);
      for (int i = 0; i < unmatched.Count; i++) {
        temp.Add(unmatched[i].RightObject);
      }
      return temp;
    }

    #endregion Public properties

    #region Public methods

    public void Merge(MergeKeyBuilder<T> mergeKeyBuilder, Predicate2<T> match, Predicate2<T> condition) {
      mergeResultList = new List<MergeResult<T>>((LeftList.Count + this.RightList.Count) / 2);

      List<T> listTemp = new List<T>(this.RightList.FindAll((x) => true));
      for (int i = 0; i < LeftList.Count; i++) {
        string mergeKey = mergeKeyBuilder.Invoke(LeftList[i]);
        T item = listTemp.Find((y) => match.Invoke(LeftList[i], y));
        if (item == null || item.Equals(default(T))) {     // A not found in B
          var mergeResult = new MergeResult<T>(mergeKey,
                                               LeftList[i], this.NullObject,
                                               MergeResultType.RightObjectUnmatched);
          mergeResultList.Add(mergeResult);

        } else if (condition.Invoke(LeftList[i], item)) {     // A found in B and condition = true
          var mergeResult = new MergeResult<T>(mergeKey,
                                               LeftList[i], item, MergeResultType.ExactMatch);
          mergeResultList.Add(mergeResult);
          listTemp.Remove(item);

        } else {                                          // A found in B but condition = false
          var mergeResult = new MergeResult<T>(mergeKey,
                                               LeftList[i], item, MergeResultType.ConditionFails);
          mergeResultList.Add(mergeResult);
          listTemp.Remove(item);
        }
      }

      for (int j = 0; j < listTemp.Count; j++) {
        string mergeKey = mergeKeyBuilder.Invoke(listTemp[j]);
        var mergeResult = new MergeResult<T>(mergeKey,
                                             this.NullObject, listTemp[j],
                                             MergeResultType.LeftObjectUnmatched);
        mergeResultList.Add(mergeResult);
      }

    }

    #endregion Public methods

  } // class FixedListComparer

} // namespace Empiria.Collections
