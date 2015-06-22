/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : Currency                                         Pattern  : Storage Item Class                *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a currency data type.                                                              *
*                                                                                                            *
********************************* Copyright (c) 1999-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.DataTypes {

  public class Currency : GeneralObject {

    #region Fields

    static readonly int defaultCurrencyId = ConfigurationData.GetInteger("Default.Currency.Id");

    #endregion Fields

    #region Constructors and parsers

    private Currency() {
      // Required by Empiria Framework.
    }

    static public Currency Parse(int id) {
      return BaseObject.ParseId<Currency>(id);
    }

    static public Currency Default {
      get { return Currency.Parse(defaultCurrencyId); }
    }

    static public Currency Empty {
      get { return BaseObject.ParseEmpty<Currency>(); }
    }

    static public Currency NoLegible {
      get { return BaseObject.ParseKey<Currency>("NoLegible.Currency"); }
    }

    static public Currency Unknown {
      get { return BaseObject.ParseUnknown<Currency>(); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string Abbreviation {
      get { return base.NamedKey; }
    }

    #endregion Public properties

  } // class Currency

} // namespace Empiria.DataTypes
