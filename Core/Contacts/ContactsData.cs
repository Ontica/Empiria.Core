/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts.Data                            License  : Please read LICENSE.txt file      *
*  Type      : ObjectReader                                     Pattern  : Data Services Static Class        *
*                                                                                                            *
*  Summary   : Provides data read methods for contact-related entities.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Contacts {

  static internal class ContactsData {

    #region Internal methods

    static internal FixedList<Contact> GetContacts(string keywords, string sort = "") {
      string filter =  String.Empty;
      if (keywords.Length != 0) {
        filter = SearchExpression.ParseAndLikeWithNoiseWords("ContactKeywords", keywords).ToString();
      }

      string sql = "SELECT * FROM Contacts" + GeneralDataOperations.GetFilterSortSqlString(filter, sort);

      return DataReader.GetList<Contact>(DataOperation.Parse(sql),
                                        (x) => BaseObject.ParseList<Contact>(x)).ToFixedList();
    }

    static internal int WriteContact(Contact o) {
      throw new NotImplementedException();
    }

    #endregion Internal methods

  } // class ContactsData

} // namespace Empiria.Contacts.Data
