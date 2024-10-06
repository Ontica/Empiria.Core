/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Value Type                              *
*  Type     : MediaData                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data structure for media data.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Storage {

  /// <summary>Data structure for media data.</summary>
  public class MediaData {

    private MediaData() {
      this.MediaType = "Empty";
      this.Url = string.Empty;
    }

    public MediaData(string mediaType, string url) {
      Assertion.Require(mediaType, "mediaType");
      Assertion.Require(url, "url");

      this.MediaType = mediaType;
      this.Url = url;
    }


    static public MediaData Empty {
      get {
        return new MediaData();
      }
    }

    public string Url {
      get;
    }

    public string MediaType {
      get;
    }

  }  // class MediaData

}  // namespace Empiria.Storage
