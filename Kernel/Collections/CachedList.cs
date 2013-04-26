/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Framework Library      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : CachedList                                       Pattern  : Cache Collection Class            *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Abstract generic class that represents a dynamic cached collection of IIdentifiable objects.  *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Abstract generic that represents a dynamic cached collection of IIdentifiable objects.</summary>
  public abstract class CachedList<T> : ICollection<T> where T : IIdentifiable {

    #region Fields

    private string name = String.Empty;
    private Dictionary<string, T> objects = null;
    private Dictionary<string, string> namedObjects = null;
    private Dictionary<string, long> lastAccess = null;
    private int size = 0;

    #endregion Fields

    #region Constructors and parsers

    private CachedList() {
      // Default constructor not allowed for derived classes
    }

    protected CachedList(int size)
      : this(String.Empty, size) {
      // no-op
    }

    protected CachedList(string name, int size) {
      this.name = name;
      this.size = size;
      this.objects = new Dictionary<string, T>(size);
      this.namedObjects = new Dictionary<string, string>(size);
      this.lastAccess = new Dictionary<string, long>(size);
    }

    #endregion Constructors and parsers

    #region Public properties

    public int Count {
      get { return objects.Count; }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    public string Name {
      get { return name; }
    }

    public int Size {
      get { return size; }
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
      string objectKey = item.Id.ToString() + "." + itemTypeName.ToLowerInvariant();
      lock (objects) {
        objects[objectKey] = item;
        lastAccess[objectKey] = DateTime.Now.Ticks;
        this.TrimToSize();
      } // lock
    }

    protected void Insert(string itemTypeName, string namedKey, T item) {
      namedKey = namedKey.ToLowerInvariant() + "." + itemTypeName.ToLowerInvariant();
      lock (objects) {
        namedObjects[namedKey] = item.Id.ToString() + "." + itemTypeName.ToLowerInvariant();
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
      return objects.ContainsKey(id.ToString() + "." + itemTypeName.ToLowerInvariant());
    }

    public bool Contains(string itemTypeName, string namedKey) {
      return namedObjects.ContainsKey(namedKey.ToLowerInvariant() + "." + itemTypeName.ToLowerInvariant());
    }

    public bool Contains(T item) {
      return objects.ContainsValue(item);
    }

    protected T GetItem(string itemTypeName, int id) {
      string objectKey = id.ToString() + "." + itemTypeName.ToLowerInvariant();

      lock (objects) {
        if (this.Contains(itemTypeName, id)) {
          lastAccess[objectKey] = DateTime.Now.Ticks;
          return objects[objectKey];
        } else {
          throw new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionItemIdNotFound, id);
        } // if
      } // lock
    }

    protected T GetItem(string itemTypeName, string namedKey) {
      lock (objects) {
        if (this.Contains(itemTypeName, namedKey)) {
          string objectKey = namedObjects[namedKey.ToLowerInvariant() + "." + itemTypeName.ToLowerInvariant()];

          return objects[objectKey];
        } else {
          throw new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionKeyNotFound, namedKey);
        } // if
      } // lock
    }

    bool ICollection<T>.Remove(T item) {
      throw new NotImplementedException();
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
          if (!objects.ContainsKey(namedObjects[namedKey])) {
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