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
