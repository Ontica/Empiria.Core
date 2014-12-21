/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : OperationBase                                    Pattern  : Structure Type                    *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Abstract class that represents a named operation with or without parameters.                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Security;

namespace Empiria {

  /// <summary>Abstract class that represents a named operation with or without parameters.</summary>
  public abstract class OperationBase {

    #region Fields

    private string name = String.Empty;
    private object[] parameters = new object[0];

    #endregion Fields

    #region Constructors and parsers

    private OperationBase() {
      // Default instance constructor not allowed
    }

    protected OperationBase(string name) {
      Assertion.AssertObject(name, "name");

      this.name = name;
    }

    protected OperationBase(string name, object[] parameters) {
      Assertion.AssertObject(name, "name");
      Assertion.AssertObject(parameters, "parameters");

      this.name = name;
      this.parameters = parameters;
    }

    static protected void ExtractFromMessage(string message, out string name, out object[] parameters) {
      Assertion.AssertObject(message, "message");

      name = message.Split('§')[0];
      string parametersString = message.Split('§')[1];

      if (parametersString.Length != 0) {
        string[] parsValues = parametersString.Split('¸');
        parameters = new object[parsValues.Length];

        for (int i = 0; i < parsValues.Length; i++) {
          parameters[i] = parsValues[i];
        }
      } else {
        parameters = null;
      }
    }

    static protected void ExtractFromMessageProtected(string message, out string name, out object[] parameters) {
      Assertion.AssertObject(message, "message");

      message = Cryptographer.Decrypt(message);
      ExtractFromMessage(message, out name, out parameters);
    }

    #endregion Constructors and parsers

    #region Public properties

    public object[] Parameters {
      get { return parameters; }
    }

    public string Name {
      get { return name; }
    }

    #endregion Public properties

    #region Public methods

    public string ParametersToString() {
      string parametersString = String.Empty;

      for (int i = 0; i < parameters.Length; i++) {
        parametersString += (i != 0 ? "¸" : String.Empty) + Convert.ToString(parameters[i]);
      }

      return parametersString;
    }

    public string ParametersToStringProtected() {
      return Cryptographer.Encrypt(EncryptionMode.Standard, this.ParametersToString());
    }

    public string ToMessage() {
      return this.name + "§" + this.ParametersToString();
    }

    public string ToMessageProtected() {
      return Cryptographer.Encrypt(EncryptionMode.Standard, this.ToMessage());
    }

    #endregion Public methods

  } // class OperationBase

} //namespace Empiria
