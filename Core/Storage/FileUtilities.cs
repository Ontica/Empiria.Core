/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : File storage services                   *
*  Assembly : Empiria.Core.dll                           Pattern   : Static methods library                  *
*  Type     : FileUtilities                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains methods for file handling.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.IO;
using System.Text;

namespace Empiria.Storage {

  /// <summary>Contains methods for file handling.</summary>
  static public class FileUtilities {


    static public string GetFullPath(string fileName) {
      return Path.Combine(ImportedFilesStoragePath, fileName);
    }


    static public string[] ReadTextFile(FileInfo textFile) {
      var ansiEncoding = Encoding.GetEncoding(1252);

      return File.ReadAllLines(textFile.FullName, ansiEncoding);
    }


    static public FileInfo SaveFile(InputFile file) {
      string fileFullPath = ImportedFilePath(file);

      using (FileStream outputStream = File.OpenWrite(fileFullPath)) {
        file.Stream.CopyTo(outputStream);
      }

      return new FileInfo(fileFullPath);
    }


    #region Helpers

    static private Encoding GetFileEncoding(FileInfo textFile) {
      using (var reader = new StreamReader(textFile.FullName, true)) {
        while (reader.Peek() >= 0) {
          reader.Read();
        }

        return reader.CurrentEncoding;
      }
    }

    static private string ImportedFilesStoragePath {
      get {
        return ConfigurationData.Get<string>("ImportedFiles.StoragePath");
      }
    }


    static private string ImportedFilePath(InputFile file) {
      var copyFileName = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss-") + file.OriginalFileName;

      return GetFullPath(copyFileName);
    }

    #endregion Helpers

  }  // class FileUtilities

}  // namespace Empiria.Storage
