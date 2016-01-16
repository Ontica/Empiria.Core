﻿/// <reference path="../lib/jquery.d.ts"/>

/**
 *  Solution : Empiria Core Client                             || v0.1.0104
 *  Type     : Empiria.DataOperation
 *  Summary  : Performs data read and write operations using Http (Ajax) requests.
 *
 *  Author   : José Manuel Cota <https://github.com/jmcota>
 *  License  : GNU GPLv3. Other licensing terms are available. See <https://github.com/Ontica/Empiria.Core>
 *
 *  Copyright (c) 2015-2016. Ontica LLC, La Vía Óntica SC and contributors. <http://ontica.org>
 */
import {Session} from "./session";

/** Performs data read and write operations using Http (Ajax) requests. */
export class DataOperation {

  // #region Fields

  private uid: string = "";
  private url: string = "";
  private async: boolean = false;
  private crossDomain: boolean = false;
  private method: string = "";

  // #endregion Fields

  // #region Constructor and parsers

  constructor(dataOperationUID: any) {
    this.uid = dataOperationUID;
  }

  /** Static method that parses a data operation, gets its rules and sets its parameters.
    * @param dataOperationUID A string with the data operation Unique ID.
    * @param parameters Optional list of data operation parameters.
    */
  public static parse(dataOperationUID: string, ...parameters: string[]): DataOperation {
    let dataOperation = new DataOperation(dataOperationUID);

    dataOperation.load(parameters);

    return dataOperation;
  }

  // #endregion Constructor and parsers

  // #region Public methods

  /** Executes the operation returning nothing, but throws an exception if it fails. */
  public execute(): void {
    jQuery.ajax(this.url, {
        async: this.async,
        crossDomain: this.crossDomain,
        headers: {
          "Authorization": "bearer " + this.getSessionToken(),
        },
        type: this.method,
      })
      .fail(function (response) {
        console.log("Something was wrong executing the data operation.");
        throw "Something was wrong executing the data operation.";
      });
  }

  /** Executes the operation and returns the response data, throwing an exception if it fails. */
  public executeAndGetData(): any {
    let returnedData: any = undefined;

    jQuery.ajax(this.url, {
        async: this.async,
        crossDomain: this.crossDomain,
        headers: {
          "Authorization": "bearer " + this.getSessionToken(),
        },
        type: this.method,
      })
      .done(function (response) {
        returnedData = response.data;
      })
      .fail(function (response) {
        console.log("Something was wrong executing the data operation.");
        throw "Something was wrong executing the data operation.";
      });
    return returnedData;
  }

  /** Invokes a read operation and returns true or false depending
      if the response has any data. */
  public existsData(): boolean {
    let existsData = false;

    jQuery.ajax(this.url, {
      async: this.async,
      crossDomain: this.crossDomain,
      headers: {
        "Authorization": "bearer " + this.getSessionToken(),
      },
      type: this.method,
    })
    .done(function (response) {
      existsData = true;
    })
    .fail(function (response) {
      console.log(response);
      existsData = false;
    });
    return existsData;
  }

  /** Invokes a read operation and gets data according to the operation rules. */
  public getData(): any {
    let returnedData: any = undefined;

    jQuery.ajax(this.url, {
        async: this.async,
        crossDomain: this.crossDomain,
        headers: {
          "Authorization": "bearer " + this.getSessionToken(),
        },
        type: this.method,
      })
      .done(function (response) {
        returnedData = response.data;
      })
      .fail(function (response) {
        console.log(response);
        throw ("something was WRONG with getData...");
      });
    return returnedData;
  }

  /** Invokes a data write operation and returns the response according
    *  to the configured operation rules.
    * @param dataToWrite The object to write.
    */
  public writeData(dataToWrite: any): any {
    let returnedData: any = undefined;

    jQuery.ajax(this.url, {
      async: this.async,
      contentType: "application/json; charset=utf-8",
      crossDomain: this.crossDomain,
      data: JSON.stringify(dataToWrite),
      dataType: "json",
      headers: {
        "Authorization": "bearer " + this.getSessionToken(),
      },
      type: this.method,
    })
    .done(function (response) {
      returnedData = response.data;
    })
    .fail(function (response) {
      console.log(response);
      throw ("something was WRONG with writeData...");
    });
    return returnedData;
  }

  // #endregion Public methods

  // #region Private methods

  private getConfigurationData(): any {
    let allRules = this.readConfigurationFile();

    return allRules[this.uid];
  }

  private getServer(): string {
    // return "http://jmcota/empiria.land/tlaxcala/api/";
    // return "http://187.157.152.5/services/";
    return "http://192.168.1.22/services/";
  }

  private getSessionToken(): string {
    return Session.getCurrent().getToken();
  }

  /** Loads data operation configuration rules from a json.config data file.
    * @param parameters An array with the data operation parameters.
    */
  private load(parameters: string[]): void {
    let configData = this.getConfigurationData();

    this.url = this.getServer() + configData.url;
    this.method = configData.method;
    this.async = configData.async;
    this.crossDomain = configData.crossDomain;

    for (let i = 0; i < parameters.length; i++) {
      this.url = this.url.replace("{" + i.toString() + "}", parameters[i]);
    }
  }

  private readConfigurationFile(): any {
    return {
      "getLandCertificate": {
        "url": "v1/certificates/{0}",
        "async": false,
        "crossDomain": true,
        "method": "GET",
      },

      "getLandCertificateText": {
        "url": "v1/certificates/{0}/as-text",
        "async": false,
        "crossDomain": true,
        "method": "GET",
      },

      "createLandCertificate": {
        "url": "v1/certificates",
        "async": false,
        "crossDomain": true,
        "method": "POST",
      },

      "updateLandCertificate": {
        "url": "v1/certificates/{0}",
        "async": false,
        "crossDomain": true,
        "method": "PUT",
      },

      "closeLandCertificate": {
        "url": "v1/certificates/{0}/close",
        "async": false,
        "crossDomain": true,
        "method": "POST",
      },

      "openLandCertificate": {
        "url": "v1/certificates/{0}/open",
        "async": false,
        "crossDomain": true,
        "method": "POST",
      },

      "deleteLandCertificate": {
        "url": "v1/certificates/{0}",
        "async": false,
        "crossDomain": true,
        "method": "DELETE",
      },

      "existsLandProperty": {
        "url": "v1/properties/{0}",
        "async": false,
        "crossDomain": true,
        "method": "GET",
      },

    };
  }

  // #endregion Private methods

}  // class DataOperation
