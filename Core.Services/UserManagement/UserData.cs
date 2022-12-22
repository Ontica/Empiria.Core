/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : User Management                            Component : Data Access Layer                       *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Data Services                           *
*  Type     : UserManagementData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data read and write methods for user management services.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.StateEnums;

namespace Empiria.Services.UserManagement {


  internal class UserData : UserDescriptorDto {

    [DataField("ContactId")]
    internal int Id {
      get; private set;
    }


    [DataField("ContactTypeId")]
    internal int TypeId {
      get; private set;
    }


    [DataField("ContactUID")]
    public string UID {
      get;
      private set;
    }


    [DataField("ContactFullName")]
    public string FullName {
      get;
      private set;
    }


    [DataField("NickName")]
    public string NickName {
      get;
      private set;
    }


    public string BusinessID {
      get {
        return this.Id.ToString("000000000");
      }
    }


    public string Workplace {
      get {
        return "Subdirección de Registro Contable";
      }
    }


    [DataField("UserName")]
    public string UserID {
      get;
      private set;
    }


    [DataField("ContactEMail")]
    public string EMail {
      get;
      private set;
    }


    public DateTime LastAccess {
      get {
        return DateTime.Today.AddDays(EmpiriaMath.GetRandom(-20, 0));
      }
    }


    public bool IsSystem {
      get {
        return this.TypeId == 103;
      }
    }


    [DataField("ContactStatus", Default = EntityStatus.Suspended)]
    public EntityStatus Status {
      get;
      private set;
    }


  }  // class UserData

} // namespace Empiria.Services.UserManagement

