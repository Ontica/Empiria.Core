/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Storage Services                  *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : StorageChangeItem                                Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Represents the changes performed over a storable object.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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
      get {
        throw new NotImplementedException();
        //return storableObject.ImplementsStorageUpdate(operation, DateTime.Now);
      }
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
