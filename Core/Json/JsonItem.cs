/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : JSON Data Services                *
*  Namespace : Empiria.Json                                     License  : Please read LICENSE.txt file      *
*  Type      : JsonItem                                         Pattern  : Information Holder                *
*                                                                                                            *
*  Summary   : Represents a json item.                                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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
