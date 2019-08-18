/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaSession                                   Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : This internal class represents an Empiria system user session.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Json;

namespace Empiria.Security {

  public sealed class EmpiriaSession : IIdentifiable {

    #region Constructors and parsers

    private EmpiriaSession() {
      Initialize();
    }

    private EmpiriaSession(EmpiriaPrincipal principal, JsonObject contextData = null) {
      Initialize();
      this.ServerId = ExecutionServer.ServerId;
      this.ClientAppId = principal.ClientApp.Id;
      this.UserId = principal.Identity.User.Id;
      if (contextData != null) {
        this.ExtendedData = contextData;
      }
      this.Token = this.CreateToken();
      this.Create();
    }

    static internal EmpiriaSession Create(EmpiriaPrincipal principal, JsonObject contextData = null) {
      Assertion.AssertObject(principal, "principal");

      return new EmpiriaSession(principal, contextData);
    }

    static private EmpiriaSession Parse(DataRow row) {
      var session = new EmpiriaSession();
      session.Load(row);

      return session;
    }

    static internal EmpiriaSession ParseActive(string sessionToken) {
      DataRow row = SecurityData.GetSessionData(sessionToken);
      var session = EmpiriaSession.Parse(row);
      if (session.IsStillActive) {
        return session;
      } else {
        throw new SecurityException(SecurityException.Msg.ExpiredSessionToken, sessionToken);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public int Id {
      get;
      private set;
    }


    public string UID {
      get {
        return this.Token;
      }
    }

    public string RefreshToken {
      get;
      private set;
    }

    public string Token {
      get;
      private set;
    }

    public string TokenType {
      get {
        return "bearer";
      }
    }

    public int ClientAppId {
      get;
      private set;
    }


    public DateTime EndTime {
      get;
      private set;
    }

    public bool IsStillActive {
      get {
        return true;
      }
    }

    public int ExpiresIn {
      get;
      set;
    }

    public int ServerId {
      get;
      private set;
    }

    public JsonObject ExtendedData {
      get;
      private set;
    }

    public DateTime StartTime {
      get;
      private set;
    }

    public int UserId {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    internal void Close() {
      SecurityData.CloseSession(this);
    }

    internal void UpdateEndTime() {
      this.EndTime = DateTime.Now;
    }

    #endregion Public methods

    #region Private methods

    private void Create() {
      if (String.IsNullOrWhiteSpace(this.RefreshToken)) {
        this.RefreshToken = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
      }
      this.Id = SecurityData.CreateSession(this);
    }

    private string CreateToken() {
      string token = "|" + UserId.ToString() + "|" + ServerId.ToString() + "|" +
                     StartTime.ToString("yyyy-MM-dd_HH:mm:ss") + "|" + ExtendedData + "|";

      return Guid.NewGuid().ToString() + "-" + FormerCryptographer.CreateHashCode(token);
    }

    private void Initialize() {
      this.Id = 0;
      this.Token = String.Empty;
      this.ServerId = 0;
      this.ClientAppId = -1;
      this.UserId = 0;
      this.ExpiresIn = 3600;
      this.ExtendedData = JsonObject.Empty;
      this.StartTime = DateTime.Now;
      this.EndTime = ExecutionServer.DateMaxValue;
    }

    private void Load(DataRow row) {
      this.Id = (int) row["SessionId"];
      this.Token = (string) row["SessionToken"];
      this.ServerId = (int) row["ServerId"];
      this.ClientAppId = (int) row["ClientAppId"];
      this.UserId = (int) row["UserId"];
      this.ExpiresIn = (int) row["ExpiresIn"];
      this.RefreshToken = (string) row["RefreshToken"];
      this.StartTime = (DateTime) row["StartTime"];
      this.EndTime = (DateTime) row["EndTime"];
    }

    #endregion Private methods

  } //class EmpiriaSession

} //namespace Empiria.Security
