/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Providers                    *
*  Assembly : Empiria.Core.dll                             Pattern   : Dependency inversion interface        *
*  Type     : IRSACryptoServiceProvider                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Interface for cryptography services providers.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Security;
using System.Security.Cryptography;

namespace Empiria.Security.Providers {

  /// <summary>Interface for cryptography services providers.</summary>
  public interface ICryptoServiceProvider {

    /// <summary>Gets a RSACryptoServiceProvider using a private key file path and a secured password.</summary>
    RSACryptoServiceProvider GetRSAProvider(string privateKeyFilePath, SecureString password);


    /// <summary>Gets a RSACryptoServiceProvider using the system's default private key file.</summary>
    RSACryptoServiceProvider GetRSASystemProvider();


  }  // interface IRSACryptoServiceProvider

}  // namespace Empiria.Security.Providers
