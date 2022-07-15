/* Empiria Storage *******************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Storage.dll                        Pattern   : Abstract type                           *
*  Type     : StorageItem                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : This is the base class for all storage items, including containers and files.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.StateEnums;

namespace Empiria.Storage {

  /// <summary>This is the base class for all storage items, including containers and files.</summary>
  public abstract class StorageItem : BaseObject {

    #region Constructors and parsers

    protected StorageItem() {
      // Required by Empiria Framework
    }

    protected StorageItem(string name, int size) {
      Assertion.Require(name, nameof(name));
      Assertion.Require(size >= 0, "Size can not be a negative integer.");

      this.Name = name;
      this.Size = size;
    }


    static public StorageItem Parse(int id) {
      return BaseObject.ParseId<StorageItem>(id);
    }


    static public StorageItem Parse(string uid) {
      return BaseObject.ParseKey<StorageItem>(uid);
    }


    #endregion Constructors and parsers

    #region Properties


    [DataField("ItemName")]
    public string Name {
      get;
      private set;
    }


    [DataField("ItemSize")]
    public int Size {
      get;
      private set;
    }


    [DataField("ItemExtData")]
    internal protected JsonObject ExtensionData {
      get;
      private set;
    } = JsonObject.Empty;


    [DataField("PostedById")]
    public Contact PostedBy {
      get;
      private set;
    } = Contact.Empty;


    [DataField("PostingTime")]
    public DateTime PostingTime {
      get;
      private set;
    } = DateTime.Now;


    [DataField("ItemStatus", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      private set;
    } = EntityStatus.Active;


    #endregion Properties

    #region Methods


    internal void Delete() {
      Assertion.Require(this.Status == EntityStatus.Active,
                       "MediaStorage must be in 'Active' status to be deleted.");

      this.Status = EntityStatus.Deleted;
    }


    protected override void OnBeforeSave() {
      if (this.IsNew) {
        PostedBy = ExecutionServer.CurrentIdentity.User.AsContact();
        PostingTime = DateTime.Now;
      }
    }


    protected override void OnSave() {
      throw Assertion.EnsureNoReachThisCode(
        "OnSave() method must be implemented in derived types, but was not."
      );
    }

    #endregion Methods

  }  // class StorageItem

} // namespace Empiria.Storage
