/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataFilter                                       Pattern  : Static Class With Objects Cache   *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a data source formed by the source or connection string and the data technology.   *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Data {

  public class SqlFilter : IFilter {

    #region Constructors and parsers

    private SqlFilter(string value) {
      this.Value = value;
    }

    static public SqlFilter Parse(string criteria) {
      Assertion.AssertObject(criteria, "criteria");
      
      return new SqlFilter(criteria);
    }

    #endregion Constructors and parsers

    #region Properties

    public string Value {
      get;
      private set;
    }

    #endregion Properties

  } // class DataFilter

} //namespace Empiria.Data
