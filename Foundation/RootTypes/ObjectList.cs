/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : ObjectList                                       Pattern  : Empiria List Class                *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a list of BaseObject instances.                                                    *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Collections;

namespace Empiria {

  /// <summary>Represents a list of BaseObject instances.</summary>
  public class ObjectList<T> : EmpiriaList<T> where T : IStorable {

    #region Constructors and parsers

    public ObjectList() {
      //no-op
    }

    public ObjectList(int capacity)
      : base(capacity) {
      // no-op
    }

    public ObjectList(string name, int capacity)
      : base(name, capacity, false) {
      //no-op
    }


    public ObjectList(List<T> list)
      : this(list.Count) {
      this.AddRange(list);
    }

    public ObjectList(System.Func<DataRow, T> parseMethod, DataView view)
      : this(view.Count) {
      for (int i = 0; i < view.Count; i++) {
        this.Add(parseMethod.Invoke(view[i].Row));
      }
    }

    public ObjectList(System.Func<DataRow, T> parseMethod, DataTable table)
      : this(table.Rows.Count) {
      for (int i = 0; i < table.Rows.Count; i++) {
        this.Add(parseMethod.Invoke(table.Rows[i]));
      }
    }

    #endregion Constructors and parsers

    #region Public methods

    protected internal new void Add(T item) {
      base.Add(item);
    }

    public override T this[int index] {
      get {
        return (T) base[index];
      }
    }

    public new bool Contains(T item) {
      return base.Contains(item);
    }

    public bool Contains(Predicate<T> match) {
      T result = base.Find(match);

      return (result != null);
    }

    public override void CopyTo(T[] array, int index) {
      for (int i = index, j = Count; i < j; i++) {
        array.SetValue(base[i], i);
      }
    }

    public new T Find(Predicate<T> match) {
      return base.Find(match);
    }

    public new List<T> FindAll(Predicate<T> match) {
      return base.FindAll(match);
    }

    protected internal int Remove(ObjectList<T> objectList) {
      IEnumerator<T> enumerator = this.GetEnumerator();

      int counter = 0;
      for (int i = 0; i < objectList.Count; i++) {
        if (base.Remove(objectList[i])) {
          counter++;
        }
      }
      return counter;
    }

    protected internal new int RemoveAll(Predicate<T> match) {
      return base.RemoveAll(match);
    }

    public new void Sort(Comparison<T> comparison) {
      base.Sort(comparison);
    }

    #endregion Public methods

  } // class ObjectList

} // namespace Empiria