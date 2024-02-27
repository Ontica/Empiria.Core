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


    [DataField(ExtensionDataFieldName + ".Abbr", IsOptional = true)]
    public string Abbr {
      get;
      private set;
    }

    [DataField(ExtensionDataFieldName + ".IsIndivisible", IsOptional = true)]
    public bool IsIndivisible {
      get;
      private set;
    }

    #endregion Properties

  } // class Unit

} // namespace Empiria.DataTypes
