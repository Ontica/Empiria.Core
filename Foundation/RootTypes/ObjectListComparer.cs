/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : ObjectList                                       Pattern  : Empiria List Class                *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a list of BaseObject instances.                                                    *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

namespace Empiria {

  public delegate bool Predicate2<in T>(T obj1, T obj2);
  public delegate string MergeKeyBuilder<in T>(T obj);

  public enum MergeResultType {
    Undefined = 'U',
    ExactMatch = 'M',
    ConditionFails = 'C',
    UnmatchedObjectA = 'A',
    UnmatchedObjectB = 'B',
  }

  public class Merge<T> where T : IStorable {

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

  /// <summary>Represents a list of BaseObject instances.</summary>
  public class ObjectListComparer<T> where T : IStorable {

    #region Fields

    private ObjectList<T> listA = null;
    private ObjectList<T> listB = null;
    private T nullObject = default(T);

    private List<Merge<T>> mergeResult = new List<Merge<T>>();

    #endregion Fields

    #region Constructors and parsers

    private ObjectListComparer() {
      //no-op
    }

    static public ObjectListComparer<T> Parse(ObjectList<T> listA, ObjectList<T> listB, T nullObject) {
      Assertion.EnsureObject(listA, "listA");
      Assertion.EnsureObject(listB, "listB");

      ObjectListComparer<T> comparer = new ObjectListComparer<T>();

      comparer.listA = listA;
      comparer.listB = listB;
      comparer.nullObject = nullObject;

      return comparer;
    }

    #endregion Constructors and parsers

    #region Public properties

    public ObjectList<T> ListA {
      get { return listA; }
    }

    public ObjectList<T> ListB {
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
        if (item == null || item.Equals(default(T))) {     // A Not founded in B
          Merge<T> merge = new Merge<T>(mergeKey, listA[i], this.nullObject, MergeResultType.UnmatchedObjectB);
          mergeResult.Add(merge);
        } else if (condition.Invoke(listA[i], item)) {     // A Founded in B and condition = true
          Merge<T> merge = new Merge<T>(mergeKey, listA[i], item, MergeResultType.ExactMatch);
          mergeResult.Add(merge);
          listTemp.Remove(item);
        } else {                                          // A Founded in B but condition = false
          Merge<T> merge = new Merge<T>(mergeKey, listA[i], item, MergeResultType.ConditionFails);
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

  } // class ObjectListComparer

} // namespace Empiria