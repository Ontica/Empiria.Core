﻿/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : Unit                                             Pattern  : Storage Item Class                *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Represents a unit of measure.                                                                 *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/

namespace Empiria.DataTypes {

  public class Unit : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.Unit";

    #endregion Fields

    #region Constructors and parsers

    public Unit()
      : base(thisTypeName) {

    }

    protected Unit(string typeName)
      : base(typeName) {

    }

    static public Unit Parse(int id) {
      return BaseObject.Parse<Unit>(thisTypeName, id);
    }

    static public Unit Parse(string unitNamedKey) {
      return BaseObject.Parse<Unit>(thisTypeName, unitNamedKey);
    }

    static public Unit Empty {
      get { return BaseObject.ParseEmpty<Unit>(thisTypeName); }
    }

    static public Unit Percentage {
      get { return Unit.Parse("Unit.Percentage"); }
    }

    static public Unit SquareMeters {
      get { return Unit.Parse("AreaUnit.SquareMeters"); }
    }

    static public Unit FullUnit {
      get { return Unit.Parse("Unit.Full"); }
    }

    static public Unit UndividedUnit {
      get { return Unit.Parse("Unit.Undivided"); }
    }

    static public Unit Unknown {
      get { return BaseObject.ParseUnknown<Unit>(thisTypeName); }
    }

    #endregion Constructors and parsers

    #region Properties

    public string Symbol {
      get { return base.Description; }
    }

    //public new string PluralName {
    //  get { return base.PluralName; }
    //}

    #endregion Properties

  } // class Unit

} // namespace Empiria.DataTypes