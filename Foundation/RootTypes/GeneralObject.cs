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

using Empiria.Contacts;
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

    static protected FixedList<T> ParseList<T>() where T : BaseObject {
      ObjectTypeInfo objectTypeInfo = ObjectTypeInfo.Parse<T>();

      DataTable table = OntologyData.GetSimpleObjectsDataTable(objectTypeInfo);
      List<T> list = new List<T>(table.Rows.Count);
      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(BaseObject.ParseDataRow<T>(table.Rows[i]));
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
    public GeneralObjectStatus Status {
      get;
      protected set;
    }

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
