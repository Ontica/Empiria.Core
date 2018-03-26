﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : PasswordStrength                                 Pattern  : Helper Class                      *
*                                                                                                            *
*  Summary   : Helper class that verifies a password strength.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Helper class that verifies a password strength.</summary>
  public class PasswordStrength {

    #region Fields;

    private readonly EmpiriaUser user;
    private readonly string password;

    #endregion Fields;

    #region Constructors and parsers

    private PasswordStrength(string password) {
      this.password = password;
    }
    internal PasswordStrength(EmpiriaUser user, string password) {
      Assertion.AssertObject(user, "user");
      Assertion.AssertObject(password, "password");

      this.user = user;
      this.password = password;
    }

    static public void AssertIsValid(string password) {
      Assertion.AssertObject(password, "password");

      var instance = new PasswordStrength(password);

      instance.VerifyLength();
      instance.VerifyCharactersCombination();
    }
    #endregion Constructors and parsers

    #region Public methods

    internal void VerifyStrength() {
      this.VerifyLength();
      this.VerifyCharactersCombination();
      this.VerifyNoUserDataPortions();
    }

    #endregion Public methods

    #region Private methods

    private void VerifyCharactersCombination() {
      int counter = 0;

      if (EmpiriaString.ContainsAnyChar(password, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(password, "abcdefghijklmnopqrstuvwxyz")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(password, "0123456789")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(password, @"~¡!@#$€£%^*&¿?-+='_""(){}[]\/|ñÑ<>.,:;")) {
        counter++;
      }
      Validate.IsTrue(counter >= 3, "Passwords must contain characters from " +
                                    "at least three of the following four categories " +
                                    "arranged in any order:\n" +
                                    "English uppercase characters(A through Z)\n" +
                                    "English lowercase characters(A through Z)\n" +
                                    "Base 10 digits (0 through 9)\n" +
                                    "Non-alphabetic characters like: ~!@#$%^*&;?.+_");
    }

    private void VerifyLength() {
      Validate.IsTrue(password.Length >= 8,
                      "Passwords must be at least eight (8) characters in length.");
    }

    private void VerifyNoUserDataPortions() {
      if (EmpiriaString.ContainsSegment(password, user.UserName, 3) ||
          EmpiriaString.ContainsSegment(password, user.FirstName, 3) ||
          EmpiriaString.ContainsSegment(password, user.LastName, 3) ||
          EmpiriaString.ContainsSegment(password, user.EMail, 3)) {

        Validate.Fail("Passwords must not contain significant portions " +
                      "(three or more contiguous characters) of the user name, " +
                      "first name, last name, or email address.");
      }
    }

    #endregion Private methods

  } // class PasswordStrength

} // namespace Empiria.Security