/* Empiria Storage *******************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Storage.dll                        Pattern   : Service provider                        *
*  Type     : StorageFile                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a stored media object treated as a value type, so it must be related to             *
*             other objects like metadata information holders or document entities.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Security;

namespace Empiria.Storage {

  /// <summary>Represents a stored media object treated as a value type, so it must be related to other
  /// objects like metadata information holders or document entities.</summary>
  public class StorageFile : StorageItem, IProtected {

    #region Constructors and parsers

    protected StorageFile() {
      // Required by Empiria Framework
    }


    static public new StorageFile Parse(int id) {
      return BaseObject.ParseId<StorageFile>(id);
    }


    static public new StorageFile Parse(string uid) {
      return BaseObject.ParseKey<StorageFile>(uid);
    }


    #endregion Constructors and parsers

    #region Properties

    [DataField("AppContentType")]
    public string AppContentType {
      get;
      private set;
    }


    [DataField("MIMEContentType")]
    public string MIMEContentType {
      get;
      private set;
    }


    [DataField("OriginalItemName")]
    public string OriginalFileName {
      get;
      private set;
    }


    [DataField("ContainerId", Default = "Empiria.Storage.Container.Default")]
    public StorageContainer Container {
      get;
      private set;
    }


    [DataField("ItemPath")]
    internal string FilePath {
      get;
      private set;
    }


    public string FullPath {
      get {
        return $"{this.Container.BasePath}/{this.FilePath}/{this.Name}";
      }
    }


    [DataField("ItemHashCode")]
    public string HashCode {
      get;
      private set;
    }


    public string Url {
      get {
        return $"{this.Container.BaseUrl}/{this.FilePath}/{this.Name}";
      }
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.AppContentType, this.Name, this.OriginalFileName);
      }
    }

    #endregion Properties

    #region Integrity protection members

    int IProtected.CurrentDataIntegrityVersion {
      get {
        return 1;
      }
    }


    object[] IProtected.GetDataIntegrityFieldValues(int version) {
      if (version == 1) {
        return new object[] {
          1, "Id", "AppContentType", this.AppContentType, "MIMEContentType", this.MIMEContentType,
          "ItemSize", this.Size, "Name", this.Name, "OriginalFileName", this.OriginalFileName,
          "FilePath", this.FilePath, "ExtData", this.ExtensionData.ToString(),
          "HashCode", this.HashCode, "PostingTime", this.PostingTime, "PostedById", this.PostedBy.Id,
          "Status", (char) this.Status
        };
      }
      throw new SecurityException(SecurityException.Msg.WrongDIFVersionRequested, version);
    }


    private IntegrityValidator _validator = null;
    public IntegrityValidator Integrity {
      get {
        if (_validator == null) {
          _validator = new IntegrityValidator(this);
        }
        return _validator;
      }
    }


    #endregion Integrity protection members

    #region Methods

    protected override void OnSave() {
      StorageData.WriteStorageFile(this);
    }

    #endregion Methods

  }  // class StorageFile

}  // namespace Empiria.Storage
