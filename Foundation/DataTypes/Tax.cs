/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : Tax                                              Pattern  : Storage Item Class                *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a tax.                                                                             *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.DataTypes {

  public class Tax : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.Tax";

    #endregion Fields

    #region Constructors and parsers

    public Tax()
      : base(thisTypeName) {

    }

    protected Tax(string typeName)
      : base(typeName) {
      // Empiria Object Type pattern classes always has this constructor. Don't delete
    }

    static public Tax Parse(int id) {
      return BaseObject.Parse<Tax>(thisTypeName, id);
    }

    #endregion Constructors and parsers

  } // class Tax

} // namespace Empiria.DataTypes
