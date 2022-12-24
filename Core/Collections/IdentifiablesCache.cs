/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Collections                        Component : Empiria Core                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : IdentifiablesCache                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Sealed class that holds a double dictionary (by id and key) for IIdentifiable objects.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using System.Collections;
using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Sealed class that holds a double dictionary (by id and key) for IIdentifiable objects.</summary>
  public sealed class IdentifiablesCache<T> : IEnumerable<T> where T : class, IIdentifiable {

    #region Fields

    private readonly Dictionary<int, T> items = null;
    private readonly Dictionary<string, T> keys = null;

    private readonly object _locker = new object();

    #endregion Fields

    #region Constructors and parsers

    public IdentifiablesCache() {
      items = new Dictionary<int, T>();
      keys = new Dictionary<string, T>();
    }

    public IdentifiablesCache(int capacity) {
      items = new Dictionary<int, T>(capacity);
      keys = new Dictionary<string, T>(capacity);
    }

    #endregion Constructors and parsers

    #region Properties

    public T this[int id] {
      get {
        if (ContainsId(id)) {
          return items[id];
        } else {
          throw new ListException(ListException.Msg.ListKeyNotFound, id);
        }
      }
    }

    public T this[string key] {
      get {
        if (ContainsKey(key)) {
          return keys[key];
        } else {
          throw new ListException(ListException.Msg.ListKeyNotFound, key);
        }
      }
    }

    public int Count {
      get {
        return items.Count;
      }
    }

    #endregion Properties

    #region Methods

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
      return (items as IEnumerable).GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
      return items.Values.GetEnumerator();
    }

    public void Insert(string key, T item) {
      Assertion.Require(key, "key");
      Assertion.Require(item, "item");

      if (!this.ContainsId(item.Id)) {
        lock (_locker) {
          if (!this.ContainsId(item.Id)) {
            items.Add(item.Id, item);
          } else {
            items[item.Id] = item;
          }
        }  // lock
      } else {
        lock (_locker) {
          items[item.Id] = item;
        }
      }  // if (this.ContainsId)

      if (!this.ContainsKey(key)) {
        lock (_locker) {
          if (!this.ContainsKey(key)) {
            keys.Add(key, item);
          } else {
            keys[key] = item;
          }
        }  // lock
      } else {
        lock (_locker) {
          keys[key] = item;
        }
      }   // if (this.ContainsKey)

    }

    public bool Remove(string key) {
      if (keys.ContainsKey(key)) {
        lock (_locker) {
          if (keys.ContainsKey(key)) {
            T item = keys[key];
            keys.Remove(key);
            if (items.ContainsKey(item.Id)) {
              items.Remove(item.Id);
            }
            return true;
          }
        }
      }
      return false;
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

    #endregion Methods

  } // class IdentifiablesCache

} // namespace Empiria.Collections
