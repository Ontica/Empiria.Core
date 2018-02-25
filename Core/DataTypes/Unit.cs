/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : Unit                                             Pattern  : Storage Item Class                *
*                                                                                                            *
*  Summary   : Represents a unit of measure.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  public class Unit : GeneralObject {

    #region Constructors and parsers

    protected Unit() {
      // Required by Empiria Framework.
    }

    static public Unit Parse(int id) {
      return BaseObject.ParseId<Unit>(id);
    }

    static public Unit Parse(string unitNamedKey) {
      return BaseObject.ParseKey<Unit>(unitNamedKey);
    }

    static public Unit Empty {
      get { return BaseObject.ParseEmpty<Unit>(); }
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
      get { return BaseObject.ParseUnknown<Unit>(); }
    }

    #endregion Constructors and parsers

    #region Properties

    public new string Name {
      get {
        return base.Name;
      }
    }


    [DataField(ExtensionDataFieldName + ".PluralName")]
    public string PluralName {
      get;
      private set;
    }


    [DataField(GeneralObject.ExtensionDataFieldName  + ".Abbr", IsOptional = true)]
    public string Abbr {
      get;
      private set;
    }

    #endregion Properties

  } // class Unit

} // namespace Empiria.DataTypes
