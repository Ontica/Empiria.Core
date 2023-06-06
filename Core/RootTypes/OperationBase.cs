/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                                 Component : Fundamental Types                     *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : OperationBase                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Abstract class that represents a named operation with or without parameters.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Newtonsoft.Json;

using Empiria.Security;

namespace Empiria {

  /// <summary>Abstract class that represents a named operation with or without parameters.</summary>
  public abstract class OperationBase {

    #region Constructors and parsers

    protected OperationBase(string name) {
      Assertion.Require(name, "name");

      this.Name = name;
      this.Parameters = new object[0];
    }


    protected OperationBase(string name, object[] parameters) {
      Assertion.Require(name, "name");
      Assertion.Require(parameters, "parameters");

      this.Name = name;
      this.Parameters = parameters;
    }


    static protected void ExtractFromMessage(string message, out string name, out object[] parameters) {
      Assertion.Require(message, "message");

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

    #endregion Constructors and parsers

    #region Public properties

    public string Name {
      get;
    }


    public object[] Parameters {
      get;
    }

    #endregion Public properties

    #region Public methods

    public string ParametersAsJson() {
      if (this.Parameters.Length != 0) {
        return JsonConvert.SerializeObject(this.Parameters);
      } else {
        return String.Empty;
      }
    }


    public string ParametersToString() {
      string parametersString = String.Empty;

      for (int i = 0; i < this.Parameters.Length; i++) {
        parametersString += (i != 0 ? "¸" : String.Empty) + Convert.ToString(this.Parameters[i]);
      }

      return parametersString;
    }


    public string ToMessage() {
      return this.Name + "§" + this.ParametersToString();
    }


    #endregion Public methods

  } // class OperationBase

} //namespace Empiria
