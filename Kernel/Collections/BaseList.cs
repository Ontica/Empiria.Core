/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaList                                      Pattern  : List                              *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a generic named sortable list of items of specific reference type.                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace Empiria.Collections {

  /// <summary>Represents a generic named sortable list of items of specific reference type.</summary>
  public class BaseList<T> : IList<T>, ICollection {

    #region Fields

    private string name = String.Empty;
    private List<T> items = null;
    private bool isSynchronized = false;

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

    #endregion Public methods

    #region Protected properties

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

    #endregion Protected properties

    #region Protected methods

    protected void Add(T item) {
      if (isSynchronized) {
        lock (items) {
          items.Add(item);
        }
      } else {
        items.Add(item);
      }
    }

    protected void AddRange(ICollection<T> collection) {
      if (isSynchronized) {
        lock (items) {
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
        lock (items) {
          items.Clear();
        }
      } else {
        items.Clear();
      }
    }

    protected bool Contains(T item) {
      if (isSynchronized) {
        lock (items) {
          return items.Contains(item);
        }
      } else {
        return items.Contains(item);
      }
    }

    protected T Find(Predicate<T> match) {
      if (isSynchronized) {
        lock (items) {
          return items.Find(match);
        }
      } else {
        return items.Find(match);
      }
    }

    protected List<T> FindAll(Predicate<T> match) {
      if (isSynchronized) {
        lock (items) {
          return items.FindAll(match);
        }
      } else {
        return items.FindAll(match);
      }
    }

    protected T FindLast(Predicate<T> match) {
      if (isSynchronized) {
        lock (items) {
          return items.FindLast(match);
        }
      } else {
        return items.FindLast(match);
      }
    }

    protected void Insert(int index, T item) {
      if (isSynchronized) {
        lock (items) {
          items.Insert(index, item);
        }
      } else {
        items.Insert(index, item);
      }
    }

    public int IndexOf(T item) {
      return items.IndexOf(item);
    }

    protected void Load(DataTable table, string valueField) {
      if (isSynchronized) {
        lock (items) {
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
        lock (items) {
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
        lock (items) {
          return items.Remove(item);
        }
      } else {
        return items.Remove(item);
      }
    }

    protected T RemoveAt(int index) {
      if (isSynchronized) {
        lock (items) {
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
        lock (items) {
          items.RemoveRange(items.Count - count, count);
        }
      } else {
        items.RemoveRange(items.Count - count, count);
      }
    }

    protected void RemoveRange(int index, int count) {
      if (isSynchronized) {
        lock (items) {
          items.RemoveRange(index, count);
        }
      } else {
        items.RemoveRange(index, count);
      }
    }

    protected void Reverse() {
      if (isSynchronized) {
        lock (items) {
          items.Reverse();
        }
      } else {
        items.Reverse();
      }
    }

    protected int RemoveAll(Predicate<T> match) {
      if (isSynchronized) {
        lock (items) {
          return items.RemoveAll(match);
        }
      } else {
        return items.RemoveAll(match);
      }
    }

    /// <summary>Sets the item with a specific index from a Collection.</summary>
    /// <param name="index">The zero-based index of the parameter to set its value.</param>
    /// <param name="parameterValue">The new item.</param>
    protected void SetItemAt(int index, T item) {
      if (isSynchronized) {
        lock (items) {
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
        lock (items) {
          items.Sort(comparison);
        }
      } else {
        items.Sort(comparison);
      }
    }

    #endregion Protected methods

    #region IEnumerator, IList and ICollection properties and methods

    T IList<T>.this[int index] {
      get { return this[index]; }
      set { this.SetItemAt(index, value); }
    }

    void ICollection<T>.Clear() {
      this.Clear();
    }

    public void CopyTo(Array array, int index) {
      throw new NotImplementedException();
    }

    public object SyncRoot {
      get {
        throw new NotImplementedException();
      }
    }

    void ICollection<T>.Add(T value) {
      items.Add(value);
    }

    bool ICollection<T>.Contains(T value) {
      return items.Contains(value);
    }

    int IList<T>.IndexOf(T value) {
      return items.IndexOf(value);
    }

    void IList<T>.Insert(int index, T value) {
      items.Insert(index, value);
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

    bool ICollection<T>.Remove(T value) {
      return items.Remove(value);
    }

    void IList<T>.RemoveAt(int index) {
      this.RemoveAt(index);
    }

    #endregion IEnumerator, IList and ICollection properties and methods

    //#endregion Inner class Enumerator

  } //class BaseList

} //namespace Empiria.Collections
