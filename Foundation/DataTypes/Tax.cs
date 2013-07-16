/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : Tax                                              Pattern  : Storage Item Class                *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a tax.                                                                             *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

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