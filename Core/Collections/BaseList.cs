/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaList                                      Pattern  : List                              *
*                                                                                                            *
*  Summary   : Represents a generic named sortable list of items of specific reference type.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Empiria.Collections {

  /// <summary>Represents a generic named sortable list of items of specific reference type.</summary>
  public class BaseList<T> : IList<T>, ICollection {

    #region Fields

    private readonly List<T> items = null;
    private readonly bool isSynchronized = false;
    private readonly object _lock = new object();

    #endregion Fields

    #region Constructors and parsers

    public BaseList() : this(false) {
      //no-op
    }


    public BaseList(bool synchronized) : this(16, synchronized) {
      // no-op
    }


    public BaseList(int capacity): this(capacity, false) {
      // no-op
    }


    public BaseList(List<T> list) : this(list, false) {
      // no-op
    }


    public BaseList(int capacity, bool synchronized) {
      this.isSynchronized = synchronized;
      items = new List<T>(capacity);
    }


    public BaseList(List<T> items, bool synchronized) {
      this.isSynchronized = synchronized;
      this.items = items;
    }


    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Gets the item with a specific index.</summary>
    public virtual T this[int index] {
      get {
        if ((0 <= index) && (index < items.Count)) {
          return items[index];
        } else {
          throw new ListException(ListException.Msg.ListIndexOutOfRange, index);
        }
      }
      protected set {
        if ((0 <= index) && (index < items.Count)) {
          SetItemAt(index, value);
        } else {
          throw new ListException(ListException.Msg.ListIndexOutOfRange, index);
        }
      }
    }


    /// <summary>Gets the number of elements contained in the collection.</summary>
    public int Count {
      get { return items.Count; }
    }


    public bool IsSynchronized {
      get { return isSynchronized; }
    }

    #endregion Public properties

    #region Public methods


    /// <summary>Gets the number of elements contained in the collection that
    /// matchs a spececific condition.</summary>
    public int CountAll(Predicate<T> match) {
      List<T> list = items.FindAll(match);

      if (list == null) {
        return 0;
      } else {
        return list.Count;
      }
    }


    /// <summary>Copies the elements of this instance of Collection to an instance of the Array
    ///  class, starting at a particular index.</summary>
    /// <param name="array">The target Array to which the Collection will be copied.</param>
    /// <param name="index">The zero-based index in the target Array where copying begins.</param>
    public virtual void CopyTo(T[] array, int index) {
      for (int i = index, j = items.Count; i < j; i++) {
        array.SetValue(items[i], i);
      }
    }


    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    public IEnumerator<T> GetEnumerator() {
      return items.GetEnumerator();
    }


    public int IndexOf(T item) {
      return items.IndexOf(item);
    }


    #endregion Public methods


    #region Protected methods

    protected void Add(T item) {
      if (isSynchronized) {
        lock (_lock) {
          items.Add(item);
        }
      } else {
        items.Add(item);
      }
    }


    protected void AddRange(IEnumerable<T> collection) {
      if (isSynchronized) {
        lock (_lock) {
          items.AddRange(collection);
        }
      } else {
        items.AddRange(collection);
      }
    }


    protected ReadOnlyCollection<T> AsReadOnly() {
      return items.AsReadOnly();
    }


    protected void Clear() {
      if (isSynchronized) {
        lock (_lock) {
          items.Clear();
        }
      } else {
        items.Clear();
      }
    }


    protected bool Contains(T item) {
      return items.Contains(item);
    }


    public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) {
      if (isSynchronized) {
        lock (_lock) {
          return items.ConvertAll(converter);
        }
      } else {
        return items.ConvertAll(converter);
      }
    }


    protected T Find(Predicate<T> match) {
      if (isSynchronized) {
        lock (_lock) {
          return items.Find(match);
        }
      } else {
        return items.Find(match);
      }
    }


    protected bool Exists(Predicate<T> match) {
      return items.Exists(match);
    }


    protected List<T> FindAll(Predicate<T> match) {
      if (isSynchronized) {
        lock (_lock) {
          return items.FindAll(match);
        }
      } else {
        return items.FindAll(match);
      }
    }


    protected T FindLast(Predicate<T> match) {
      if (isSynchronized) {
        lock (_lock) {
          return items.FindLast(match);
        }
      } else {
        return items.FindLast(match);
      }
    }


    protected void Insert(int index, T item) {
      if (isSynchronized) {
        lock (_lock) {
          items.Insert(index, item);
        }
      } else {
        items.Insert(index, item);
      }
    }


    protected void Load(DataTable table, string valueField) {
      if (isSynchronized) {
        lock (_lock) {
          Clear();
          for (int i = 0; i < table.Rows.Count; i++) {
            Add((T) table.Rows[i][valueField]);
          }
        }
      } else {
        Clear();
        for (int i = 0; i < table.Rows.Count; i++) {
          Add((T) table.Rows[i][valueField]);
        }
      }
    }


    protected void Load(DataView view, string valueField) {
      if (isSynchronized) {
        lock (_lock) {
          Clear();
          for (int i = 0; i < view.Count; i++) {
            Add((T) view[i][valueField]);
          }
        }
      } else {
        Clear();
        for (int i = 0; i < view.Count; i++) {
          Add((T) view[i][valueField]);
        }
      }
    }


    protected bool Remove(T item) {
      if (isSynchronized) {
        lock (_lock) {
          return items.Remove(item);
        }
      } else {
        return items.Remove(item);
      }
    }


    protected int RemoveAll(Predicate<T> match) {
      if (isSynchronized) {
        lock (_lock) {
          return items.RemoveAll(match);
        }
      } else {
        return items.RemoveAll(match);
      }
    }


    protected T RemoveAt(int index) {
      if (isSynchronized) {
        lock (_lock) {
          T item = items[index];
          items.RemoveAt(index);
          return item;
        }
      } else {
        T item = items[index];
        items.RemoveAt(index);
        return item;
      }
    }


    protected void RemoveLast(int count) {
      if (isSynchronized) {
        lock (_lock) {
          items.RemoveRange(items.Count - count, count);
        }
      } else {
        items.RemoveRange(items.Count - count, count);
      }
    }


    protected void RemoveRange(int index, int count) {
      if (isSynchronized) {
        lock (_lock) {
          items.RemoveRange(index, count);
        }
      } else {
        items.RemoveRange(index, count);
      }
    }


    protected void Reverse() {
      if (isSynchronized) {
        lock (_lock) {
          items.Reverse();
        }
      } else {
        items.Reverse();
      }
    }


    protected IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector) {
      if (isSynchronized) {
        lock (_lock) {
          return items.Select<T, TResult>(selector);
        }
      } else {
        return items.Select<T, TResult>(selector);
      }
    }


    /// <summary>Sets the item with a specific index from a Collection.</summary>
    /// <param name="index">The zero-based index of the parameter to set its value.</param>
    /// <param name="item">The new item to set at the given index.</param>
    protected void SetItemAt(int index, T item) {
      if (isSynchronized) {
        lock (_lock) {
          if ((0 <= index) && (index <= items.Count)) {
            items[index] = item;
          } else {
            throw new ListException(ListException.Msg.ListIndexOutOfRange, index);
          }
        } // lock
      } else {
        if ((0 <= index) && (index <= items.Count)) {
          items[index] = item;
        } else {
          throw new ListException(ListException.Msg.ListIndexOutOfRange, index);
        }
      } // if
    }


    protected void Sort(Comparison<T> comparison) {
      if (isSynchronized) {
        lock (_lock) {
          items.Sort(comparison);
        }
      } else {
        items.Sort(comparison);
      }
    }


    #endregion Protected methods


    #region IEnumerator, IList and ICollection properties and methods

    T IList<T>.this[int index] {
      get {
        return this[index];
      }
      set {
        this.SetItemAt(index, value);
      }
    }


    void ICollection<T>.Clear() {
      this.Clear();
    }


    public void CopyTo(Array array, int index) {
      Array.Copy(this.items.ToArray(), index, array, 0, this.items.Count - index);
    }


    public object SyncRoot {
      get {
        return ((ICollection) items).SyncRoot;
      }
    }


    void ICollection<T>.Add(T item) {
      items.Add(item);
    }


    bool ICollection<T>.Contains(T item) {
      return items.Contains(item);
    }


    int IList<T>.IndexOf(T item) {
      return items.IndexOf(item);
    }


    void IList<T>.Insert(int index, T item) {
      items.Insert(index, item);
    }


    bool ICollection<T>.IsReadOnly {
      get { return false; }
    }


    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
      return (items as IEnumerable<T>).GetEnumerator();
    }


    /// <summary>Gets an IEnumerator that can iterate through the collection.</summary>
    IEnumerator IEnumerable.GetEnumerator() {
      return (items as System.Collections.IEnumerable).GetEnumerator();
    }


    bool ICollection<T>.Remove(T item) {
      return items.Remove(item);
    }


    void IList<T>.RemoveAt(int index) {
      this.RemoveAt(index);
    }


    #endregion IEnumerator, IList and ICollection properties and methods


  } //class BaseList

} //namespace Empiria.Collections
