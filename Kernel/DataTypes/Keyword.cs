/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Kernel.dll                *
*  Type      : Keyword                                          Pattern  : Standard Struct                   *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Sealed structure that represents a keyword.                                                   *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/

namespace Empiria.DataTypes {

  public struct Keyword {

    private string keywordValue;

    public Keyword(string value) {
      keywordValue = value;
    }

    public string Value {
      get { return keywordValue; }
      set {
        keywordValue = value;
      }
    }

  } // struct Keyword

} // namespace Empiria.DataTypes
