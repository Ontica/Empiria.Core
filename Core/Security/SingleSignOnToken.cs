﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : SingleSignOnToken                                Pattern  : Static Class                      *
*                                                                                                            *
*  Summary   : Represents a single sign-on data token that enables a looged user log in once and gain access *
*              to multiple software systems without beign prompted to log in again.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.Security {

  /// <summary>Represents a single sign-on data token that enables a looged user log in once and gain access
  /// to multiple software systems without beign prompted to log in again.</summary>
  public sealed class SingleSignOnToken {

    #region Fields

    private readonly int userId = 0;
    private readonly string sessionToken = String.Empty;
    private List<int> path = null;
    private int checksum = 0;

    #endregion Fields

    #region Constructors and Parsers

    private SingleSignOnToken(WebServer targetServer) {
      this.sessionToken = ExecutionServer.CurrentPrincipal.Session.Token;
      this.userId = ExecutionServer.CurrentUserId;

      this.path = new List<int>(2);
      this.path.Add(ExecutionServer.ServerId);
      this.path.Add(targetServer.Id);
      CalculateChecksum();
    }

    private SingleSignOnToken(string token, int userId, List<int> path) {
      this.sessionToken = token;
      this.userId = userId;
      this.path = path;

      CalculateChecksum();
    }

    static public SingleSignOnToken Create(WebServer targetServer) {
      return new SingleSignOnToken(targetServer);
    }

    static public SingleSignOnToken ParseFromMessage(string message) {
      try {
        string[] parts = message.Split('§');

        string msgToken = parts[0];
        int userId = int.Parse(parts[1]);
        string[] integrationPath = parts[2].Split('~');

        List<int> path = new List<int>(integrationPath.Length);
        for (int i = 0; i < integrationPath.Length; i++) {
          path.Add(int.Parse(integrationPath[i]));
        }
        int checksum = int.Parse(parts[3]);

        SingleSignOnToken token = new SingleSignOnToken(msgToken, userId, path);
        if (token.checksum == checksum) {
          return token;
        } else {
          throw new SecurityException(SecurityException.Msg.InvalidSingleSignOnToken);
        }
      } catch {
        throw new SecurityException(SecurityException.Msg.InvalidSingleSignOnToken);
      }
    }

    #endregion Constructors and Parsers

    #region Public methods

    public SingleSignOnToken Copy() {
      List<int> copyPath = new List<int>(this.path.Count);
      for (int i = 0; i < this.path.Count; i++) {
        copyPath.Add(this.path[i]);
      }
      SingleSignOnToken token = new SingleSignOnToken(this.sessionToken, this.userId, copyPath);
      token.CalculateChecksum();

      return token;
    }

    public bool ExistsOnPath(WebServer server) {
      for (int i = 0; i < this.path.Count; i++) {
        if (this.path[i] == server.Id) {
          return true;
        }
      }
      return false;
    }

    public SingleSignOnToken SignOnServer(WebServer targetServer) {
      SingleSignOnToken token = this.Copy();

      if (targetServer.Id != ExecutionServer.ServerId) {
        token.path.Add(targetServer.Id);
      }
      token.CalculateChecksum();

      return token;
    }

    public string ToMessage() {
      string msg = sessionToken.ToString() + "§";
      msg += userId.ToString() + "§";
      msg += PathToString() + "§";
      msg += checksum.ToString();

      return msg;
    }

    #endregion Public methods

    #region Private methods

    private void CalculateChecksum() {
      int accumulator = Math.Abs(sessionToken.GetHashCode()) % 41;
      for (int i = 0; i < path.Count; i++) {
        accumulator += (path[i] * path.Count) % (i + 7);
      }
      accumulator += (userId * userId) % path.Count;

      this.checksum = accumulator + 23;
    }

    private string PathToString() {
      string message = String.Empty;

      for (int i = 0; i < path.Count; i++) {
        if (i == 0) {
          message += path[i].ToString();
        } else {
          message += '~' + path[i].ToString();
        }
      }

      return message;
    }

    #endregion Private methods

  } // class SecurityToken

} // namespace Empiria.Security
