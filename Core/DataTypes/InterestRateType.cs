/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : InterestRateType                                 Pattern  : Storage Item Class                *
*                                                                                                            *
*  Summary   : Represents an interest rate type.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  public class InterestRateType : GeneralObject {

    #region Constructors and parsers

    private InterestRateType() {
      // Required by Empiria Framework.
    }

    static public InterestRateType Parse(int id) {
      return BaseObject.ParseId<InterestRateType>(id);
    }

    static public InterestRateType Empty {
      get { return BaseObject.ParseEmpty<InterestRateType>(); }
    }

    static public InterestRateType Unknown {
      get { return BaseObject.ParseUnknown<InterestRateType>(); }
    }

    #endregion Constructors and parsers

  } // class InterestRateType

} // namespace Empiria.DataTypes
