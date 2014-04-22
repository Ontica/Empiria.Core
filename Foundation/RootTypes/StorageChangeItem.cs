/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : StorageChangeItem                                Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents the changes performed over a storable object.                                      *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
