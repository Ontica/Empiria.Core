/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataQuery                                        Pattern  : Standard Class                    *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a set of data query options that serves to implement sorting, filtering,           *
*              and field selecting operations.                                                               *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Json;

namespace Empiria.Data {

  /// <summary>Represents a set of data query options that serves to implement sorting, filtering
  /// and field selecting operations.</summary>
  public class DataQuery {

    #region Constructors and parsers

    public DataQuery(string select = "", string filter = "", string orderBy = "", int top = -1) {
      this.Select = select;
      this.Filter = filter;
      this.OrderBy = orderBy;
      //this.Top = top;
    }

    static public DataQuery Parse(JsonObject json) {
      var dataQuery = new DataQuery();

      dataQuery.Select = json.Get<string>("Select", String.Empty);
      dataQuery.Filter = json.Get<string>("Filter", String.Empty);
      dataQuery.OrderBy = json.Get<string>("OrderBy", String.Empty);
      //dataQuery.Top = json.Get<int>("Top", -1);

      return dataQuery;
    }

    static private DataQuery _emptyInstance = new DataQuery();
    static public DataQuery Empty {
      get {
        return _emptyInstance;
      }
    }

    #endregion Constructors and parsers

    #region Fields

    /// <summary>Selects which fields or properties to include in the result.</summary>
    public string Select {
      get;
      private set;
    }

    /// <summary>Filters the results based on a Boolean condition.</summary>
    public string Filter {
      get;
      private set;
    }

    /// <summary>Sorts the results by.</summary>
    public string OrderBy {
      get;
      private set;
    }

    ///// <summary>Returns only the first n rows.</summary>
    //public int Top {
    //  get;
    //  private set;
    //}

    #endregion Fields

    #region Public methods

    internal DataTable ApplyTo(DataTable dataTable) {
      dataTable = this.ApplyFilterAndOrderBy(dataTable);

      return this.SelectColumns(dataTable);
    }

    public override string ToString() {
      var json = new JsonObject();
      json.AddIfValue(new JsonItem("Select", this.Select));
      json.AddIfValue(new JsonItem("Filter", this.Filter));
      json.AddIfValue(new JsonItem("OrderBy", this.OrderBy));

      return json.ToString();
    }

    #endregion Public methods

    #region Private methods

    private DataTable ApplyFilterAndOrderBy(DataTable dataTable) {
      if (String.IsNullOrWhiteSpace(this.Filter) && String.IsNullOrWhiteSpace(this.OrderBy)) {
        return dataTable;
      }
      var view = new DataView(dataTable, this.Filter, this.OrderBy, DataViewRowState.CurrentRows);

      return view.ToTable();
    }

    private List<string> GetSelectFieldNames() {
      if (String.IsNullOrWhiteSpace(this.Select)) {
        return new List<string>();
      }
      string[] array = this.Select.Replace(" ", String.Empty).Split(',');

      return new List<string>(array);
    }

    private DataTable SelectColumns(DataTable dataTable) {
      if (String.IsNullOrWhiteSpace(this.Select)) {
        return dataTable;
      }

      List<string> selectFields = this.GetSelectFieldNames();
      List<string> toRemoveFields = new List<string>();
      foreach (DataColumn column in dataTable.Columns) {
        if (!selectFields.Contains(column.ColumnName)) {
          toRemoveFields.Add(column.ColumnName);
        }
      }
      foreach (string field in toRemoveFields) {
        dataTable.Columns.Remove(field);
      }
      return dataTable;
    }

    #endregion Private methods

  }  // class DataQuery

} // namespace Empiria.Data
