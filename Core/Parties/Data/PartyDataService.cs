/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Data Access Layer                     *
*  Assembly : Empiria.Core.dll                             Pattern   : Data service                          *
*  Type     : PartyDataService                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides parties persistence methods.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;
using Empiria.Ontology;

namespace Empiria.Parties {

  /// <summary>Provides parties persistence methods.</summary>
  static internal class PartyDataService {

    internal static FixedList<T> GetPartiesInDate<T>(DateTime date) where T : Party {
      ObjectTypeInfo typeInfo = ObjectTypeInfo.Parse<T>();

      var sql = $"SELECT * FROM PARTIES " +
                $"WHERE PARTY_TYPE_ID = {typeInfo.Id} AND " +
                $"PARTY_START_DATE <= {DataCommonMethods.FormatSqlDbDate(date)} AND " +
                $"{DataCommonMethods.FormatSqlDbDate(date)} <= PARTY_END_DATE AND " +
                $"PARTY_STATUS <> 'X'";

      return DataReader.GetFixedList<T>(DataOperation.Parse(sql));
    }


    internal static void CloseHistoricParty(IHistoricObject historyOf) {
      var sql = $"UPDATE PARTIES SET " +
                $"PARTY_END_DATE = {DataCommonMethods.FormatSqlDbDate(DateTime.Today)}" +
                $"WHERE PARTY_HISTORIC_ID = {historyOf.HistoricId} " +
                $"AND PARTY_END_DATE = {DataCommonMethods.FormatSqlDbDate(ExecutionServer.DateMaxValue)}";

      DataWriter.Execute(DataOperation.Parse(sql));
    }

    internal static void CloseHistoricPartyRelation(IHistoricObject historyOf) {
      var sql = $"UPDATE PARTIES_RELATIONS SET " +
                $"PTY_RELATION_END_DATE = {DataCommonMethods.FormatSqlDbDate(DateTime.Today)}" +
                $"WHERE PTY_RELATION_HISTORIC_ID = {historyOf.HistoricId} " +
                $"AND PTY_RELATION_END_DATE = {DataCommonMethods.FormatSqlDbDate(ExecutionServer.DateMaxValue)}";

      DataWriter.Execute(DataOperation.Parse(sql));
    }


    static internal void WriteParty(Party o) {
      var op = DataOperation.Parse("writeParty",
                 o.Id, o.UID, o.PartyType.Id,
                 o.Name, o.ExtendedData.ToString(), o.Keywords,
                 o.HistoricId, o.StartDate, o.EndDate,
                 o.PostedById, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    internal static void WritePartyRelation(PartyRelation o) {
      var op = DataOperation.Parse("writePartyRelation",
                 o.Id, o.UID, o.PartyRelationType.Id,
                 o.Role.Id, o.Commissioner.Id, o.Responsible.Id,
                 o.ExtendedData.ToString(), o.Keywords,
                 o.HistoricId, o.StartDate, o.EndDate,
                 o.PostedById, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }
  } // class PartyDataService


} // namespace Empiria.Parties
