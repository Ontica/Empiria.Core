/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : IUnitOfWork                                      Pattern  : Separated Interface               *
*                                                                                                            *
*  Summary   : Interface that represents an identificable and serializable unit of work, that consists in    *
*              a serializable set of ordered operations, a guid identificator and status.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Interface that represents a unit of work, that consists in a set of ordered operations
  /// that can be commited all at once.</summary>
  public interface IUnitOfWork : IDisposable {

    #region Members definition

    Guid Guid { get; }

    string Name { get; }

    DateTime Timestamp { get; }

    void Commit();

    void Rollback();

    #endregion Members definition

  } //interface IUnitOfWork

} //namespace Empiria
