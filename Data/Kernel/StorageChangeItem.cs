/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : StorageChangeItem                                Pattern  : Standard Class                    *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents the changes performed over a storable object.                                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
