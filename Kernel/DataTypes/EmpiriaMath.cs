/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaMath                                      Pattern  : Data Type                         *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Performs arithmetical operations over numerical data types.                                   *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Performs arithmetical operations over numerical data types.</summary>
  static public class EmpiriaMath {

    #region Public methods

    static public int Add(int a, int b) {
      return a + b;
    }

    static public decimal Add(decimal a, decimal b) {
      return a + b;
    }

    static public int Add(int a, params int[] list) {
      int result = 0;

      result = a;
      for (int i = 0; i < list.Length; i++) {
        result += list[i];
      }
      return result;
    }

    static public decimal Add(decimal a, params decimal[] list) {
      decimal result = 0m;

      result = a;
      for (int i = 0; i < list.Length; i++) {
        result += list[i];
      }
      return result;
    }

    static public decimal ApplyPercentage(decimal quantity, decimal percentage) {
      return (quantity * percentage) / 100m;
    }

    static public decimal Divide(int a, int b) {
      return ((decimal) a) / ((decimal) b);
    }

    static public decimal Divide(decimal a, decimal b) {
      return a / b;
    }

    static public int Divide(int a, int b, int ifZeroDivisionReturnValue) {
      if (b != 0) {
        return a / b;
      } else {
        return ifZeroDivisionReturnValue;
      }
    }

    static public decimal Divide(decimal a, decimal b, decimal ifZeroDivisionReturnValue) {
      if (b != 0) {
        return a / b;
      } else {
        return ifZeroDivisionReturnValue;
      }
    }

    static public int Max(int a, int b, int c) {
      return Math.Max(Math.Max(a, b), c);
    }

    static public int Min(int a, int b, int c) {
      return Math.Min(Math.Min(a, b), c);
    }

    static public int Multiply(int a, int b) {
      return a * b;
    }

    static public decimal Multiply(decimal a, decimal b) {
      return a * b;
    }

    static public decimal ScalarProduct(decimal[] a, decimal[] b) {
      decimal result = 0;

      for (int i = 0; i < a.Length; i++) {
        result += a[i] * b[i];
      }
      return result;
    }

    static public int Substract(int a, int b) {
      return a - b;
    }

    static public decimal Substract(decimal a, decimal b) {
      return a - b;
    }

    #endregion Public methods

  }  // class EmpiriaMath

} // namespace Empiria.DataTypes