/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Collections                              Assembly : Empiria.dll                       *
*  Type      : DoubleKeyList                                    Pattern  : Collection class                  *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Sealed class that holds a double sorted list.                                                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Sealed class that holds a double sorted list.</summary>
  public sealed class DoubleKeyList<T> : IEnumerable<T> where T : IIdentifiable {

    #region Fields

    private SortedList<int, T> items = null;
    private SortedList<string, int> keys = null;

    private string name = String.Empty;

    #endregion Fields

    #region Constructors and parsers

    public DoubleKeyList() {
      items = new SortedList<int, T>();
      keys = new SortedList<string, int>();
    }

    public DoubleKeyList(int capacity) {
      items = new SortedList<int, T>(capacity);
      keys = new SortedList<string, int>(capacity);
    }

    public DoubleKeyList(string name) {
      this.name = name;
    }

    public DoubleKeyList(int capacity, string name) {
      items = new SortedList<int, T>(capacity);
      keys = new SortedList<string, int>(capacity);
      this.name = name;
    }

    #endregion Constructors and parsers

    #region Public properties

    public T this[int id] {
      get { return (T) items[id]; }
    }

    public T this[string key] {
      get { return items[keys[key]]; }
      set { items[keys[key]] = value; }
    }

    public int Count {
      get { return items.Count; }
    }

    public IList<int> Ids {
      get { return items.Keys; }
    }

    public IList<string> Keys {
      get { return keys.Keys; }
    }

    public string Name {
      get { return this.name; }
    }

    public IList<T> Values {
      get { return items.Values; }
    }

    #endregion Public properties

    #region Public methods

    public void Add(string key, T item) {
      items.Add(item.Id, item);
      keys.Add(key, item.Id);
    }

    public bool ContainsId(int id) {
      return items.ContainsKey(id);
    }

    public bool ContainsKey(string key) {
      return keys.ContainsKey(key);
    }

    public void CopyTo(T[] array, int arrayIndex) {
      items.Values.CopyTo(array, arrayIndex);
    }

    public void Clear() {
      items.Clear();
      keys.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return (items as System.Collections.IEnumerable).GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
      return items.Values.GetEnumerator();
    }

    public bool Remove(string key) {
      items.Remove(keys[key]);
      return keys.Remove(key);
    }

    #endregion Public methods

  } // class DoubleKeyList

} // namespace Empiria.Collections
