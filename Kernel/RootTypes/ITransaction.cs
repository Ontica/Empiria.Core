/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ITransaction                                     Pattern  : Separated Interface               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Interface that represents a transactional operation.                                          *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
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