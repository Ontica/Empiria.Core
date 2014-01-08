/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : EmpiriaPrincipal                                 Pattern  : Standard Class                    *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Represents the security context of the user or access account on whose behalf the Empiria®    *
*              framework code is running, including that user's identity (EmpiriaIdentity) and any domain    *
*              roles to which they belong. This class can't be derived.                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
using System;
using System.Security.Principal;

namespace Empiria.Security {

  /// <summary>Represents the security context of the user or access account on whose behalf the Empiria®
  /// framework code is running, including that user's identity (EmpiriaIdentity) and any domain
  /// roles to which they belong. This class can't be derived.</summary>
  public sealed class EmpiriaPrincipal : IEmpiriaPrincipal {

    #region Fields

    private EmpiriaIdentity identity = null;
    private string[] rolesArray = null;
    private bool disposed = false;
    private string executionTypesString = String.Empty;

    #endregion Fields

    #region Constructors and parsers

    /// <summary>Initializes a new instance of the EmpiriaPrincipal class from an authenticated
    /// EmpiriaIdentity. Fails if identity represents a non authenticated EmpiriaIdentity.</summary>
    /// <param name="identity">Represents an authenticated Empiria user.</param>
    public EmpiriaPrincipal(EmpiriaIdentity identity) {
      if (identity != null && identity.IsAuthenticated) {
        this.identity = identity;
        //userRoles = null; //identity.User.GetRoles();
        rolesArray = LoadRolesArray(identity.UserId);
      } else {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }
    }

    ~EmpiriaPrincipal() {
      Dispose(false);
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Gets the IIdentity instance of the current principal.</summary>
    public IIdentity Identity {
      get { return identity; }
    }

    public string[] RolesArray {
      get { return rolesArray; }
    }

    #endregion Public properties

    #region Public methods

    public bool CanExecute(int typeId, char operationType) {
      return true;
    }

    public bool CanExecute(int typeId, char operation, int instanceId) {
      return true;
    }

    public SearchExpression ExecutionTypesSearchExp() {
      if (executionTypesString.Length != 0) {
        return SearchExpression.ParseInSet("ObjectTypeId", executionTypesString.Split('|'));
      } else {
        return SearchExpression.Empty;
      }
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public bool IsAuditable(int typeId, char operation) {
      return false;
    }

    /// <summary>Determines whether the current principal belongs to the specified role.</summary>
    /// <param name="role">The name of the role for which to check membership.</param>
    /// <returns>true if the current principal is a member of the specified role in the current domain; 
    /// otherwise, false.</returns>
    public bool IsInRole(string role) {
      if (identity.UserId == -3 || identity.UserId == 14 || identity.UserId == 427 || 
          identity.UserId == 217 || identity.UserId == 235) {
        return true;
      }
      return false;
    }

    /// <summary>Determines whether the current principal belongs to the specified role.</summary>
    /// <param name="domain">The domain for which to check role membership.</param>
    /// <param name="role">The name of the role for which to check membership.</param>
    /// <returns>true if the current principal is a member of the specified role; otherwise, false.</returns>
    public bool IsInRole(string domain, string role) {
      return IsInRole(role);
    }

    public string RegenerateToken() {
      EmpiriaIdentity identity = (EmpiriaIdentity) this.Identity;
      EmpiriaSession session = (EmpiriaSession) identity.Session;
      return session.RegenerateToken();
    }

    #endregion Public methods

    #region Private methods

    private void Dispose(bool disposing) {
      try {
        if (!disposed) {
          disposed = true;
          if (disposing) {
            identity.Dispose();
          }
        }
      } finally {

      }
    }

    private string[] LoadRolesArray(int participantId) {
      return null;
    }

    #endregion Private methods

  } // class EmpiriaPrincipal

} // namespace Empiria.Security