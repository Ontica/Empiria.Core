/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Collection                         Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : EmpiriaHashTable                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Thread safe hash table of items of the form string-value.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Thread safe hash table of items of the form string-value.</summary>
  public class EmpiriaHashTable<ItemsType> {

    #region Fields

    private readonly Dictionary<string, ItemsType> items = null;
    private readonly object locker = new object();

    #endregion Fields

    #region Constructors and parsers

    public EmpiriaHashTable(int capacity = 16) {
      items = new Dictionary<string, ItemsType>(capacity);
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Gets the item with a specific key.</summary>
    public ItemsType this[string key] {
      get {
        if (ContainsKey(key)) {
          return items[key];
        } else {
          throw new ListException(ListException.Msg.ListKeyNotFound, key);
        }
      }
    }


    /// <summary>Gets the number of elements contained in the cache.</summary>
    public int Count {
      get { return items.Count; }
    }


    public ICollection<string> Keys {
      get { return items.Keys; }
    }


    public ICollection<ItemsType> Values {
      get { return items.Values; }
    }

    #endregion Public properties

    #region Public methods

    public void Clear() {
      lock (locker) {
        items.Clear();
      }
    }


    public bool ContainsKey(string key) {
      return items.ContainsKey(key);
    }


    public bool ContainsValue(ItemsType value) {
      return items.ContainsValue(value);
    }


    public void CopyTo(ItemsType[] array, int index) {
      IEnumerator<ItemsType> enumerator = items.Values.GetEnumerator();
      int i = 0;
      while (enumerator.MoveNext()) {
        if (i >= index) {
          array.SetValue(enumerator.Current, i);
        }
      }
    }


    public void Insert(string key, ItemsType item) {
      if (!items.ContainsKey(key)) {
        lock (locker) {
          if (!items.ContainsKey(key)) {
            items.Add(key, item);
          } else {
            items[key] = item;
          }
        }
      } else {
        lock (locker) {
          items[key] = item;
        }
      }
    }


    public bool Remove(string key) {
      if (this.ContainsKey(key)) {
        lock (locker) {
          if (this.ContainsKey(key)) {
            return items.Remove(key);
          }
        } // lock
      }
      return false;
    }


    public FixedList<ItemsType> ToFixedList() {
      return new FixedList<ItemsType>(items.Values);
    }

    #endregion Public methods

  } //class EmpiriaHashTable

} //namespace Empiria.Collections
