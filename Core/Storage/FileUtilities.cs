/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Services Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Static methods library                  *
*  Type     : FileUtilities                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains methods for file handling.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;
using System.Text;

using Empiria.Security;

namespace Empiria.Storage {

  /// <summary>Contains methods for file handling.</summary>
  static public class FileUtilities {


    static public string CalculateStreamHashCode(Stream stream) {
      Assertion.Require(stream, nameof(stream));

      stream.Position = 0;

      byte[] array = StreamToArray(stream);

      stream.Position = 0;

      string hash = Cryptographer.CreateHashCode(array);

      if (hash.Length > 128) {
        return hash.Substring(0, 128);
      } else {
        return hash;
      }
    }


    static public string CombinePath(string path1, string path2, string path3 = "") {
      Assertion.Require(path1, nameof(path1));
      Assertion.Require(path2, nameof(path2));
      Assertion.Require(path3 != null, nameof(path3));

      if (path3.Length == 0) {
        return Path.Combine(path1, path2);
      } else {
        return Path.Combine(path1, path2, path3);
      }
    }


    static public string CombineUrl(string path1, string path2, string path3 = "") {
      path1 = SetSlashChars(path1);
      path2 = SetSlashChars(path2);
      path3 = SetSlashChars(path3);

      Assertion.Require(path1, nameof(path1));
      Assertion.Require(path2, nameof(path2));

      string temp = path1 + "/" + path2;

      if (path3.Length != 0) {
        temp += "/" + path3;
      }
      return temp;
    }


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


    static internal void MoveFile(string fileFullPath, string targetPath) {
      var file = new FileInfo(fileFullPath);

      var targetFullPath = CombinePath(targetPath, file.Name);

      EnsureExistsFolderForFullPath(targetFullPath);

      file.CopyTo(targetFullPath);

      file.Delete();
    }


    static internal FileInfo SaveFile(string fullPath, InputFile file) {

      EnsureExistsFolderForFullPath(fullPath);

      using (FileStream outputStream = File.OpenWrite(fullPath)) {
        file.Stream.Position = 0;

        file.Stream.CopyTo(outputStream);
      }

      return new FileInfo(fullPath);
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

    private static void EnsureExistsFolderForFullPath(string fullPath) {
      string folderPath = fullPath.Substring(0, fullPath.LastIndexOf('\\'));

      if (!Directory.Exists(folderPath)) {
        Directory.CreateDirectory(folderPath);
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


    static private byte[] StreamToArray(Stream stream) {
      byte[] array = new byte[stream.Length];

      stream.Read(array, 0, (int) stream.Length);

      return array;
    }


    static private string SetSlashChars(string path) {
      if (String.IsNullOrWhiteSpace(path)) {
        return string.Empty;
      }

      string temp = path.TrimStart('/');
      temp = temp.TrimEnd('\\');

      temp = temp.Replace('\\', '/');

      return temp;
    }

    #endregion Helpers

  }  // class FileUtilities

}  // namespace Empiria.Storage
