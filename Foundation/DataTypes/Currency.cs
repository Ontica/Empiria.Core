/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : Currency                                         Pattern  : Storage Item Class                *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a currency data type.                                                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.DataTypes {

  public class Currency : GeneralObject {

    #region Fields

    static readonly int defaultCurrencyId = ConfigurationData.GetInteger("Default.Currency.ID");

    private const string thisTypeName = "ObjectType.GeneralObject.Currency";

    #endregion Fields

    #region Constructors and parsers

    public Currency()
      : base(thisTypeName) {

    }

    protected Currency(string typeName)
      : base(typeName) {
      // Empiria Object Type pattern classes always has this constructor. Don't delete
    }

    static public Currency Parse(int id) {
      return BaseObject.Parse<Currency>(thisTypeName, id);
    }

    static public Currency Default {
      get { return Currency.Parse(defaultCurrencyId); }
    }

    static public Currency Empty {
      get { return BaseObject.ParseEmpty<Currency>(thisTypeName); }
    }

    static public Currency NoLegible {
      get { return BaseObject.Parse<Currency>(thisTypeName, "NoLegible.Currency"); }
    }

    static public Currency Unknown {
      get { return BaseObject.ParseUnknown<Currency>(thisTypeName); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string Abbreviation {
      get { return base.NamedKey; }
    }

    #endregion Public properties

  } // class Currency

} // namespace Empiria.DataTypes