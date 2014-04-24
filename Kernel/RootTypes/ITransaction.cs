/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ITransaction                                     Pattern  : Separated Interface               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Interface that represents a transactional operation.                                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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