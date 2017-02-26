/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Kernel.dll                *
*  Type      : Keyword                                          Pattern  : Standard Struct                   *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Sealed structure that represents a keyword.                                                   *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/

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
