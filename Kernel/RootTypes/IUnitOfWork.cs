/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : IUnitOfWork                                      Pattern  : Separated Interface               *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Interface that represents an identificable and serializable unit of work, that consists in    *
*              a serializable set of ordered operations, a guid identificator and status.                    *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Runtime.Serialization;

namespace Empiria {

  /// <summary>Interface that represents an identificable and serializable unit of work, that consists in
  /// a serializable set of ordered operations, a guid identificator and status.</summary>
  public interface IUnitOfWork : ISerializable, IDisposable {

    #region Members definition

    Guid Guid { get; }

    string Name { get; }

    DateTime TimeStamp { get; }

    int Update();

    #endregion Members definition

  } //interface IUnitOfWork

} //namespace Empiria