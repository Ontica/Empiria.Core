/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              Assembly : Empiria.Kernel.dll                *
*  Type      : AssortedDictionary                               Pattern  : HashList Class                    *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents an assorted objects dictionary retrived by a string key.                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Represents an assorted objects dictionary retrived by a string key.</summary>
  public class AssortedDictionary : IEnumerable {

    #region Fields

    private Dictionary<string, object> items = null;

    #endregion Fields

    #region Constructors and parsers

    public AssortedDictionary(int capacity = 16) {
      items = new Dictionary<string, object>(capacity);
    }

    #endregion Constructors and parsers

    #region Properties

    public int Count {
      get {
        return items.Count;
      }
    }

    #endregion Properties

    #region Public methods

    public void Add(string key, object item) {
      Assertion.AssertObject(key, "key");
      Assertion.AssertObject(item, "item");

      if (!this.ContainsKey(key)) {
        items.Add(key, item);
      } else {
        throw new ListException(ListException.Msg.ListKeyAlreadyExists, key);
      }
    }

    public bool ContainsKey(string key) {
      return items.ContainsKey(key);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return items.GetEnumerator();
    }

    public T GetItem<T>(string key) {
      if (this.ContainsKey(key)) {
        return (T) items[key];
      }
      throw new ListException(ListException.Msg.ListKeyNotFound, key);
    }

    public bool Remove(string key) {
      return items.Remove(key);
    }

    public void SetItem(string key, object item) {
      Assertion.AssertObject(key, "key");
      Assertion.AssertObject(item, "item");

      if (!this.ContainsKey(key)) {
        items.Add(key, item);
      } else {
        items[key] = item;
      }
    }

    #endregion Public methods

  }  // class AssortedDictionary

}  // namespace Empiria.Collections
