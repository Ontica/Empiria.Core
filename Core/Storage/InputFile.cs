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
      Stream = fileInfo.OpenRead();
      AppContentType = string.Empty;
      MediaType = string.Empty;
      OriginalFileName = fileInfo.Name;
      FileUID = Guid.NewGuid().ToString().ToLowerInvariant();
      FileTimestamp = fileInfo.LastWriteTime;
      FileExtension = fileInfo.Extension;
      FileName = $"{FileUID}-{FileTimestamp.ToString("yyyy.MM.dd-HH.mm.ss")}.{this.FileExtension}";
      IsStored = true;
    }

    public InputFile(Stream stream,
                     string appContentType,
                     string mediaType,
                     string originalFileName) {
      Assertion.Require(stream, nameof(stream));
      Assertion.Require(appContentType, nameof(appContentType));
      Assertion.Require(mediaType, nameof(mediaType));
      Assertion.Require(originalFileName, nameof(originalFileName));

      Stream = stream;
      AppContentType = appContentType;
      MediaType = mediaType;
      OriginalFileName = originalFileName;
      FileUID = Guid.NewGuid().ToString().ToLowerInvariant();
      FileTimestamp = DateTime.Now;
      FileExtension = Path.GetExtension(this.OriginalFileName);
      FileName = $"{FileTimestamp.ToString("yyyy.MM.dd-HH.mm.ss")}-{OriginalFileName}";
      IsStored = false;
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
      get; private set;
    } = string.Empty;


    public DateTime FileTimestamp {
      get; private set;
    } = ExecutionServer.DateMaxValue;


    public string FileExtension {
      get; private set;
    } = string.Empty;


    public bool IsStored {
      get; private set;
    }

    public FileInfo Store(string baseFileDirectory) {
      Assertion.Require(baseFileDirectory, nameof(baseFileDirectory));
      Assertion.Require(!IsStored, "File was already stored");

      string path = Path.Combine(baseFileDirectory, FileName);

      FileInfo fileInfo = FileUtilities.SaveFile(path, this);

      IsStored = true;

      return fileInfo;
    }

    #endregion Properties

  }  // class InputFile

}  // namespace Empiria.Storage
