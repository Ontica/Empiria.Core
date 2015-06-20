/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Json                                     Assembly : Empiria.Kernel.dll                *
*  Type      : JsonItem                                         Pattern  : Information Holder                *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a json item.                                                                       *
*                                                                                                            *
********************************* Copyright (c) 2013-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Json {

  public class JsonItem {

    public JsonItem(string key, string value) {
      this.Key = key;
      this.Value = value;
    }

    public JsonItem(string key, int value) {
      this.Key = key;
      this.Value = value;
    }

    public JsonItem(string key, decimal value) {
      this.Key = key;
      this.Value = value;
    }

    public JsonItem(string key, bool value) {
      this.Key = key;
      this.Value = value;
    }

    public JsonItem(string key, DateTime value) {
      this.Key = key;
      this.Value = value;
    }

    public JsonItem(string key, JsonObject nestedItem) {
      this.Key = key;
      this.Value = nestedItem;
    }

    public JsonItem(string key, Exception e) {
      this.Key = key;
      this.Value = e;
    }

    public JsonItem(string key, object nestedObject) {
      this.Key = key;
      this.Value = nestedObject;
    }

    public string Key {
      get;
      private set;
    }

    public object Value {
      get;
      private set;
    }

  }  // class JsonItem

}  // namespace Empiria.Json
