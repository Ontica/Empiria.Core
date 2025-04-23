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
using System.Linq;

using Empiria.Collections;

namespace Empiria {

  /// <summary>Static class that contains FixedList extension methods.</summary>
  static public class FixedListExtensionMethods {

    ///// <summary>Extends generic List[T] objects to return a FixedList[T] type.</summary>
    //static public FixedList<T> ToFixedList<T>(this List<T> list) {
    //  return new FixedList<T>(list);
    //}

    /// <summary>Extends generic IEnumerable<T> objects to return a FixedList[T] type.</summary>
    static public FixedList<T> ToFixedList<T>(this IEnumerable<T> list) {
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


    public FixedList<T> Intersect(FixedList<T> second) {
      var intersect = this.Intersect<T>(second);

      return new FixedList<T>(intersect);
    }


    public new FixedList<T> Reverse() {
      base.Reverse();

      return this;
    }


    public new bool Remove(T item) {
      Assertion.Require(item, nameof(item));

      return base.Remove(item);
    }


    public FixedList<T> Remove(IEnumerable<T> items) {
      var copy = new FixedList<T>(this);

      foreach (var item in items) {
        copy.Remove(item);
      }

      return copy;
    }


    public bool SameItems(IEnumerable<T> second) {
      var copy = second.ToFixedList();

      if (this.Count != copy.Count) {
        return false;
      }

      var intersect = this.Intersect(copy);

      return (intersect.Count == copy.Count);
    }


    public new IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector) {
      return base.Select<TResult>(selector);
    }


    public FixedList<TResult> SelectDistinct<TResult>(Func<T, TResult> selector) {
      return base.Select<TResult>(selector)
                 .Distinct()
                 .ToFixedList();
    }


    public new FixedList<T> Sort(Comparison<T> comparison) {
      base.Sort(comparison);

      return this;
    }


    public FixedList<T> Sublist(int index) {
      Assertion.Require(index < this.Count, $"index out of bounds: {index}");

      var sublist = new List<T>(this);

      if (index > 0) {
        sublist.RemoveRange(0, index);
      }

      return sublist.ToFixedList();
    }


    public T[] ToArray() {
      var array = new T[this.Count];

      base.CopyTo(array, 0);

      return array;
    }

    #endregion Public methods

  } // class FixedList

} // namespace Empiria
