/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : FixedList                                        Pattern  : Empiria List Class                *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a list of IStorable objects that cannot be added, removed or changed.              *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Collections;
using Empiria.Data;

namespace Empiria {

  static public class ListExtensionMethods {

    /// <summary>Extends List<T> objects to return a FixedList<T> type</summary>
    static public FixedList<T> ToFixedList<T>(this List<T> list) where T : IStorable {
      return new FixedList<T>(list);
    }

  }

  /// <summary>Represents a list of IStorable objects that cannot be added, removed or changed.</summary>
  public class FixedList<T> : EmpiriaList<T> where T : IStorable {

    #region Constructors and parsers

    public FixedList() {
      //no-op
    }

    public FixedList(int capacity) : base(capacity) {
      // no-op
    }

    public FixedList(List<T> list) : base(list) {

    }

    public FixedList(Func<DataRow, T> parser, DataView view) : this(view.Count) {
      foreach (DataRowView row in view) {
        this.Add(parser.Invoke(row.Row));
      }
    }

    public FixedList(Func<DataRow, T> parser, DataTable table) {
      foreach (DataRow row in table.Rows) {
        this.Add(parser.Invoke(row));
      }
    }

    #endregion Constructors and parsers

    #region Public methods

    public new bool Contains(T item) {
      return base.Contains(item);
    }

    public bool Contains(Predicate<T> match) {
      T result = base.Find(match);

      return (result != null);
    }

    public new T Find(Predicate<T> match) {
      return base.Find(match);
    }

    public new List<T> FindAll(Predicate<T> match) {
      return base.FindAll(match);
    }

    public new T FindLast(Predicate<T> match) {
      return base.FindLast(match);
    }

    public new void Sort(Comparison<T> comparison) {
      base.Sort(comparison);
    }

    public string ToJson() {
      throw new NotImplementedException();
    }

    #endregion Public methods

  } // class FixedList

} // namespace Empiria
