/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts.Data                            Assembly : Empiria.dll                       *
*  Type      : ObjectReader                                     Pattern  : Data Services Static Class        *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Provides data read methods for Empiria Foundation Ontology objects.                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Security;

namespace Empiria.Contacts {

  static internal class ContactsData {

    #region Internal methods

    static internal T GetContactAttribute<T>(Contact contact, string attributeName) {
      return GeneralDataOperations.GetEntityField<T>("EOSContacts", attributeName, "ContactId", contact.Id);
    }

    static internal int GetContactIdWithUserName(string userName) {
      return GeneralDataOperations.GetEntityId("EOSContacts", "ContactId", "UserName", userName);
    }

    static internal FixedList<Contact> GetContacts(string keywords, string sort = "") {
      string filter = keywords.Length != 0 ? 
                      SearchExpression.ParseAndLikeWithNoiseWords("ContactKeywords", keywords).ToString() :
                      String.Empty;
      string sql = "SELECT * FROM EOSContacts";
      sql += GeneralDataOperations.GetFilterSortSqlString(filter, sort);

      return DataReader.GetList<Contact>(DataOperation.Parse(sql), 
                                        (x) => BaseObject.ParseList<Contact>(x)).ToFixedList();
    }

    static internal int WriteContact(Contact o) {
      throw new NotImplementedException();
    }

    static internal int WriteContactAttribute(Contact contact, string attributeName, object atributeValue) {
      string sql = String.Empty;

      if (atributeValue is System.String) {
        sql = "UPDATE EOSContacts SET " + attributeName + " = '" + (string) atributeValue + "'";
      } else {
        sql = "UPDATE EOSContacts SET " + attributeName + " = " + Convert.ToString(atributeValue);
      }
      sql += " WHERE ContactId = " + contact.Id.ToString();

      return DataWriter.Execute(DataOperation.Parse(sql));
    }

    static internal int WriteTempUserSettings(int userId, int organizationId, int externalObjectId) {
      return DataWriter.Execute(DataOperation.Parse("writeTempUserSettings", 
                                userId, organizationId, externalObjectId));
    }

    static internal int WriteUser(EmpiriaUser user) {
      throw new NotImplementedException();
    }

    #endregion Internal methods

  } // class ContactsData

} // namespace Empiria.Contacts.Data
