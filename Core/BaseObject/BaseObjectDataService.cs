/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base Object                                  Component : Data Access Layer                     *
*  Assembly : Empiria.Core.dll                             Pattern   : Data service                          *
*  Type     : BaseObjectDataService                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides persistence methods for base objects.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria {

  /// <summary>Provides persistence methods for base objects.</summary>
  static internal class BaseObjectDataService {

    static internal FixedList<T> GetBaseObjectLinks<T>(BaseObjectLinkType linkType) where T : BaseObjectLink {
      var sql = "SELECT * FROM OBJECT_LINKS " +
               $"WHERE OBJECT_LINK_TYPE_ID = {linkType.Id} AND " +
               $"OBJECT_LINK_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<T>(op);
    }


    static internal FixedList<T> GetBaseObjectLinksWithBaseObject<T>(BaseObjectLinkType linkType,
                                                                     BaseObject baseObject) where T : BaseObjectLink {
      var sql = "SELECT * FROM OBJECT_LINKS " +
               $"WHERE OBJECT_LINK_TYPE_ID = {linkType.Id} AND " +
               $"OBJECT_LINK_BASE_OBJECT_ID = {baseObject.Id} AND " +
               $"OBJECT_LINK_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<T>(op);
    }


    static internal FixedList<T> GetBaseObjectLinksWithLinkedObject<T>(BaseObjectLinkType linkType,
                                                                       BaseObject linkedObject) where T : BaseObjectLink {
      var sql = "SELECT * FROM OBJECT_LINKS " +
               $"WHERE OBJECT_LINK_TYPE_ID = {linkType.Id} AND " +
               $"OBJECT_LINK_LINKED_OBJECT_ID = {linkedObject.Id} AND " +
               $"OBJECT_LINK_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<T>(op);
    }

    static internal FixedList<T> GetBaseObjectsFor<T>(BaseObjectLinkType linkType,
                                                      BaseObject linkedObject) where T : BaseObject {
      string baseObjectDataSource = linkType.BaseObjectType.DataSource;
      string baseObjectIdField = linkType.BaseObjectType.IdFieldName;

      string filter = $"OBJECT_LINK_TYPE_ID = {linkType.Id} AND " +
                      $"OBJECT_LINK_LINKED_OBJECT_ID = {linkedObject.Id} AND " +
                      $"OBJECT_LINK_STATUS <> 'X'";

      var op = DataOperation.Parse("@qryEntitiesJoinedFiltered", baseObjectDataSource, "OBJECT_LINKS",
                                   baseObjectIdField, "OBJECT_LINK_BASE_OBJECT_ID", filter);

      return DataReader.GetFixedList<T>(op);
    }


    static internal FixedList<T> GetLinkedObjectsFor<T>(BaseObjectLinkType linkType,
                                                        BaseObject baseObject) where T : BaseObject {

      string linkedObjectDataSource = linkType.LinkedObjectType.DataSource;
      string linkedObjectIdField = linkType.LinkedObjectType.IdFieldName;

      string filter = $"OBJECT_LINK_TYPE_ID = {linkType.Id} AND " +
                      $"OBJECT_LINK_BASE_OBJECT_ID = {baseObject.Id} AND " +
                      $"OBJECT_LINK_STATUS <> 'X'";

      var op = DataOperation.Parse("@qryEntitiesJoinedFiltered", linkedObjectDataSource, "OBJECT_LINKS",
                                   linkedObjectIdField, "OBJECT_LINK_LINKED_OBJECT_ID", filter);

      return DataReader.GetFixedList<T>(op);
    }


    static internal void WriteBaseObjectLink(BaseObjectLink o) {
      var op = DataOperation.Parse("write_Object_Link",
                 o.Id, o.UID, o.BaseObjectLinkType.Id,
                 o.BaseObject.Id, o.LinkedObject.Id, o.LinkedObjectRole,
                 o.Description, o.Identificators, o.Tags, o.ExtensionData.ToString(),
                 o.Keywords, o.StartDate, o.EndDate,
                 o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  } // class BaseObjectDataService

} // namespace Empiria
