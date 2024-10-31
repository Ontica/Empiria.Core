/* Empiria Storage *******************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Value type                              *
*  Type     : FileData                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a physical file.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

namespace Empiria.Storage {

  /// <summary>Holds information about a physical file.</summary>
  public class FileData {

    #region Constructors and parsers

    private FileData(JsonObject json) {
      this.ExtData = json;
    }

    private FileData(InputFile inputFile) {
      this.AppContentType = inputFile.AppContentType;
      this.MediaType = inputFile.MediaType;
      this.MediaLength = inputFile.MediaLength;
      this.OriginalFileName = inputFile.OriginalFileName;
      this.FileUID = inputFile.FileUID;
      this.FileName = inputFile.FileName;
      this.FileExtension = inputFile.FileExtension;
      this.FileTimestamp = inputFile.FileTimestamp;
    }

    static public FileData Parse(InputFile inputFile) {
      Assertion.Require(inputFile, nameof(inputFile));

      return new FileData(inputFile);
    }

    static public FileData Parse(string jsonString) => new FileData(JsonObject.Parse(jsonString));

    static internal FileData Empty => new FileData(JsonObject.Empty);

    #endregion Constructors and parsers

    #region Properties

    private JsonObject ExtData {
      get; set;
    } = new JsonObject();


    public string AppContentType {
      get {
        return ExtData.Get("appContentType", string.Empty);
      }
      private set {
        ExtData.SetIfValue("appContentType", value);
      }
    }


    public string MediaType {
      get {
        return ExtData.Get("mediaType", string.Empty);
      }
      private set {
        ExtData.SetIfValue("mediaType", value);
      }
    }


    public long MediaLength {
      get {
        return ExtData.Get("mediaLength", 0);
      }
      private set {
        ExtData.SetIfValue("mediaLength", value);
      }
    }


    public string OriginalFileName {
      get {
        return ExtData.Get("originalFileName", string.Empty);
      }
      private set {
        ExtData.SetIfValue("originalFileName", value);
      }
    }


    public string FileUID {
      get {
        return ExtData.Get("fileUID", string.Empty);
      }
      private set {
        ExtData.SetIfValue("fileUID", value);
      }
    }


    public string FileName {
      get {
        return ExtData.Get("fileName", string.Empty);
      }
      private set {
        ExtData.SetIfValue("fileName", value);
      }
    }


    public string FileExtension {
      get {
        return ExtData.Get("fileExtension", string.Empty);
      }
      private set {
        ExtData.SetIfValue("fileExtension", value);
      }
    }


    public DateTime FileTimestamp {
      get {
        return ExtData.Get("fileTimestamp", DateTime.Now);
      }
      private set {
        ExtData.SetIfValue("fileTimestamp", value);
      }
    }

    #endregion Properties

    #region Methods

    public override string ToString() {
      return ExtData.ToString();
    }

    #endregion Methods

  }  // class FileData

} // namespace Empiria.Storage
