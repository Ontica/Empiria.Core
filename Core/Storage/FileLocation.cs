/* Empiria Storage *******************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : FileLocation                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a physical file directory or a documents web site.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.IO;

namespace Empiria.Storage {

  /// <summary>Holds information about a physical file directory or a documents web site.</summary>
  public class FileLocation : CommonStorage {

    #region Constructors and parsers

    static private string DEFAULT_BASE_FILE_DIRECTORY = ConfigurationData.GetString("Documents.Storage.Path");
    static private string DEFAULT_BASE_URL = ConfigurationData.GetString("Documents.Storage.BaseUrl");

    static public FileLocation Parse(int id) => ParseId<FileLocation>(id);

    static public FileLocation Parse(string uid) => ParseKey<FileLocation>(uid);

    static public FileLocation Empty => ParseEmpty<FileLocation>();

    #endregion Constructors and parsers

    #region Properties

    public string BaseUrl {
      get {
        if (string.IsNullOrEmpty(RelativeUrl) && !string.IsNullOrEmpty(RelativePath)) {
          return $"{DEFAULT_BASE_URL}/{RelativePath}";
        }
        return $"{DEFAULT_BASE_URL}/{RelativeUrl}";
      }
    }


    public string BaseFileDirectory {
      get {
        if (string.IsNullOrEmpty(RelativePath)) {
          return DEFAULT_BASE_FILE_DIRECTORY;
        }
        return Path.Combine(DEFAULT_BASE_FILE_DIRECTORY, RelativePath);
      }
    }


    private string RelativePath {
      get {
        return base.ExtData.Get("relativePath", string.Empty);
      }
    }


    private string RelativeUrl {
      get {
        return base.ExtData.Get("relativeUrl", string.Empty);
      }
    }

    #endregion Properties

    #region Methods

    public string GetFileFullLocalName(FileData fileData) {
      return Path.Combine(BaseFileDirectory, fileData.FileName);
    }


    public FileData Store(InputFile file) {
      Assertion.Require(file, nameof(file));

      string path = Path.Combine(BaseFileDirectory, file.FileName);

      FileInfo fileInfo = FileUtilities.SaveFile(path, file);

      return FileData.Parse(file);
    }

    #endregion Methods

  }  // class FileLocation

}  // namespace Empiria.Storage
