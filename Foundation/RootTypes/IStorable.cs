/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Storage Services                  *
*  Namespace : Empiria.Storage                                  Assembly : Empiria.dll                       *
*  Type      : IStorable                                        Pattern  : Separated Interface               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Interface that represents an storable.                                                        *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
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