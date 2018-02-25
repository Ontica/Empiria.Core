/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ConfigurationSetting                             Pattern  : Static Class                      *
*                                                                                                            *
*  Summary   : Application configuration element.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria {

  /// <summary> Application configuration element.</summary>
  internal class ConfigurationSetting {

    private ConfigurationSetting() {

    }

    static ConfigurationSetting Parse(JsonObject json) {
      var config = new ConfigurationSetting();

      config.TypeName = json.Get<string>("type");
      config.Key = json.Get<string>("key");
      config.Value = json.Get<string>("value");

      return config;
    }

    public string TypeName {
      get;
      private set;
    }

    public string Key {
      get;
      private set;
    }

    public string Value {
      get;
      private set;
    }

  }  // class ConfigurationSetting

} //namespace Empiria
