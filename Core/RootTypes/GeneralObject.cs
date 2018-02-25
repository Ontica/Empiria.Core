/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : GeneralObject                                    Pattern  : Storage Item                      *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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

    /// ToDo: Replace this method by BaseObject.GetList<T>(filter, sort)
    ///       when that method will be able to return in the list derivated types of <T>.
    static protected new FixedList<T> GetList<T>(string filter = "", string sort = "") where T : BaseObject {
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
