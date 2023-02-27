/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Contacts Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : Contact                                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents either a person, an organization or a group that has a meaningful name and can be   *
*             contacted in some way and can play one or more roles.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;
using Empiria.StateEnums;

namespace Empiria.Contacts {

  /// <summary>Represents either a person, an organization or a group that has a meaningful name
  /// and can be contacted in some way and can play one or more roles.</summary>
  public class Contact : BaseObject, IContact, INamedEntity {

    #region Constructors and parsers

    protected Contact() {
      // Required by Empiria Framework.
    }

    static public Contact Parse(int id) {
      return BaseObject.ParseId<Contact>(id);
    }


    static public Contact Parse(string uid) {
      return BaseObject.ParseKey<Contact>(uid);
    }


    static public Contact Empty => BaseObject.ParseEmpty<Contact>();


    #endregion Constructors and parsers

    #region Properties

    [DataField("ContactFullName")]
    public string FullName {
      get;
      protected set;
    }


    [DataField("ShortName")]
    public string ShortName {
      get;
      protected set;
    }


    [DataField("Initials")]
    public string Initials {
      get;
      protected set;
    }


    [DataField("ContactEmail")]
    public string EMail {
      get;
      protected set;
    }


    [DataField("OrganizationId")]
    private int _organizationId {
      get; set;
    }


    public Organization Organization {
      get {
        return Organization.Parse(_organizationId);
      }
    }


    [DataField("ContactTags")]
    public string Tags {
      get;
      private set;
    }


    [DataField("ContactExtData")]
    public JsonObject ExtendedData {
      get;
      private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(FullName, ShortName, Initials, EMail);
      }
    }


    [DataField("ContactStatus", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      protected set;
    }


    string INamedEntity.Name {
      get {
        return !String.IsNullOrWhiteSpace(this.ShortName) ? this.ShortName : this.FullName;
      }
    }

    #endregion Properties

  } // class Contact

} // namespace Empiria.Contacts
