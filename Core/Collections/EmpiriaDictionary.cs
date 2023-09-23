/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaDictionary                                Pattern  : HashList Class                    *
*                                                                                                            *
*  Summary   : Represents a thread safe dictionary or hash table of items of the form key-value.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Represents a thread safe dictionary or hash table of items of the form key-value.</summary>
  public class EmpiriaDictionary<KeyType, ItemsType> {

    #region Fields

    private readonly Dictionary<KeyType, ItemsType> items = null;
    private readonly object locker = new object();

    #endregion Fields

    #region Constructors and parsers

    public EmpiriaDictionary(int capacity = 16) {
      items = new Dictionary<KeyType, ItemsType>(capacity);
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Gets the item with a specific key.</summary>
    public ItemsType this[KeyType key] {
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

    public ICollection<KeyType> Keys {
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

    public bool ContainsKey(KeyType key) {
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

    public void Insert(KeyType key, ItemsType item) {
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

    public bool Remove(KeyType key) {
      if (this.ContainsKey(key)) {
        lock (locker) {
          if (this.ContainsKey(key)) {
            return items.Remove(key);
          }
        } // lock
      }
      return false;
    }


    public void Remove(Predicate<ItemsType> itemsCondition) {

      var enumerator = items.GetEnumerator();

      List<KeyType> keysToRemove = new List<KeyType>();

      while (enumerator.MoveNext()) {
        if (itemsCondition.Invoke(enumerator.Current.Value)) {
          keysToRemove.Add(enumerator.Current.Key);
        }
      }

      foreach (var key in keysToRemove) {
        Remove(key);
      }
    }

    #endregion Public methods

  } //class EmpiriaDictionary

} //namespace Empiria.Collections
