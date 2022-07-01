/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : File storage services                   *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Data Holder                       *
*  Type     : PostedFile                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input data holder for a file stream.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.IO;

namespace Empiria.Storage {

  /// <summary>Input data holder for a file stream.</summary>
  public class InputFile {

    public InputFile(Stream stream,
                     string mediaType,
                     string originalFileName) {
      Assertion.Require(stream, nameof(stream));
      Assertion.Require(mediaType, nameof(mediaType));
      Assertion.Require(originalFileName, nameof(originalFileName));

      this.Stream = stream;
      this.MediaType = MediaType;
      this.OriginalFileName = originalFileName;
    }


    public Stream Stream {
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
