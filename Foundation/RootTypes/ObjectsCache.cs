/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : ObjectsCache                                     Pattern  : Cache Collection Class            *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Dynamic cached collection of BaseObject instances.                                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

namespace Empiria {

  /// <summary>Dynamic cached collection of BaseObject instances.</summary>
  internal sealed class ObjectsCache : ICollection<BaseObject> {

    #region Fields

    static private readonly int cacheSize =
                                ConfigurationData.GetInteger("Empiria.Ontology", "ObjectCache.Size");

    private Dictionary<string,BaseObject> objects = null;
    private Dictionary<string, BaseObject> namedObjects = null;
    private Dictionary<string, long> lastAccess = null;

    #endregion Fields

    #region Constructors and parsers

    internal ObjectsCache() {
      this.objects = new Dictionary<string, BaseObject>(cacheSize);
      this.namedObjects = new Dictionary<string, BaseObject>(cacheSize);
      this.lastAccess = new Dictionary<string, long>(cacheSize);
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

    void ICollection<BaseObject>.Add(BaseObject item) {
      throw new NotImplementedException();
    }

    internal void Clear() {
      lock (objects) {
        objects.Clear();
        namedObjects.Clear();
        lastAccess.Clear();
      }
    }

    void ICollection<BaseObject>.Clear() {
      this.Clear();
    }

    public bool Contains(string itemTypeName, int id) {
      return objects.ContainsKey(id.ToString() + "." + itemTypeName);
    }

    public bool Contains(string itemTypeName, string namedKey) {
      return namedObjects.ContainsKey(namedKey + "." + itemTypeName);
    }

    public bool Contains(BaseObject item) {
      return objects.ContainsValue(item);
    }

    private bool ContainsKey(string key) {
      return objects.ContainsKey(key);
    }

    void ICollection<BaseObject>.CopyTo(BaseObject[] array, int index) {
      IEnumerator<BaseObject> enumerator = objects.Values.GetEnumerator();
      int i = 0;
      while (enumerator.MoveNext()) {
        if (i >= index) {
          array.SetValue(enumerator.Current, i);
        }
      }
    }

    IEnumerator<BaseObject> IEnumerable<BaseObject>.GetEnumerator() {
      return objects.Values.GetEnumerator();
    }

    /// <summary>Gets an IEnumerator that can iterate through the Collection.</summary>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return objects.Values.GetEnumerator();
    }

    internal void Insert(BaseObject item) {
      string typeInfoName = item.GetEmpiriaType().Name;

      // Empty and Unknown instances are unique per type, so don't create their base objects inside
      // the inheritance hierarchy.
      // Example: ReadItem.Book.Fiction.Empty generates only one item in the cache
      // ('-1.ReadItem.Book.Fiction'), but not '-1.ReadItem.Book'. That's because
      // ReadItem.Book.Empty could be defined and also it will use the same id but for its own type:
      // '-1.ReadItem.Book' of type Book. SpecialCase instances are unique and static per type
      if (item.IsSpecialCase) {
        this.ExecuteInsert(typeInfoName, item);
        return;
      }

      // Stores item object into the objects cache and additionally insert it again for each
      // parent class defined on that item's inheritance hierarchy.
      // Example: An object with typeInfoName = 'ReadItem.Book.Fiction' and Id = 1001 generates
      // three items in the cache: '1001.ReadItem.Book.Fiction', '1001.ReadItem.Book' and '1001.ReadItem'
      while (true) {
        if (typeInfoName.LastIndexOf('.') > 0) {
          this.ExecuteInsert(typeInfoName, item);
          typeInfoName = typeInfoName.Substring(0, typeInfoName.LastIndexOf('.'));
        } else {
          break;
        }
      }
    }

    internal void Insert(BaseObject item, string namedKey) {
      string typeInfoName = item.GetEmpiriaType().Name;

      while (true) {
        if (typeInfoName.LastIndexOf('.') > 0) {
          this.ExecuteInsert(typeInfoName, namedKey, item);
          typeInfoName = typeInfoName.Substring(0, typeInfoName.LastIndexOf('.'));
        } else {
          break;
        }
      }
    }

    internal void Remove(BaseObject item) {
      string typeInfoName = item.GetEmpiriaType().Name;

      while (true) {
        if (typeInfoName.LastIndexOf('.') > 0) {
          this.ExecuteRemove(typeInfoName, item);
          typeInfoName = typeInfoName.Substring(0, typeInfoName.LastIndexOf('.'));
        } else {
          break;
        }
      }
    }

    bool ICollection<BaseObject>.Remove(BaseObject item) {
      throw new NotImplementedException();
    }

    internal T TryGetItem<T>(string itemTypeName, int id) where T : BaseObject {
      string objectKey = id.ToString() + "." + itemTypeName;

      BaseObject value;
      if (objects.TryGetValue(objectKey, out value)) {
        //lastAccess[objectKey] = DateTime.Now.Ticks;
        return (T) value;
      } else {
        return null;
      }
    }

    internal T TryGetItem<T>(string itemTypeName, string namedKey) where T: BaseObject {
      string objectKey = namedKey + "." + itemTypeName;
      BaseObject value;
      if (namedObjects.TryGetValue(objectKey, out value)) {
        //lastAccess[objectKey] = DateTime.Now.Ticks;
        return (T) value;
      } else {
        return null;
      }
    }

    #endregion Public methods

    #region Private methods

    private void ExecuteInsert(string itemTypeName, BaseObject item) {
      string objectKey = item.Id.ToString() + "." + itemTypeName;
      lock (objects) {
        objects[objectKey] = item;
        //lastAccess[objectKey] = DateTime.Now.Ticks;
        this.TrimToSize();
      } // lock
    }

    private void ExecuteInsert(string itemTypeName, string namedKey, BaseObject item) {
      string key = namedKey + "." + itemTypeName;
      lock (objects) {
        namedObjects[key] = item;
        this.ExecuteInsert(itemTypeName, item);
      } // lock
    }

    private void ExecuteRemove(string itemTypeName, BaseObject item) {
      string objectKey = item.Id.ToString() + "." + itemTypeName;
      lock (objects) {
        objects.Remove(objectKey);
        //lastAccess[objectKey] = DateTime.Now.Ticks;
      } // lock
    }

    private void TrimToSize() {
      if (lastAccess.Count <= cacheSize) {
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

  } //class ObjectsCache

} //namespace Empiria
