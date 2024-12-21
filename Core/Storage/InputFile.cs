/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Data Transfer Object              *
*  Type     : InputFile                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object for input file streams.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;

namespace Empiria.Storage {

  /// <summary>Data transfer object for input file streams.</summary>
  public class InputFile {

    #region Constructors and parsers

    public InputFile(FileInfo fileInfo) {
      Assertion.Require(fileInfo, nameof(fileInfo));

      Stream = fileInfo.OpenRead();
      AppContentType = string.Empty;
      MediaType = string.Empty;
      OriginalFileName = fileInfo.Name;
      FileUID = GenerateUID();
      FileTimestamp = DateTime.Now;
      FileExtension = GetFileExtension(OriginalFileName);
    }


    public InputFile(Stream stream,
                     string mediaType,
                     string originalFileName,
                     string appContentFile) {
      Assertion.Require(stream, nameof(stream));
      Assertion.Require(mediaType, nameof(mediaType));
      Assertion.Require(originalFileName, nameof(originalFileName));

      Stream = stream;
      MediaType = mediaType;
      OriginalFileName = originalFileName;
      FileUID = GenerateUID();
      FileTimestamp = DateTime.Now;
      FileExtension = GetFileExtension(OriginalFileName);
      AppContentType = appContentFile ?? string.Empty;
    }

    #endregion Constructors and parsers

    #region Properties

    public Stream Stream {
      get;
    }


    public string AppContentType {
      get;
    }


    public string MediaType {
      get;
    }

    public long MediaLength {
      get {
        return this.Stream.Length;
      }
    }

    public string OriginalFileName {
      get;
    }

    public string FileUID {
      get; private set;
    } = string.Empty;


    public string FileName {
      get {
        return $"{FileTimestamp.ToString("yyyy.MM.dd-HH.mm.ss.ff")}-{OriginalFileName}";
      }
    }


    public DateTime FileTimestamp {
      get; private set;
    } = ExecutionServer.DateMaxValue;


    public string FileExtension {
      get; private set;
    } = string.Empty;


    #endregion Properties

    #region Helpers

    static private string GetFileExtension(string fileName) {
      var extension = Path.GetExtension(fileName);

      extension = EmpiriaString.TrimAll(extension, ".", string.Empty);

      return EmpiriaString.Clean(extension);
    }

    static private string GenerateUID() {
      return Guid.NewGuid().ToString().ToLowerInvariant();
    }

    #endregion Helpers

  }  // class InputFile

}  // namespace Empiria.Storage
