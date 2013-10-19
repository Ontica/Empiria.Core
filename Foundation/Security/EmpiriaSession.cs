/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : EmpiriaSession                                   Pattern  : Standard Class                    *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This internal class represents an Empiria system user session.                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

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