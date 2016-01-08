/**
 *  Solution : Empiria Core Client                             || v0.1.0104
 *  Type     : Empiria.Validate
 *  Summary  : Static library that validate conditions.
 *
 *  Author   : José Manuel Cota <https://github.com/jmcota>
 *  License  : GNU GPLv3. Other licensing terms are available. See <https://github.com/Ontica/Empiria.Core>
 *
 *  Copyright (c) 2015-2016. Ontica LLC, La Vía Óntica SC and contributors. <http://ontica.org>
*/

module Empiria {

  /** Static library that validate conditions.
    *
    *  @class Validate
    */
  export class Validate {

    // #region Static methods

    /** Returns true if the object value is equal to null, undefined, NaN, an empty
        string or an empty object.
      * @param object The object to check its value.
      */
    public static hasValue(object: any): boolean {
      if (object == null || object == undefined || object == {} || object == NaN || object == "") {
        return false;
      }
      return true;
    }

    // #endregion Static methods

  }  // class Validate

}  // module Empiria
