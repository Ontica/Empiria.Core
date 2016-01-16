/**
 *  Solution : Empiria Core Client                             || v0.1.0104
 *  Type     : Empiria.Exception
 *  Summary  : Static library that allows assertion checking.
 *
 *  Author   : José Manuel Cota <https://github.com/jmcota>
 *  License  : GNU GPLv3. Other licensing terms are available. See <https://github.com/Ontica/Empiria.Core>
 *
 *  Copyright (c) 2015-2016. Ontica LLC, La Vía Óntica SC and contributors. <http://ontica.org>
*/

export class Exception extends Error {

  private tag: string = "undefined";

  constructor(message: string, tag?: string) {
    super(message);
    super.name = "EmpiriaException";
    this.tag = tag;
    super.message = message;      // Assign because there are ES6 issues yet
                                  // look at: https://github.com/Microsoft/TypeScript/issues/5069
                                  //          https://github.com/Microsoft/TypeScript/issues/1168
  }

  // Override of the toString method, in order to return a specific message.
  public toString(): string {
    return super.message + " [Code: " + this.tag + "]";
  }

}  // class Exception
