/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Data access layer                     *
*  Assembly : Empiria.Core.dll                             Pattern   : Read only data service                *
*  Type     : SecurityItemsDataReader                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Reads security items data.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Security.Items {

  /// <summary>Reads security items data.</summary>
  static internal class SecurityItemsDataReader {

    static internal FixedList<T> GetContextItems<T>(IIdentifiable context,
                                                    SecurityItemType itemType) where T : SecurityItem {
      Assertion.Require(context, "context");
      Assertion.Require(itemType, "itemType");

      string sql = $"SELECT * FROM SecurityItems " +
                   $"WHERE ContextId = {context.Id} AND " +
                   $"SecurityItemTypeId = {itemType.Id} AND " +
                   $"SecurityItemStatus = 'A'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<T>(op);
    }


    static internal FixedList<T> GetIdentityTargetItems<T>(IIdentifiable context,
                                                           EmpiriaIdentity identity,
                                                           SecurityItemType itemType) where T : SecurityItem {
      Assertion.Require(context, "context");
      Assertion.Require(identity, "identity");
      Assertion.Require(itemType, "itemType");

      string sql = $"SELECT TargetId FROM SecurityItems " +
                   $"WHERE ContextId = {context.Id} AND " +
                   $"SubjectId = {identity.User.Id} AND " +
                   $"SecurityItemTypeId = {itemType.Id} AND " +
                   $"SecurityItemStatus = 'A'";

      var op = DataOperation.Parse(sql);

      var targets = DataReader.GetFieldValues<int>(op);

      return targets.ToFixedList()
                    .Select(targetId => SecurityItem.Parse<T>(targetId))
                    .ToFixedList();
    }

  }  // class SecurityItemsDataReader

}  // namespace Empiria.Security.Items
