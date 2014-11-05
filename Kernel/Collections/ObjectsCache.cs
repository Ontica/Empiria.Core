/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : ObjectsCache                                     Pattern  : HashList Class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents an objects cache.                                                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Collections {

  /// <summary>Represents an objects cache.</summary>
  public class ObjectsCache<KeyType, ItemsType> where ItemsType : class {

    #region Fields

    private Dictionary<KeyType, ItemsType> items = null;
    private object locker = new object();

    #endregion Fields

    #region Constructors and parsers

    public ObjectsCache(int capacity = 16, int maxCapacity = -1) {
      items = new Dictionary<KeyType, ItemsType>(capacity);
      this.MaxCapacity = maxCapacity;
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Gets or sets the item with a specific key.</summary>
    public ItemsType this[KeyType key] {
      get {
        if (ContainsKey(key)) {
          return items[key];
        } else {
          throw new ListException(ListException.Msg.ListKeyNotFound, key);
        }
      }
      set {
        if (ContainsKey(key)) {
          lock (locker) {
            if (ContainsKey(key)) {
              items[key] = value;
              return;
            }
          } // lock
        }
        throw new ListException(ListException.Msg.ListKeyNotFound, key);
      }
    }

    /// <summary>Gets the number of elements contained in the cache.</summary>
    public int Count {
      get { return items.Count; }
    }

    public ICollection<KeyType> Keys {
      get { return items.Keys; }
    }

    /// <summary>Gets the max count of items for the cache.</summary>
    public int MaxCapacity {
      get;
      private set;
    }

    public ICollection<ItemsType> Values {
      get { return items.Values; }
    }

    #endregion Public properties

    #region Public methods

    public void Add(KeyType key, ItemsType item) {
      if (!items.ContainsKey(key)) {
        lock (locker) {
          if (!items.ContainsKey(key)) {
            items.Add(key, item);
            return;
          }
        }
      }
      throw new ListException(ListException.Msg.ListKeyAlreadyExists, key);
    }

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

    public ItemsType Remove(KeyType key) {
      if (this.ContainsKey(key)) {
        lock (locker) {
          if (this.ContainsKey(key)) {
            ItemsType item = items[key];
            items.Remove(key);
            return item;
          }
        } // lock
      }
      throw new ListException(ListException.Msg.ListKeyNotFound, key);
    }

    #endregion Public methods

  } //class ObjectsCache

} //namespace Empiria.Collections
