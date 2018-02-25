/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : DiscountType                                     Pattern  : Storage Item Class                *
*                                                                                                            *
*  Summary   : Represents a discount type.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  public class DiscountType : GeneralObject {

    #region Constructors and parsers

    private DiscountType() {
      // Required by Empiria Framework.
    }

    static public DiscountType Parse(int id) {
      return BaseObject.ParseId<DiscountType>(id);
    }

    static public DiscountType Empty {
      get {
        return BaseObject.ParseEmpty<DiscountType>();
      }
    }

    static public DiscountType Unknown {
      get {
        return BaseObject.ParseUnknown<DiscountType>();
      }
    }

    #endregion Constructors and parsers

  } // class DiscountType

} // namespace Empiria.DataTypes
