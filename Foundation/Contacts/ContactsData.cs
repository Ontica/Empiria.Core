/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Contacts Management               *
*  Namespace : Empiria.Contacts.Data                            Assembly : Empiria.dll                       *
*  Type      : ObjectReader                                     Pattern  : Data Services Static Class        *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Provides data read methods for Empiria Foundation Ontology objects.                           *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

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

    static internal int WriteUser(User user) {
      throw new NotImplementedException();
    }

    #endregion Internal methods

  } // class ContactsData

} // namespace Empiria.Contacts.Data
