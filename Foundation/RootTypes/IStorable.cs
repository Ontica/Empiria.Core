/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria.Storage                                  Assembly : Empiria.dll                       *
*  Type      : IStorable                                        Pattern  : Loose coupling interface          *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Interface that represents an storable.                                                        *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Data;

namespace Empiria {

  /// <summary>Interface that represents an identifiable object throw an integer Id.</summary>
  public interface IStorable : IIdentifiable {

    #region Members definition

    DataOperationList ImplementsStorageUpdate(StorageContextOperation operation, DateTime timestamp);

    void ImplementsOnStorageUpdateEnds();

    #endregion Members definition

  } // interface IStorable

} // namespace Empiria