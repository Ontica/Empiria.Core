/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : StorageChangeItem                                Pattern  : Standard Class                    *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents the changes performed over a storable object.                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

using Empiria.Data;

namespace Empiria {

  internal class StorageChangeItem {

    #region Fields

    private StorageContextOperation operation;
    private IStorable storableObject;

    #endregion Fields

    #region Constructors and parsers

    internal StorageChangeItem(StorageContextOperation operation, IStorable storableObject) {
      this.operation = operation;
      this.storableObject = storableObject;
    }

    #endregion Constructors and parsers

    #region Public properties

    internal DataOperationList ChangesList {
      get { return storableObject.ImplementsStorageUpdate(operation, DateTime.Now); }
    }

    internal StorageContextOperation Operation {
      get { return operation; }
    }

    internal IStorable StorableObject {
      get { return storableObject; }
    }

    #endregion Public properties

  } // class StorageChangeItem

} //namespace Empiria
