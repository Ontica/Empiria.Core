/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : IUnitOfWork                                      Pattern  : Separated Interface               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Interface that represents an identificable and serializable unit of work, that consists in    *
*              a serializable set of ordered operations, a guid identificator and status.                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Runtime.Serialization;

namespace Empiria {

  /// <summary>Interface that represents a unit of work, that consists in a set of ordered operations
  /// that can be commited all at once.</summary>
  public interface IUnitOfWork : IDisposable {

    #region Members definition

    Guid Guid { get; }

    string Name { get; }

    DateTime Timestamp { get; }

    int Update();

    #endregion Members definition

  } //interface IUnitOfWork

} //namespace Empiria
