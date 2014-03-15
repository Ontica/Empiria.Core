/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : IUnitOfWork                                      Pattern  : Separated Interface               *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Interface that represents an identificable and serializable unit of work, that consists in    *
*              a serializable set of ordered operations, a guid identificator and status.                    *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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