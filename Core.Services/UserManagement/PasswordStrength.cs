/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : User Management                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Service provider                        *
*  Type     : PasswordStrength                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains services to verify a password strength.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Security;

namespace Empiria.Services.UserManagement {

  /// <summary>Contains services to verify a password strength.</summary>
  public class PasswordStrength {

    #region Fields

    private readonly EmpiriaUser _user;
    private readonly string _password;

    #endregion Fields

    #region Constructors and parsers

    private PasswordStrength(string password) {
      _password = password;
    }

    internal PasswordStrength(EmpiriaUser user, string password) {
      Assertion.Require(user, "user");
      Assertion.Require(password, "password");

      _user = user;
      _password = password;
    }

    static public void AssertIsValid(string password) {
      Assertion.Require(password, "password");

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

      if (EmpiriaString.ContainsAnyChar(_password, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(_password, "abcdefghijklmnopqrstuvwxyz")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(_password, "0123456789")) {
        counter++;
      }
      if (EmpiriaString.ContainsAnyChar(_password, @"~¡!@#$€£%^*&¿?-+='_""(){}[]\/|ñÑ<>.,:;")) {
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
      Validate.IsTrue(_password.Length >= 8,
                      "Passwords must be at least eight (8) characters in length.");
    }


    private void VerifyNoUserDataPortions() {
      if (EmpiriaString.ContainsSegment(_password, _user.UserName, 3) ||
          EmpiriaString.ContainsSegment(_password, _user.FirstName, 3) ||
          EmpiriaString.ContainsSegment(_password, _user.LastName, 3) ||
          EmpiriaString.ContainsSegment(_password, _user.EMail, 3)) {

        Validate.Fail("Passwords must not contain significant portions " +
                      "(three or more contiguous characters) of the user name, " +
                      "first name, last name, or email address.");
      }
    }


    #endregion Private methods

  } // class PasswordStrength

} // namespace Empiria.Services.UserManagement
