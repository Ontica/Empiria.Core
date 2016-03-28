﻿/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : FixedList                                        Pattern  : Empiria List Class                *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a list of objects that cannot be added, removed or changed.                        *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Collections;

namespace Empiria {

  static public class FixedListExtensionMethods {

    /// <summary>Extends List<T> objects to return a FixedList<T> type</summary>
    static public FixedList<T> ToFixedList<T>(this List<T> list) {
      return new FixedList<T>(list);
    }

  }  // class FixedListExtensionMethods

  /// <summary>Represents a list of objects that cannot be added, removed or changed.</summary>
  public class FixedList<T> : BaseList<T> {

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

    static public FixedList<T> Empty {
      get {
        return new FixedList<T>();
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

    #endregion Public methods

  } // class FixedList

} // namespace Empiria
