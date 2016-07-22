/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : GeneralObject                                    Pattern  : Storage Item                      *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Ontology;

namespace Empiria {

  /// <summary>Abstract type that holds basic object instances which are
  /// stored in a general common table</summary>
  public abstract class GeneralObject : BaseObject {

    #region Fields

    /// <summary>Use this field name in derived types to access extended fields items.</summary>
    protected const string ExtensionDataFieldName = "ObjectExtData";

    #endregion Fields

    #region Constructors and parsers

    protected GeneralObject() {
      // Required by Empiria Framework.
    }

    static protected FixedList<T> ParseList<T>(string filter = "", string sort = "") where T : BaseObject {
      ObjectTypeInfo objectTypeInfo = ObjectTypeInfo.Parse<T>();

      DataView view = OntologyData.GetSimpleObjects(objectTypeInfo, filter, sort);
      List<T> list = new List<T>(view.Count);
      for (int i = 0; i < view.Count; i++) {
        list.Add(BaseObject.ParseDataRow<T>(view[i].Row));
      }
      return list.ToFixedList();
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("ObjectKey")]
    protected string NamedKey {
      get;
      set;
    }

    [DataField("ObjectName")]
    [Newtonsoft.Json.JsonIgnore]
    public string Name {
      get;
      protected set;
    }

    [DataField(GeneralObject.ExtensionDataFieldName, IsOptional=true)]
    protected Json.JsonObject ExtendedDataField {
      get;
      set;
    }

    [DataField("ObjectKeywords")]
    protected string Keywords {
      get;
      set;
    }

    [DataField("ObjectStatus", Default = GeneralObjectStatus.Active)]
    [Newtonsoft.Json.JsonIgnore]
    public GeneralObjectStatus Status {
      get;
      protected set;
    }

    [Newtonsoft.Json.JsonIgnore]
    public string StatusName {
      get {
        switch (this.Status) {
          case GeneralObjectStatus.Active:
            return "Activo";
          case GeneralObjectStatus.Deleted:
            return "Eliminado";
          case GeneralObjectStatus.Pending:
            return "Pendiente";
          case GeneralObjectStatus.Suspended:
            return "Suspendido";
          default:
            return "No determinado";
        }
      }
    }

    #endregion Public properties

  } // class GeneralObject

} // namespace Empiria
