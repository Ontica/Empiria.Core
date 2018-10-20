/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Json services                                Component : Json provider                         *
*  Assembly : Empiria.Core.dll                             Pattern   : General Object                        *
*  Type     : StoredJson                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Gets a json object stored in a general purpose repository.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Json {

  /// <summary>Gets a json object stored in a general purpose repository.</summary>
  public class StoredJson : GeneralObject {

    #region Constructors and parsers

    private StoredJson() {
      // Required by Empiria Framework.
    }

    static public StoredJson Parse(int id) {
      return BaseObject.ParseId<StoredJson>(id);
    }

    static public StoredJson Parse(string namedKey) {
      return BaseObject.ParseKey<StoredJson>(namedKey);
    }

    static public StoredJson Empty {
      get { return BaseObject.ParseEmpty<StoredJson>(); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public JsonObject Value {
      get {
        return base.ExtendedDataField;
      }
    }

    #endregion Public properties

  } // class StoredJson

} // namespace Empiria.Json
