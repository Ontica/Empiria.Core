﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : CachedList                                       Pattern  : Cache Collection Class            *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Abstract generic class that represents a dynamic cached collection of IIdentifiable objects.  *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Abstract generic that represents a dynamic cached collection of IIdentifiable objects.</summary>
  public abstract class CachedList<T> : ICollection<T> where T : class, IIdentifiable  {

    #region Fields

    private Dictionary<string, T> objects = null;
    private Dictionary<string, T> namedObjects = null;
    private Dictionary<string, long> lastAccess = null;
    private int size = 0;

    #endregion Fields

    #region Constructors and parsers

    private CachedList() {
      // Default constructor not allowed for derived classes
    }

    protected CachedList(int size) {
      this.size = size;
      this.objects = new Dictionary<string, T>(size);
      this.namedObjects = new Dictionary<string, T>(size);
      this.lastAccess = new Dictionary<string, long>(size);
    }

    protected CachedList(string name, int size) {

    }

    #endregion Constructors and parsers

    #region Public properties

    public int Count {
      get { return objects.Count; }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    #endregion Public properties

    #region Public methods

    void ICollection<T>.CopyTo(T[] array, int index) {
      IEnumerator<T> enumerator = objects.Values.GetEnumerator();
      int i = 0;
      while (enumerator.MoveNext()) {
        if (i >= index) {
          array.SetValue(enumerator.Current, i);
        }
      }
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
      return objects.Values.GetEnumerator();
    }

    /// <summary>Gets an IEnumerator that can iterate through the Collection.</summary>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return objects.Values.GetEnumerator();
    }

    #endregion Public methods

    #region Protected methods

    protected void Insert(string itemTypeName, T item) {
      string objectKey = item.Id.ToString() + "." + itemTypeName;
      lock (objects) {
        objects[objectKey] = item;
        lastAccess[objectKey] = DateTime.Now.Ticks;
        this.TrimToSize();
      } // lock
    }

    protected void Insert(string itemTypeName, string namedKey, T item) {
      string key = namedKey + "." + itemTypeName;
      lock (objects) {
        namedObjects[key] = item;
        this.Insert(itemTypeName, item);
      } // lock
    }

    void ICollection<T>.Add(T item) {
      throw new NotImplementedException();
    }

    protected void Clear() {
      lock (objects) {
        objects.Clear();
        namedObjects.Clear();
        lastAccess.Clear();
      }
    }

    void ICollection<T>.Clear() {
      this.Clear();
    }

    public bool Contains(string itemTypeName, int id) {
      return objects.ContainsKey(id.ToString() + "." + itemTypeName);
    }

    public bool Contains(string itemTypeName, string namedKey) {
      return namedObjects.ContainsKey(namedKey + "." + itemTypeName);
    }

    public bool Contains(T item) {
      return objects.ContainsValue(item);
    }

    private bool ContainsKey(string key) {
      return objects.ContainsKey(key);
    }

    bool ICollection<T>.Remove(T item) {
      throw new NotImplementedException();
    }

    protected T TryGetItem(string itemTypeName, int id) {
      string objectKey = id.ToString() + "." + itemTypeName;

      T value;
      if (objects.TryGetValue(objectKey, out value)) {
        lastAccess[objectKey] = DateTime.Now.Ticks;
        return value;
      } else {
        return null;
      }
    }

    protected T TryGetItem(string itemTypeName, string namedKey) {
      string objectKey = namedKey + "." + itemTypeName;
      T value;
      if (namedObjects.TryGetValue(objectKey, out value)) {
        lastAccess[objectKey] = DateTime.Now.Ticks;
        return value;
      } else {
        return null;
      }
    }

    #endregion Protected methods

    #region Private methods

    private void TrimToSize() {
      if (lastAccess.Count <= size) {
        return;
      }
      lock (objects) {
        List<long> sortedList = new List<long>(lastAccess.Values);

        int toDeleteItems = lastAccess.Count / 4;   // Remove 25 percent of old accessed objects 
        long trimValueUpperBound = sortedList[toDeleteItems];

        string[] keys = new string[lastAccess.Keys.Count];

        lastAccess.Keys.CopyTo(keys, 0);
        // Remove objects in list and lastAccess list items
        for (int i = 0; i < keys.Length; i++) {
          string key = keys[i];
          if (lastAccess[key] <= trimValueUpperBound) {
            lastAccess.Remove(key);
            objects.Remove(key);
          }
        }

        // Remove deleted named objects references
        keys = new string[namedObjects.Keys.Count];
        namedObjects.Keys.CopyTo(keys, 0);

        for (int i = 0; i < keys.Length; i++) {
          string namedKey = keys[i];
          if (!objects.ContainsKey(namedKey)) {
            namedObjects.Remove(namedKey);
          }
        }
        Empiria.Messaging.Publisher.Publish("Se eliminaron " + toDeleteItems.ToString() +
                                            " objetos de la caché del sistema Empiria.");
      } // lock
    }

    #endregion Private methods

  } //class EmpiriaCache

} //namespace Empiria.Collection
