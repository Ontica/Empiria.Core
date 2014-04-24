/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Kernel.dll                *
*  Type      : Keyword                                          Pattern  : Standard Struct                   *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Sealed structure that represents a keyword.                                                   *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

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
