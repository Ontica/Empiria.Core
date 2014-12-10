﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : GeneralList                                      Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a list type that holds BaseObject instances.                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Data;
using Empiria.Json;
using Empiria.Ontology;

namespace Empiria {

  /// <summary>Represents a list type that holds BaseObject instances.</summary>
  public class GeneralList : GeneralObject {

    #region Constructors and parsers

    private GeneralList() {
      // Required by Empiria Framework.
    }

    static public GeneralList Parse(int id) {
      return BaseObject.ParseId<GeneralList>(id);
    }

    static public GeneralList Parse(string listNamedKey) {
      return BaseObject.ParseKey<GeneralList>(listNamedKey);
    }

    static public GeneralList Empty {
      get { return BaseObject.ParseEmpty<GeneralList>(); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string UniqueCode {
      get { return base.NamedKey; }
    }

    #endregion Public properties

    #region Public methods

    public FixedList<T> GetItems<T>() {
      return base.ExtendedDataField.GetList<T>("ListItems").ToFixedList();
    }

    public FixedList<T> GetItems<T>(Comparison<T> sort) {
      var list = base.ExtendedDataField.GetList<T>("ListItems");
      list.Sort(sort);

      return list.ToFixedList();
    }

    #endregion Public methods

  } // class GeneralList

} // namespace Empiria
