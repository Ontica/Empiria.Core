/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     License  : Please read LICENSE.txt file      *
*  Type      : DataFilter                                       Pattern  : Static Class With Objects Cache   *
*                                                                                                            *
*  Summary   : Represents a data source formed by the source or connection string and the data technology.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Data {

  public class SqlFilter : IFilter {

    #region Constructors and parsers

    private SqlFilter(string value) {
      this.Value = value;
    }

    static public SqlFilter Parse(string criteria) {
      Assertion.Require(criteria, "criteria");

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
