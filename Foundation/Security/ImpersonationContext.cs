/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : ImpersonationContext                             Pattern  : Context Class                     *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides a context for use resources that requires a Windows impersonation operation.         *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Empiria.Security {

  /// <summary>Provides a context for use resources that requires a Windows impersonation operation.</summary>
  public sealed class ImpersonationContext : IDisposable {

    #region Fields

    private string impersonationToken = String.Empty;

    private WindowsImpersonationContext impersonationContext = null;
    WindowsIdentity windowsIdentity = null;
    IntPtr token = IntPtr.Zero;
    IntPtr tokenDuplicate = IntPtr.Zero;

    private bool disposed = false;

    #endregion Fields

    #region Constructors and parsers

    private ImpersonationContext(string impersonationToken) {
      // instances of this class are created using one of the Open() methods
      this.impersonationToken = impersonationToken;
    }

    static public ImpersonationContext Open(string impersonationToken) {
      if (impersonationToken.Length == 0) {
        return new ImpersonationContext(impersonationToken);
      }
      if (!IsValidToken(impersonationToken)) {
        throw new SecurityException(SecurityException.Msg.WrongImpersonationToken, impersonationToken);
      }
      ImpersonationContext newContext = null;
      try {
        newContext = new ImpersonationContext(impersonationToken);
        if (!NativeMethods.RevertToSelf()) {
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        if (!newContext.LogonUser()) {
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        if (!newContext.DuplicateToken()) {
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        newContext.windowsIdentity = new WindowsIdentity(newContext.tokenDuplicate);
        newContext.impersonationContext = newContext.windowsIdentity.Impersonate();

        return newContext;
      } finally {
        if (newContext.token != IntPtr.Zero) {
          NativeMethods.CloseHandle(newContext.token);
        }
        if (newContext.tokenDuplicate != IntPtr.Zero) {
          NativeMethods.CloseHandle(newContext.tokenDuplicate);
        }
      }
    }

    ~ImpersonationContext() {
      Dispose(false);
    }

    #endregion Constructors and parsers

    #region Public properties

    public string ImpersonationToken {
      get { return this.impersonationToken; }
    }

    #endregion Public properties

    #region Public methods

    public void Close() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public void Dispose() {
      Close();
    }

    #endregion Public methods

    #region Private methods

    private void Dispose(bool disposing) {
      if (this.ImpersonationToken.Length == 0) {
        return;
      }
      if (!disposed) {
        if (disposing) {
          UndoImpersonation();
        }
        if (this.token != IntPtr.Zero) {
          NativeMethods.CloseHandle(this.token);
        }
        if (this.tokenDuplicate != IntPtr.Zero) {
          NativeMethods.CloseHandle(this.tokenDuplicate);
        }
      }
      disposed = true;
    }

    private bool DuplicateToken() {
      int result = NativeMethods.DuplicateToken(this.token, NativeMethods.LOGON32_LOGON_INTERACTIVE,
                                                ref this.tokenDuplicate);

      return (result != 0);
    }

    static private bool IsValidToken(string token) {
      return true;
    }

    private bool LogonUser() {
      string[] tokenPartsArray = ConfigurationData.GetString("ImpersonationToken." + this.impersonationToken).Split('|');
      string userName = tokenPartsArray[0];
      string domain = tokenPartsArray[1];
      string password = tokenPartsArray[2];

      int result = NativeMethods.LogonUser(userName, domain, password, NativeMethods.LOGON32_LOGON_INTERACTIVE,
                                           NativeMethods.LOGON32_PROVIDER_DEFAULT, ref this.token);
      return (result != 0);
    }

    /// <summary> /// Reverts the impersonation. /// </summary>
    private void UndoImpersonation() {
      if (windowsIdentity != null) {
        windowsIdentity.Dispose();
      }
      if (impersonationContext != null) {
        impersonationContext.Undo();
        impersonationContext.Dispose();
      }
    }

    #endregion Private methods

    #region P/Invoke class

    static private class NativeMethods {

      static public int LOGON32_LOGON_INTERACTIVE = 2;
      static public int LOGON32_PROVIDER_DEFAULT = 0;

      [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
      static public extern bool CloseHandle(IntPtr handle);

      [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      static public extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

      [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      static public extern bool RevertToSelf();

      [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      static public extern int LogonUser(string lpszUserName, string lpszDomain, string lpszPassword,
                                          int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

    }

    #endregion P/Invoke class

  } // class ImpersonationContext

} // namespace Empiria.Security
