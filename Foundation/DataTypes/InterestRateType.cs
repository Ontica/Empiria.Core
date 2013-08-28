﻿/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : InterestRateType                                 Pattern  : Storage Item Class                *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents an interest rate type.                                                             *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria.DataTypes {

  public class InterestRateType : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.InterestRateType";

    #endregion Fields

    #region Constructors and parsers

    public InterestRateType()
      : base(thisTypeName) {

    }

    protected InterestRateType(string typeName)
      : base(typeName) {
      // Empiria Object Type pattern classes always has this constructor. Don't delete
    }

    static public InterestRateType Parse(int id) {
      return BaseObject.Parse<InterestRateType>(thisTypeName, id);
    }

    static public InterestRateType Empty {
      get { return BaseObject.ParseEmpty<InterestRateType>(thisTypeName); }
    }

    static public InterestRateType Unknown {
      get { return BaseObject.ParseUnknown<InterestRateType>(thisTypeName); }
    }

    #endregion Constructors and parsers

  } // class InterestRateType

} // namespace Empiria.DataTypes