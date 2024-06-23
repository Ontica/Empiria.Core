/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Data Transfer Object              *
*  Type     : InputFile                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object for input file streams.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.IO;

namespace Empiria.Storage {

  /// <summary>Data transfer object for input file streams.</summary>
  public class InputFile {

    public InputFile(Stream stream,
                     string appContentType,
                     string mediaType,
                     string originalFileName) {
      Assertion.Require(stream, nameof(stream));
      Assertion.Require(appContentType, nameof(appContentType));
      Assertion.Require(mediaType, nameof(mediaType));
      Assertion.Require(originalFileName, nameof(originalFileName));

      this.Stream = stream;
      this.AppContentType = appContentType;
      this.MediaType = mediaType;
      this.OriginalFileName = originalFileName;
    }


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

  }  // class InputFile

}  // namespace Empiria.Storage
