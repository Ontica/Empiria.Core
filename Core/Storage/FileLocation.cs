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
  public class FileLocation : GeneralObject {

    #region Constructors and parsers

    static public FileLocation Parse(int id) => ParseId<FileLocation>(id);

    static public FileLocation Parse(string uid) => ParseKey<FileLocation>(uid);

    static public FileLocation Empty => ParseEmpty<FileLocation>();

    #endregion Constructors and parsers

    #region Properties

    public string BaseUrl {
      get {
        return ExtendedDataField.Get<string>("baseUrl");
      }
    }


    public string BaseFileDirectory {
      get {
        return ExtendedDataField.Get<string>("baseFileDirectory");
      }
    }

    #endregion Properties

    #region Mehtods

    public FileData Store(InputFile file) {
      Assertion.Require(file, nameof(file));

      string path = Path.Combine(BaseFileDirectory, file.FileName);

      FileInfo fileInfo = FileUtilities.SaveFile(path, file);

      return FileData.Parse(file);
    }

    #endregion Methods

  }  // class FileLocation

}  // namespace Empiria.Storage
