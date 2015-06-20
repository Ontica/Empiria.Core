/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ITransaction                                     Pattern  : Separated Interface               *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Interface that represents a transactional operation.                                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
