/**
 *  Solution : Empiria Core Client                             || v0.1.0104
 *  Type     : Empiria.Session
 *  Summary  : Holds data about a user's session
 *
 *  Author   : José Manuel Cota <https://github.com/jmcota>
 *  License  : GNU GPLv3. Other licensing terms are available. See <https://github.com/Ontica/Empiria.Core>
 *
 *  Copyright (c) 2015-2016. Ontica LLC, La Vía Óntica SC and contributors. <http://ontica.org>
*/
import {Assertion} from "./assertion";
import {DataOperation} from "./data.operation";

/** Holds data about a user's session. */
export class Session {

  // #region Fields

  private static _singletonInstance: Session = new Session();

  private token: string = "";

  /* tslint:disable --> Unused fields */
  private tokenType: string = "";
  private refreshToken: string = "";
  private startTime: Date = new Date();
  private expiresIn: number = 0;
  /* tslint:enable */

  private apiKey: string = "";


  // #endregion Fields

  // #region Constructor and parsers

  constructor() {
    if (Session._singletonInstance) {
      throw new Error("Error: Instantiation failed: Use one of the static methods instead of new.");
    }
    Session._singletonInstance = this;
  }

  public static getCurrent(): Session {
    return Session._singletonInstance;
  }

  public static setCurrent(apiKey: string, token: string): Session {
    // var dataOperation = Empiria.DataOperation.parse("getEmpiriaSession", apiKey, activeToken);

    // var data = dataOperation.getData();

    // Empiria.Assertion.hasValue(data,
    //            "There was a problem reading session data for active token {0}.", activeToken);

    // return new Session(this.prototype.parseServerData(data));

    Session._singletonInstance.apiKey = apiKey;
    Session._singletonInstance.token = token;

    return Session._singletonInstance;

    // return new Session(apiKey, token);
  }

  public static authenticate(apiKey: string, username: string, password: string): Session {
    let dataOperation = DataOperation.parse("createEmpiriaSession",
                                             apiKey, username, this.prototype.md5(password));

    let data = dataOperation.getData();
    Assertion.assertValue(data, "There was a problem authenticating user '{0}'.", username);

    Session._singletonInstance.parseServerData(data);

    return Session._singletonInstance;
  }

  /** Closes this user session if it is opened. */
  public close(): void {
    let dataOperation = DataOperation.parse("closeEmpiriaSession", this.apiKey, this.token);

    try {
      dataOperation.execute();
    } catch (e) {
      console.log("Can't close session {0}", e);
    }
  }

  /** Returns the session's token. */
  public getToken(): string {
    return this.token;
  }

  /** Refreshes this user session if it is not closed, otherwise throws an error. */
  public refresh(): void {
    let dataOperation = DataOperation.parse("refreshEmpiriaSession", this.apiKey, this.token);

    try {
      dataOperation.execute();
    } catch (e) {
      console.log("Can't refresh session {0}", e);
      throw e;
    }
  }

  private parseServerData(serverData: any): any {
    let sessionData = {
      apiKey: serverData.apiKey,
      expiresIn: serverData.expiresIn,
      refreshToken: serverData.refreshToken,
      startTime: serverData.startTime,
      token: serverData.token,
      tokenType: serverData.tokenType
    };
    return sessionData;
  }

  private md5(source: string): string {
    return source;
  }

}  // class Session
