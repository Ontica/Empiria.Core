/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : Tax                                              Pattern  : Storage Item Class                *
*                                                                                                            *
*  Summary   : Represents a tax.                                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  public class Tax : GeneralObject {

    #region Constructors and parsers

    private Tax() {
      // Required by Empiria Framework.
    }

    static public Tax Parse(int id) {
      return BaseObject.ParseId<Tax>(id);
    }

    #endregion Constructors and parsers

  } // class Tax

} // namespace Empiria.DataTypes
