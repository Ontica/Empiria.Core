
using System;

namespace Empiria.Json {

  public class JsonItem {

    public JsonItem(string key, string value) {
      this.Key = key;
      this.Value = value;
    }


    public JsonItem(string key, Exception e) {
      this.Key = key;
      this.Value = e.Message;
    }

    public string Key {
      get;
      private set;
    }

    public string Value {
      get;
      private set;
    }

  }  // class JsonItem

}  // namespace Empiria.Json
