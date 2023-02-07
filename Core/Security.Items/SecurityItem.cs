/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Partitioned Type                      *
*  Type     : SecurityItem                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Abstract type that represents a security item. Security items are roles, user credentials,     *
*             claims, features, application keys, or other objects used for authorization or                 *
*             authentication. SecurityItem is a partitioned type of SecurityItemType.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria.Security.Items {

  /// <summary>Abstract type that represents a security item. Security items are roles, user credentials,
  /// claims, features, application keys, or other objects used for authorization or authentication.
  /// SecurityItem is a partitioned type of SecurityItemType.</summary>
  [PartitionedType(typeof(SecurityItemType))]
  abstract internal class SecurityItem : BaseObject {

    protected SecurityItem(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public SecurityItem Parse(int id) {
      return BaseObject.ParseId<SecurityItem>(id);
    }

    static public T Parse<T>(int id) where T : SecurityItem {
      return BaseObject.ParseId<T>(id);
    }


    static public T Parse<T>(string uid) where T : SecurityItem {
      return BaseObject.ParseKey<T>(uid);
    }

    #region Properties

    public SecurityItemType SecurityItemType {
      get {
        return (SecurityItemType) base.GetEmpiriaType();
      }
    }

    [DataField("ContextId")]
    protected int ContextId {
      get; private set;
    }


    [DataField("SubjectId")]
    protected int BaseSubjectId {
      get; private set;
    }


    [DataField("TargetId")]
    protected int TargetId {
      get; private set;
    }


    [DataField("SecurityItemKey")]
    protected string BaseKey {
      get; private set;
    }


    [DataField("SecurityItemValue")]
    protected JsonObject ExtensionData {
      get; private set;
    }


    [DataField("LastUpdate")]
    public DateTime LastUpdate {
      get; private set;
    }


    [DataField("UpdatedById")]
    public Contact UpdatedBy {
      get; private set;
    }


    [DataField("SecurityItemStatus", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

  }  // class SecurityItem

}  // namespace Empiria.Security.Items
