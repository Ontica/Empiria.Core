/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              License  : Please read LICENSE.txt file      *
*  Type      : AssortedDictionary                               Pattern  : HashList Class                    *
*                                                                                                            *
*  Summary   : Represents an assorted objects dictionary retrived by a string key.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Collections;
using System.Collections.Generic;

namespace Empiria.Collections {

  /// <summary>Represents an assorted objects dictionary retrived by a string key.</summary>
  public class AssortedDictionary : IEnumerable {

    #region Fields

    private readonly Dictionary<string, object> items = null;

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
      Assertion.Require(key, "key");
      Assertion.Require(item, "item");

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
      Assertion.Require(key, "key");
      Assertion.Require(item, "item");

      if (!this.ContainsKey(key)) {
        items.Add(key, item);
      } else {
        items[key] = item;
      }
    }

    #endregion Public methods

  }  // class AssortedDictionary

}  // namespace Empiria.Collections
