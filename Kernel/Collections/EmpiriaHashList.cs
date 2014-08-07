/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaHashList                                  Pattern  : HashList Class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a named list of pairs object-string, where object is a reference type.             *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Collections {

  /// <summary>Represents a named list of pairs object-string, where object is a reference type.</summary>
  public class EmpiriaHashList<T> : ICollection<T> where T : class {

    #region Fields

    private string name = String.Empty;
    private Dictionary<string, T> items = null;
    private bool isSynchronized = false;

    #endregion Fields

    #region Constructors and parsers

    private EmpiriaHashList() {
      // Default constructor not allowed for derived classes
    }

    protected EmpiriaHashList(bool synchronized)
      : this(String.Empty, 32, synchronized) {
      // no-op
    }

    protected EmpiriaHashList(int capacity, bool synchronized)
      : this(String.Empty, capacity, synchronized) {
      // no-op
    }

    protected EmpiriaHashList(string name, bool synchronized)
      : this(name, 32, synchronized) {
      // no-op
    }

    protected EmpiriaHashList(string name, int capacity, bool synchronized) {
      this.name = name;
      this.isSynchronized = synchronized;
      items = new Dictionary<string, T>(capacity);
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Gets the number of elements contained in the collection.</summary>
    public int Count {
      get { return items.Count; }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    public bool IsSynchronized {
      get { return isSynchronized; }
    }

    public string Name {
      get { return name; }
    }

    #endregion Public properties

    #region Protected properties

    /// <summary>Gets the item with a specific index.</summary>
    protected T this[string key] {
      get {
        if (ContainsKey(key)) {
          return items[key.ToLowerInvariant()];
        } else {
          throw new EmpiriaListException(EmpiriaListException.Msg.ListKeyNotFound, key);
        }
      }
      set {
        items[key.ToLowerInvariant()] = value;
      }
    }

    protected ICollection<string> Keys {
      get { return items.Keys; }
    }

    protected ICollection<T> Values {
      get { return items.Values; }
    }

    #endregion Protected properties

    #region Public methods

    /// <summary>Copies the elements of this instance of Collection to an instance of the Array
    ///  class, starting at a particular index.</summary>
    /// <param name="array">The target Array to which the Collection will be copied.</param>
    /// <param name="index">The zero-based index in the target Array where copying begins.</param>
    void ICollection<T>.CopyTo(T[] array, int index) {
      IEnumerator<T> enumerator = items.Values.GetEnumerator();
      int i = 0;
      while (enumerator.MoveNext()) {
        if (i >= index) {
          array.SetValue(enumerator.Current, i);
        }
      }
    }

    /// <summary>Gets an IEnumerator that can iterate through the ParametersCollection.</summary>
    public IEnumerator<T> GetEnumerator() {
      return items.Values.GetEnumerator();
    }

    /// <summary>Gets an IEnumerator that can iterate through the Collection.</summary>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return items.Values.GetEnumerator();
    }

    #endregion Public methods

    #region Protected methods

    protected void Add(string key, T item) {
      if (isSynchronized) {
        lock (items) {
          items.Add(key.ToLowerInvariant(), item);
        }
      } else {
        items.Add(key.ToLowerInvariant(), item);
      }
    }

    void ICollection<T>.Add(T item) {
      Add(System.Guid.NewGuid().ToString(), item);
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

    void ICollection<T>.Clear() {
      Clear();
    }

    protected bool ContainsKey(string key) {
      return items.ContainsKey(key.ToLowerInvariant());
    }

    protected bool ContainsValue(T value) {
      return items.ContainsValue(value);
    }

    bool ICollection<T>.Contains(T value) {
      return items.ContainsValue(value);
    }

    protected void Load(DataTable table, string keyField, string valueField) {
      if (isSynchronized) {
        lock (items) {
          Clear();
          for (int i = 0; i < table.Rows.Count; i++) {
            Add((string) table.Rows[i][keyField], (T) table.Rows[i][valueField]);
          }
        }
      } else {
        Clear();
        for (int i = 0; i < table.Rows.Count; i++) {
          Add((string) table.Rows[i][keyField], (T) table.Rows[i][valueField]);
        }
      }
    }

    protected void Load(DataView view, string keyField, string valueField) {
      if (isSynchronized) {
        lock (items) {
          Clear();
          for (int i = 0; i < view.Count; i++) {
            Add((string) view[i][keyField], (T) view[i][valueField]);
          }
        }
      } else {
        Clear();
        for (int i = 0; i < view.Count; i++) {
          Add((string) view[i][keyField], (T) view[i][valueField]);
        }
      }
    }

    protected T Remove(string key) {
      if (isSynchronized) {
        lock (items) {
          key = key.ToLowerInvariant();
          if (ContainsKey(key)) {
            T item = items[key];
            items.Remove(key);
            return item;
          } else {
            throw new EmpiriaListException(EmpiriaListException.Msg.ListKeyNotFound, key);
          }
        }
      } else {
        key = key.ToLowerInvariant();
        if (ContainsKey(key)) {
          T item = items[key];
          items.Remove(key);
          return item;
        } else {
          throw new EmpiriaListException(EmpiriaListException.Msg.ListKeyNotFound, key);
        }
      }
    }

    bool ICollection<T>.Remove(T item) {
      throw new NotImplementedException();
    }

    #endregion Protected methods

  } //class EmpiriaHashList

} //namespace Empiria.Collections
