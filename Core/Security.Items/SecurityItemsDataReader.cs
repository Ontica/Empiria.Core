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

    static internal FixedList<T> GetSubjectSecurityItems<T>(IIdentifiable subject,
                                                            SecurityItemType itemType) where T : SecurityItem {
      Assertion.Require(subject, "subject");
      Assertion.Require(itemType, "itemType");

      string sql = $"SELECT * FROM SecurityItems " +
                   $"WHERE SubjectId = {subject.Id} AND " +
                   $"SecurityItemTypeId = {itemType.Id} AND " +
                   $"SecurityItemStatus = 'A'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<T>(op);
    }

  }  // class SecurityItemsDataReader

}  // namespace Empiria.Security.Items
