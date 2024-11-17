/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Base Objects                               Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : CommonStorageData                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds separated data for CommonStorage instances to be used by derived types.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>Holds separated data for CommonStorage instances to be used by derived types.</summary>
  public class CommonStorageData {

    #region Public properties

    [DataField("OBJECT_CATEGORY_ID")]
    public int ObjectCategoryId {
      get; set;
    }


    [DataField("OBJECT_CLASSIFICATION_ID")]
    public int ObjectClassificationId {
      get; set;
    }


    [DataField("OBJECT_NAMED_KEY")]
    public string NamedKey {
      get; set;
    }


    [DataField("OBJECT_CODE")]
    public string Code {
      get; set;
    }


    [DataField("OBJECT_LINKED_TYPE_ID")]
    public int LinkedTypeId {
      get; set;
    }


    [DataField("OBJECT_LINKED_OBJECT_ID")]
    public int LinkedObjectId {
      get; set;
    }


    [DataField("OBJECT_IDENTIFICATORS")]
    public string Identificators {
      get; set;
    }


    [DataField("OBJECT_TAGS")]
    public string Tags {
      get; set;
    }


    [DataField("PARENT_OBJECT_ID")]
    public int ParentObjectId {
      get; set;
    }

    #endregion Public properties

  } // class CommonStorageData

} // namespace Empiria
