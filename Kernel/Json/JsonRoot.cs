using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.Json {

  public class JsonRoot : IEnumerable {

    #region Fields

    private Dictionary<string, object> items = new Dictionary<string, object>();

    #endregion Fields

    #region Constructors and parsers

    static public JsonRoot Empty {
      get {
        return new JsonRoot();
      }
    }

    #endregion Constructors and parsers

    #region Public methods

    public void Add(JsonItem item) {
      items.Add(item.Key, item.Value);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return items.GetEnumerator();
    }

    public Dictionary<string, object> ToDictionary() {
      return items;
    }

    public override string ToString() {
      return JsonConverter.ToJson(this.items);
    }

    #endregion Public methods

  }  // class JsonRoot

}  // namespace Empiria.Json
