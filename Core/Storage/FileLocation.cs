/* Empiria Storage *******************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : FileLocation                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a physical file directory or a documents web site.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

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

  }  // class FileLocation

}  // namespace Empiria.Storage
