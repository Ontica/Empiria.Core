/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : GeneralList                                      Pattern  : Storage Item                      *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a list type that holds BaseObject instances.                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

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
