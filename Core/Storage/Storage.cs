/* Empiria Storage *******************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Storage.dll                        Pattern   : Service provider                        *
*  Type     : Storage                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a stored media object treated as a value type, so it must be related to             *
*             other objects like metadata information holders or document entities.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Storage {

  /// <summary>Represents a stored media object treated as a value type, so it must be related to other
  /// objects like metadata information holders or document entities.</summary>
  public class Storage : StorageItem {

    #region Constructors and parsers

    protected Storage() {
      // Required by Empiria Framework
    }


    static public new Storage Parse(int id) {
      return BaseObject.ParseId<Storage>(id);
    }


    static public new Storage Parse(string uid) {
      return BaseObject.ParseKey<Storage>(uid);
    }


    static public Storage Empty => BaseObject.ParseEmpty<Storage>();


    #endregion Constructors and parsers

    #region Properties

    [DataField("AppContentType")]
    public string StorageType {
      get;
      private set;
    }


    [DataField("ItemPath")]
    public string BasePath {
      get;
      private set;
    }


    public string BaseUrl {
      get {
        if (this.IsEmptyInstance) {
          return string.Empty;
        }
        return this.ExtensionData.Get<string>("baseUrl");
      }
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.StorageType, this.Name);
      }
    }

    #endregion Properties

  }  // class Storage

}  // namespace Empiria.Storage
