/**
 *  Solution : Empiria Core Client                             || v0.1.0104
 *  Type     : Empiria.Assertion
 *  Summary  : Static library that allows assertion checking.
 *
 *  Author   : José Manuel Cota <https://github.com/jmcota>
 *  License  : GNU GPLv3. Other licensing terms are available. See <https://github.com/Ontica/Empiria.Core>
 *
 *  Copyright (c) 2015-2016. Ontica LLC, La Vía Óntica SC and contributors. <http://ontica.org>
*/

module Empiria {

  /** Static type that allows assertion checking.
    *
    *  @class Assertion
    */
  export class Assertion {

    // #region Static methods

    /** Asserts a condition throwing an exception if it fails.
      * @param condition The condition to assert.
      * @param failMessage The message to throw if the assertion fails.
      * @param parameters Optional strings list to merge into the throwed message.
      */
    static assert(condition: boolean, failMessage: string, ...parameters: string[]): void {
      if (!condition) {
        var msg = this.prototype.mergeParametersIntoString(failMessage, parameters)

        throw new Exception(msg, "Assert");
      }
    }


    /** Throws a noReachThisCode exception. Used to stop the execution
        when the code flow reaches an invalid code line.
      */
    static assertNoReachThisCode(failMessage? : string): void {
      const defaultMsg = "PROGRAMMING ERROR: The program reached an invalid code flow statement." +
                         "Probably it has a loop, if or switch that miss an exit condition or " +
                         "there are data with unexpected values. Please report this incident " +
                         "immediately to the system administrator or at support @ ontica.org.";

      var msg = Empiria.Validate.hasValue(failMessage) ? failMessage : defaultMsg;

      throw new Exception(msg, "AssertNoReachThisCode");
    }


    /** Asserts an object has value distinct to null, undefined, NaN, or an empty string or object.
      * @param object The object to check its value.
      * @param failMessage The message to throw if the assertion fails.
      * @param parameters Optional strings list to merge into the throwed message.
      */
    static assertValue(object: any, failMessage: string, ...parameters: string[]): void {
      if (object == null || object == undefined || object == {} || object == NaN || object == "") {
        var msg = this.prototype.mergeParametersIntoString(failMessage, parameters)

        throw new Exception(msg, "AssertValue");
      }
    }

    // #endregion Static methods

    // #region Helper methods

    /** Helper that merges a set of strings into a parmeterized message.
      */
    private mergeParametersIntoString(message: string, parameters: string[]): string {
      var temp: string = message;

      for (var i = 0; i < parameters.length; i++) {
        temp = temp.replace("{" + i.toString() + "}", parameters[i]);
      }
      return temp;
    }
    // #endregion Helper methods

  }  // class Assertion

}  // module Empiria
