﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : GeneralObject                                    Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
    protected const string ExtensionDataFieldName = "GeneralObjectExtData";

    #endregion Fields

    #region Constructors and parsers

    protected GeneralObject() {
      // Required by Empiria Framework.
    }

    static protected FixedList<T> ParseList<T>() where T : BaseObject {
      ObjectTypeInfo objectTypeInfo = ObjectTypeInfo.Parse<T>();

      DataTable table = OntologyData.GetGeneralObjectsDataTable(objectTypeInfo);
      List<T> list = new List<T>(table.Rows.Count);
      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(BaseObject.ParseDataRow<T>(table.Rows[i]));
      }
      return list.ToFixedList();
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("GeneralObjectDescription")]
    protected string Description {
      get;
      set;
    }

    [DataField("EndDate")]
    protected DateTime EndDate {
      get;
      set;
    }

    [DataField(GeneralObject.ExtensionDataFieldName)]
    protected string ExtendedDataField {
      get;
      set;
    }

    [DataField("GeneralObjectValue")]
    protected string ValueField {
      get;
      set;
    }

    [DataField("GeneralObjectIndex", Default = -1)]
    protected int Index {
      get;
      set;
    }

    [DataField("GeneralObjectKeywords")]
    protected string Keywords {
      get;
      set;
    }

    [DataField("GeneralObjectName")]
    public string Name {
      get;
      protected set;
    }

    [DataField("GeneralObjectNamedKey")]
    protected string NamedKey {
      get;
      set;
    }

    [DataField("PostedById")]
    protected Contact PostedBy {
      get;
      set;
    }

    [DataField("GeneralObjectReferenceId", Default = -1)]
    protected int ReferenceId {
      get;
      set;
    }

    [DataField("ReplacedById")]
    protected int ReplacedById {
      get;
      private set;
    }

    [DataField("StartDate", Default = "DateTime.Today")]
    protected DateTime StartDate {
      get;
      private set;
    }

    [DataField("GeneralObjectStatus", Default = GeneralObjectStatus.Active)]
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
