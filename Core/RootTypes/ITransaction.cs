/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ITransaction                                     Pattern  : Separated Interface               *
*                                                                                                            *
*  Summary   : Interface that represents a transactional operation.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Interface that represents a transactional operation.</summary>
  public interface ITransaction : IDisposable {

    #region Members definition

    int Commit();

    void Rollback();

    #endregion Members definition

  } // interface ITransaction

} // namespace Empiria
