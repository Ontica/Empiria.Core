/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : FixedList                                        Pattern  : Empiria List Class                *
*                                                                                                            *
*  Summary   : Represents a fixed list of objects whose members cannot be added, removed or changed.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Collections;

namespace Empiria {

  /// <summary>Static class that contains FixedList extension methods.</summary>
  static public class FixedListExtensionMethods {

    /// <summary>Extends generic List[T] objects to return a FixedList[T] type.</summary>
    static public FixedList<T> ToFixedList<T>(this List<T> list) {
      return new FixedList<T>(list);
    }

  }  // class FixedListExtensionMethods


  /// <summary>Represents a fixed list of objects whose members cannot be added, removed or changed.</summary>
  public class FixedList<T> : BaseList<T> {

    #region Constructors and parsers

    public FixedList() {
      //no-op
    }


    public FixedList(int capacity) : base(capacity) {
      // no-op
    }


    public FixedList(IEnumerable<T> list) : base() {
      foreach (var item in list) {
        this.Add(item);
      }
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


    public new bool Exists(Predicate<T> match) {
      return base.Exists(match);
    }


    public new T Find(Predicate<T> match) {
      return base.Find(match);
    }


    public new FixedList<T> FindAll(Predicate<T> match) {
      return base.FindAll(match).ToFixedList();
    }


    public new T FindLast(Predicate<T> match) {
      return base.FindLast(match);
    }


    public new void Reverse() {
      base.Reverse();
    }


    public FixedList<T> Remove(IEnumerable<T> items) {
      var copy = new FixedList<T>(this);

      foreach (var item in items) {
        copy.Remove(item);
      }

      return copy;
    }


    public new IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector) {
      return base.Select<TResult>(selector);
    }


    public new void Sort(Comparison<T> comparison) {
      base.Sort(comparison);
    }


    #endregion Public methods

  } // class FixedList

} // namespace Empiria
