/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Data Access Layer                       *
*  Assembly : Empiria.Core.dll                           Pattern   : Data service                            *
*  Type     : PartyDataService                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides parties data persistence services.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;
using Empiria.Ontology;

namespace Empiria.Parties.Data {

  /// <summary>Provides parties data persistence methods.</summary>
  static internal class PartyDataService {

    static internal void CleanParty(Party party) {
      if (party.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE PARTIES " +
                $"SET PARTY_NAME = '{EmpiriaString.Clean(party.Name).Replace("'", "''")}', " +
                $"PARTY_UID = '{Guid.NewGuid().ToString()}', " +
                $"PARTY_HISTORIC_ID = {party.Id}, " +
                $"PARTY_KEYWORDS = '{party.Keywords}', " +
                $"PARTY_POSTING_TIME = {DataCommonMethods.FormatSqlDbDate(new DateTime(2025, 01, 25))}, " +
                $"PARTY_START_DATE = {DataCommonMethods.FormatSqlDbDate(new DateTime(2025, 01, 25))}, " +
                $"PARTY_END_DATE = {DataCommonMethods.FormatSqlDbDate(new DateTime(2078, 12, 31))} " +
                $"WHERE PARTY_ID = {party.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

    static internal FixedList<T> GetPartiesInDate<T>(DateTime date) where T : Party {
      ObjectTypeInfo typeInfo = ObjectTypeInfo.Parse<T>();

      var sql = $"SELECT * FROM PARTIES " +
                $"WHERE PARTY_TYPE_ID = {typeInfo.Id} AND " +
                $"PARTY_START_DATE <= {DataCommonMethods.FormatSqlDbDate(date)} AND " +
                $"{DataCommonMethods.FormatSqlDbDate(date)} <= PARTY_END_DATE AND " +
                $"PARTY_STATUS <> 'X'";

      return DataReader.GetFixedList<T>(DataOperation.Parse(sql));
    }


    static internal void CloseHistoricParty(IHistoricObject historyOf) {
      var sql = $"UPDATE PARTIES SET " +
                $"PARTY_END_DATE = {DataCommonMethods.FormatSqlDbDate(DateTime.Today)}" +
                $"WHERE PARTY_HISTORIC_ID = {historyOf.HistoricId} " +
                $"AND PARTY_END_DATE = {DataCommonMethods.FormatSqlDbDate(ExecutionServer.DateMaxValue)}";

      DataWriter.Execute(DataOperation.Parse(sql));
    }


    static internal FixedList<Party> SearchPartiesInRole(string roleName, string keywords = "") {
      Assertion.Require(roleName, nameof(roleName));

      var sql = "SELECT * FROM PARTIES " +
                $"WHERE PARTY_ROLES LIKE '%{roleName}%' AND " +
                $"PARTY_STATUS <> 'X'";

      if (!string.IsNullOrEmpty(keywords)) {
        sql += " AND " + SearchExpression.ParseAndLikeKeywords("PARTY_KEYWORDS", keywords);
      }
      sql += " ORDER BY PARTY_NAME";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Party>(op);
    }



    static internal FixedList<PartyRole> GetPartySecurityRoles(Party party) {
      Assertion.Require(party, nameof(party));

      var sql = "SELECT DISTINCT SECURITYITEMS.TARGETID " +
                "FROM SECURITYITEMS " +
                $"WHERE SECURITYITEMS.SUBJECTID = {party.Id} AND " +
                $"SECURITYITEMTYPEID = 140 AND SECURITYITEMSTATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      var rolesIds = DataReader.GetFieldValues<int>(op);

      return PartyRole.GetList()
                      .FindAll(x => rolesIds.Contains(x.Id));
    }


    static internal FixedList<Party> SearchPartyRoleSecurityPlayers(PartyRole partyRole, string keywords) {
      Assertion.Require(partyRole, nameof(partyRole));
      keywords = keywords ?? string.Empty;

      string keywordsFilter = SearchExpression.ParseAndLikeKeywords("PARTY_KEYWORDS", keywords);

      var sql = "SELECT PARTIES.* FROM PARTIES " +
                "INNER JOIN SECURITYITEMS " +
                "ON PARTIES.PARTY_CONTACT_ID = SECURITYITEMS.SUBJECTID " +
                $"WHERE SECURITYITEMS.TARGETID = {partyRole.Id} AND " +
                "SECURITYITEMTYPEID = 140 AND {{KEYWORDS.FILTER}}" +
                $"PARTY_STATUS <> 'X' AND SECURITYITEMSTATUS <> 'X' " +
                "ORDER BY PARTY_NAME";

      if (keywordsFilter.Length != 0) {
        sql = sql.Replace("{{KEYWORDS.FILTER}}", $"{keywordsFilter} AND ");
      } else {
        sql = sql.Replace("{{KEYWORDS.FILTER}}", string.Empty);
      }

      return DataReader.GetFixedList<Party>(DataOperation.Parse(sql));
    }


    static internal void WriteParty(Party o) {
      var op = DataOperation.Parse("write_party",
                 o.Id, o.UID, o.PartyType.Id, o.Code, o.Name,
                 EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Roles),
                 EmpiriaString.Tagging(o.Tags), o.ExtendedData.ToString(), o.Keywords,
                 o.HistoricId, o.StartDate, o.EndDate, o.ParentId, o.PostedById, o.PostingTime,
                 (char) o.Status, o.Contact.Id);

      DataWriter.Execute(op);
    }


    static internal void WritePartyRelation(PartyRelation o) {
      var op = DataOperation.Parse("write_party_relation",
                 o.Id, o.UID, o.PartyRelationType.Id, o.Category.Id, o.Role.Id,
                 o.Commissioner.Id, o.Responsible.Id, o.Code, o.Description,
                 EmpiriaString.Tagging(o.Identificators), EmpiriaString.Tagging(o.Tags),
                 o.ExtData.ToString(), o.Keywords, o.StartDate, o.EndDate,
                 o.PostingTime, o.PostedBy.Id, (char) o.Status);

      DataWriter.Execute(op);
    }

  } // class PartyDataService

} // namespace Empiria.Parties.Data
