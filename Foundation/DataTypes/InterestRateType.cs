/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : InterestRateType                                 Pattern  : Storage Item Class                *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents an interest rate type.                                                             *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
