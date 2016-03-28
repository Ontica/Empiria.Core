﻿/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : DiscountType                                     Pattern  : Storage Item Class                *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a discount type.                                                                   *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
