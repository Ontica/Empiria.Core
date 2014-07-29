/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : DiscountType                                     Pattern  : Storage Item Class                *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a discount type.                                                                   *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.DataTypes {

  public class DiscountType : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.DiscountType";

    #endregion Fields

    #region Constructors and parsers

    public DiscountType() : base(thisTypeName) {

    }

    protected DiscountType(string typeName) : base(typeName) {
      // Empiria Object Type pattern classes always has this constructor. Don't delete.
    }

    static public DiscountType Parse(int id) {
      return BaseObject.Parse<DiscountType>(thisTypeName, id);
    }

    static public DiscountType Empty {
      get {
        return BaseObject.ParseEmpty<DiscountType>(thisTypeName);
      }
    }

    static public DiscountType Unknown {
      get {
        return BaseObject.ParseUnknown<DiscountType>(thisTypeName);
      }
    }

    #endregion Constructors and parsers

  } // class DiscountType

} // namespace Empiria.DataTypes
