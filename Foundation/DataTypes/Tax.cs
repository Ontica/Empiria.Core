/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : Tax                                              Pattern  : Storage Item Class                *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a tax.                                                                             *
*                                                                                                            *
********************************* Copyright (c) 1999-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
