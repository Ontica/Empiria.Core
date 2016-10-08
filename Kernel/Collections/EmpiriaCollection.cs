/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaCollection                                Pattern  : Collection Class                  *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Abstract class that represents a strongly typed list of items that can be accessed            *
*              by index or key.                                                                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Collections {

  /// <summary>Abstract class that represents a strongly typed list of items that can be
  /// accessed by index or key.</summary>
  public abstract class EmpiriaCollection<TKey, TItem> : ICollection<TItem> {

    #region Fields

    private string name = String.Empty;
    private bool isReadOnly = false;
    private bool isSynchronized = false;

    private Dictionary<TKey, TItem> itemsDictionary = null;
    private List<TKey> keysList = null;

    #endregion Fields

    #region Constructors and parsers

    protected EmpiriaCollection()
      : this(String.Empty, 32) {
      // Default constructor not allowed for derived classes
    }

    protected EmpiriaCollection(int capacity)
      : this(String.Empty, capacity) {

    }

    protected EmpiriaCollection(string name)
      : this(name, 32) {

    }

    protected EmpiriaCollection(string name, int capacity) {
      this.name = name;
      this.itemsDictionary = new Dictionary<TKey, TItem>(capacity);
      this.keysList = new List<TKey>(capacity);
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Gets the item with a specific index.</summary>
    public TItem this[int index] {
      get {
        if ((0 <= index) && (index < itemsDictionary.Count)) {
          return itemsDictionary[keysList[index]];
        } else {
          throw
            new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionIndexOutOfRange, index);
        }
      }
    }

    /// <summary>Gets the number of elements contained in the collection.</summary>
    public int Count {
      get { return keysList.Count; }
    }

    /// <summary>Gets a value indicating whether access to the collection is synchronizated
    /// (thread safe).</summary>
    public bool IsSynchronized {
      get { return this.isSynchronized; }
    }

    /// <summary>Gets a value indicating whether the collection is read only.</summary>
    public bool IsReadOnly {
      get { return this.isReadOnly; }
    }

    public string Name {
      get { return this.name; }
    }

    #endregion Public properties

    #region Public methods

    public bool Contains(TKey key) {
      return itemsDictionary.ContainsKey(key);
    }

    public bool Contains(TItem item) {
      return itemsDictionary.ContainsValue(item);
    }

    /// <summary>Copies the elements of this instance of collection to an instance of the Array
    ///  class, starting at a particular index.</summary>
    /// <param name="array">The target array to which the collection will be copied.</param>
    /// <param name="index">The zero-based index in the target Array where copying begins.</param>
    public void CopyTo(TItem[] array, int index) {
      int arrayIndex = 0;
      for (int i = index; i < keysList.Count; i++) {
        array.SetValue(itemsDictionary[keysList[i]], arrayIndex);
        arrayIndex++;
      }
    }

    /// <summary>Copies the elements of this instance of collection to an instance of the Array
    ///  class, starting at a particular index.</summary>
    /// <param name="array">The target array to which the collection will be copied.</param>
    /// <param name="index">The zero-based index in the target Array where copying begins.</param>
    void ICollection<TItem>.CopyTo(TItem[] array, int index) {
      int arrayIndex = 0;
      for (int i = index; i < keysList.Count; i++) {
        array.SetValue(itemsDictionary[keysList[i]], arrayIndex);
        arrayIndex++;
      }
    }

    /// <summary>Gets an IEnumerator that can iterate through the collection.</summary>
    public IEnumerator<TItem> GetEnumerator() {
      return itemsDictionary.Values.GetEnumerator();
    }

    /// <summary>Gets an IEnumerator that can iterate through the collection.</summary>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return itemsDictionary.Values.GetEnumerator();
    }

    /// <summary>Gets the key of the object at the specified index of the collection.</summary>
    /// <param name="index">The index of the key to get.</param>
    /// <returns>The name of the parameter at the specified index of the collection.</returns>
    public TKey GetKeyAt(int index) {
      if ((0 <= index) && (index < keysList.Count)) {
        return keysList[index];
      } else {
        throw new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionIndexOutOfRange, index);
      }
    }

    public TKey[] Keys {
      get { return keysList.ToArray(); }
    }

    public TItem[] Values {
      get {
        TItem[] array = new TItem[this.Count];
        for (int i = 0; i < this.keysList.Count; i++) {
          array[i] = this.itemsDictionary[this.keysList[i]];
        }
        return array;
      }
    }

    #endregion Public methods

    #region Protected methods

    protected void Add(TKey key, TItem item) {
      if (Contains(key)) {
        throw new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionItemAlreadyExists, key);
      }
      itemsDictionary.Add(key, item);
      keysList.Add(key);
    }

    void ICollection<TItem>.Add(TItem item) {
      throw new NotImplementedException();
    }

    protected void Clear() {
      itemsDictionary.Clear();
      keysList.Clear();
    }

    void ICollection<TItem>.Clear() {
      itemsDictionary.Clear();
      keysList.Clear();
    }

    public TItem GetItem(TKey key) {
      if (Contains(key)) {
        return itemsDictionary[key];
      } else {
        throw new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionKeyNotFound, key);
      }
    }

    public int IndexOf(TKey key) {
      return keysList.IndexOf(key);
    }

    protected void Load(DataTable table, string keyField, string valueField) {
      Clear();
      for (int i = 0; i < table.Rows.Count; i++) {
        Add((TKey) table.Rows[i][keyField], (TItem) table.Rows[i][valueField]);
      }
    }

    protected void Load(DataView view, string keyField, string valueField) {
      Clear();
      for (int i = 0; i < view.Count; i++) {
        Add((TKey) view[i][keyField], (TItem) view[i][valueField]);
      }
    }

    protected TItem Remove(TKey key) {
      if (this.Contains(key)) {
        TItem item = itemsDictionary[key];
        itemsDictionary.Remove(key);
        keysList.Remove(key);

        return item;
      } else {
        throw new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionKeyNotFound, key);
      }
    }

    bool ICollection<TItem>.Remove(TItem item) {
      throw new NotImplementedException();
    }

    /// <summary>Sets the item with a specific key from a Collection.</summary>
    /// <param name="key">The key of the item to set its value.</param>
    /// <param name="item">The new item.</param>
    protected void SetItem(TKey key, TItem item) {
      if (Contains(key)) {
        itemsDictionary[key] = item;
      } else {
        throw new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionKeyNotFound, key);
      }
    }

    /// <summary>Sets the item with a specific index from a Collection.</summary>
    /// <param name="index">The index of the object to set its value.</param>
    /// <param name="item">The new item value.</param>
    protected void SetItemAt(int index, TItem item) {
      if ((0 <= index) && (index < itemsDictionary.Count)) {
        TKey key = keysList[index];
        itemsDictionary[key] = item;
      } else {
        throw new EmpiriaCollectionException(EmpiriaCollectionException.Msg.CollectionIndexOutOfRange, index);
      }
    }

    #endregion Protected methods

  } //class EmpiriaCollection

} //namespace Empiria.Collections
