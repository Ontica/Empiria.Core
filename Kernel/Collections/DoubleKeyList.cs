/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Collections                              Assembly : Empiria.dll                       *
*  Type      : DoubleKeyList                                    Pattern  : Collection class                  *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Sealed class that holds a double dictionary list.                                             *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Sealed class that holds a double dictionary list.</summary>
  public sealed class DoubleKeyList<T> : IEnumerable<T> where T : class, IIdentifiable {

    #region Fields

    private Dictionary<int, T> items = null;
    private Dictionary<string, T> keys = null;

    #endregion Fields

    #region Constructors and parsers

    public DoubleKeyList() {
      items = new Dictionary<int, T>();
      keys = new Dictionary<string, T>();
    }

    public DoubleKeyList(int capacity) {
      items = new Dictionary<int, T>(capacity);
      keys = new Dictionary<string, T>(capacity);
    }

    #endregion Constructors and parsers

    #region Public properties

    public T this[int id] {
      get { return (T) items[id]; }
    }

    public T this[string key] {
      get { return keys[key]; }
      set { keys[key] = value; }
    }

    public int Count {
      get { return items.Count; }
    }

    #endregion Public properties

    #region Public methods

    public void Add(string key, T item) {
      items.Add(item.Id, item);
      keys.Add(key, item);
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
      if (!keys.ContainsKey(key)) {
        return false;
      }
      var item = keys[key];
      keys.Remove(key);
      items.Remove(item.Id);
      return true;
    }

    public T TryGetValue(int id) {
      T value;
      if (items.TryGetValue(id, out value)) {
        return value;
      } else {
        return null;
      }
    }

    public T TryGetValue(string key) {
      T value;
      if (keys.TryGetValue(key, out value)) {
        return value;
      } else {
        return null;
      }
    }

    #endregion Public methods

  } // class DoubleKeyList

} // namespace Empiria.Collections
