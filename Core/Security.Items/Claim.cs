/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information holder                    *
*  Type     : Claim                                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Contains attributes that identifies a subject, like a userID or password.                      *
*             Subjects are users, applications, systems, services or computers.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Items {

  /// <summary>Contains attributes that identifies a subject, like a userID or password.
  /// Subjects are users, applications, systems, services or computers.</summary>
  internal class Claim : SecurityItem {

    #region Constructors and parsers

    internal Claim(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal new Claim Parse(int id) {
      return BaseObject.ParseId<Claim>(id);
    }


    static internal Claim TryParseWithKey(SecurityItemType claimType,
                                          IIdentifiable context,
                                          string securityKey) {

      return SecurityItemsDataReader.TryGetSubjectItemWithKey<Claim>(context,
                                                                     claimType,
                                                                     securityKey);
    }


    #endregion Constructors and parsers

    #region Properties

    internal T GetAttribute<T>(string attributeName) {
      Assertion.Require(attributeName, nameof(attributeName));

      return base.ExtensionData.Get<T>(attributeName);
    }


    internal T GetSubject<T>() where T : BaseObject {
      return BaseObject.ParseId<T>(base.SubjectId);
    }

    #endregion Properties

  }  // class Claim

}  // namespace Empiria.Security.Items
