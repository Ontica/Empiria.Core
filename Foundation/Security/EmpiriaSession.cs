/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : EmpiriaSession                                   Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This internal class represents an Empiria system user session.                                *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Security {

  internal sealed class EmpiriaSession : IEmpiriaSession {

    #region Fields

    private int id = 0;
    private string token = String.Empty;
    private int serverId = 0;
    private int userId = 0;
    private string systemSession = String.Empty;
    private string clientAddress = String.Empty;
    private string clientEnvironment = String.Empty;
    private DateTime startTime = DateTime.Now;
    private DateTime endTime = ExecutionServer.DateMaxValue;

    #endregion Fields

    #region Constructors and parsers

    private EmpiriaSession() {
      // TODO: Complete member initialization
    }

    internal EmpiriaSession(int userId, string systemSession,
                            string clientAddress, string clientEnvironment) {
      this.serverId = ExecutionServer.ServerId;
      this.userId = userId;
      this.systemSession = systemSession;
      this.clientAddress = clientAddress;
      this.clientEnvironment = clientEnvironment;
      this.token = CreateToken();
      this.Create();
    }

    static internal bool TryParseActive(string sessionToken, out EmpiriaSession session) {
      DataRow row = SecurityData.GetSessionData(sessionToken);
      session = null;
      if (row != null) {
        session = new EmpiriaSession();
        session.Load(row);
        if (session.IsStillActive) {
          return true;
        }
      }
      return false;
    }

    private void Load(DataRow row) {
      this.id = (int) row["SessionId"];
      this.token = (string) row["SessionToken"];
      this.serverId = (int) row["ServerId"];
      this.userId = (int) row["UserId"];
      this.systemSession = (string) row["SystemSession"];
      this.clientAddress = (string) row["ClientAddress"];
      this.clientEnvironment = (string) row["ClientEnvironment"];
      this.startTime = (DateTime) row["StartTime"];
      this.endTime = (DateTime) row["EndTime"];
    }

    #endregion Constructors and parsers

    #region Public properties

    public int Id {
      get { return id; }
      internal set { id = value; }
    }

    public string ClientAddress {
      get { return clientAddress; }
    }

    public string ClientEnvironment {
      get { return clientEnvironment; }
    }

    public DateTime EndTime {
      get { return endTime; }
    }

    public bool IsStillActive {
      get {
        return true;
      }
    }

    public string Token {
      get { return token; }
    }

    public int ServerId {
      get { return serverId; }
    }

    public DateTime StartTime {
      get { return startTime; }
    }

    public string SystemSession {
      get { return systemSession; }
    }

    public int UserId {
      get { return userId; }
    }

    #endregion Public properties

    #region Internal and private methods

    internal void Close() {
      SecurityData.WriteSession(this);
    }

    private void Create() {
      SecurityData.WriteSession(this);
    }

    private string CreateToken() {
      string token = UserId.ToString() + "|" + ServerId.ToString() + "|" +
                     StartTime.ToString("yyyy-MM-dd_HH:mm:ss") + "|" +
                     SystemSession.ToString() + "|" + ClientAddress + "|" +
                     ClientEnvironment;
      return Guid.NewGuid().ToString() + "-" + Empiria.Security.Cryptographer.CreateHashCode(token);
    }

    internal string RegenerateToken() {
      this.token = CreateToken(); 
      SecurityData.WriteSession(this);

      return this.token;
    }

    internal void UpdateEndTime() {
      endTime = DateTime.Now;
    }

    #endregion Internal and private methods



  } //class EmpiriaSession

} //namespace Empiria.Security