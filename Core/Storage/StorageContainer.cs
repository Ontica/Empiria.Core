/* Empiria Storage *******************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Storage.dll                        Pattern   : Service provider                        *
*  Type     : StorageContainer                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a stored media object treated as a value type, so it must be related to             *
*             other objects like metadata information holders or document entities.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Storage {

  /// <summary>Represents a stored media object treated as a value type, so it must be related to other
  /// objects like metadata information holders or document entities.</summary>
  public class StorageContainer : StorageItem {

    #region Constructors and parsers

    protected StorageContainer() {
      // Required by Empiria Framework
    }


    static public new StorageContainer Parse(int id) {
      return BaseObject.ParseId<StorageContainer>(id);
    }


    static public new StorageContainer Parse(string uid) {
      return BaseObject.ParseKey<StorageContainer>(uid);
    }


    #endregion Constructors and parsers

    #region Properties

    [DataField("AppContentType")]
    public string ContainerType {
      get;
      private set;
    }


    [DataField("StorageId")]
    public Storage Storage {
      get;
      private set;
    }


    [DataField("ItemPath")]
    internal string RelativePath {
      get;
      private set;
    }


    public string BasePath {
      get {
        return $"{this.Storage.BasePath}\\{this.RelativePath}";
      }
    }


    public string BaseUrl {
      get {
        return $"{this.Storage.BaseUrl}/{this.RelativePath}";
      }
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.ContainerType, this.Name);
      }
    }

    #endregion Properties

    #region Methods

    public StorageFile Add(InputFile inputFile) {
      throw new NotImplementedException();
    }


    public StorageFile GetFile(string fileUID) {
      throw new NotImplementedException();
    }


    protected override void OnSave() {
      StorageData.WriteStorageContainer(this);
    }


    public void Remove(StorageFile file) {
      throw new NotImplementedException();
    }

    #endregion Methods

  }  // class StorageContainer

}  // namespace Empiria.Storage
