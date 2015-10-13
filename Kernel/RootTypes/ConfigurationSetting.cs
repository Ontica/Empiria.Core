/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ConfigurationSetting                             Pattern  : Static Class                      *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Application configuration element.                                                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
