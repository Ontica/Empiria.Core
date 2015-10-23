/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : StorageChangeItemList                            Pattern  : Empiria List Class                *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents an ordered list of StorageChangeItemList instances.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using Empiria.Collections;

namespace Empiria {

  internal class StorageChangeItemList : BaseList<StorageChangeItem> {

    #region Fields

    #endregion Fields

    #region Constructors and parsers

    public StorageChangeItemList() {
      //no-op
    }

    public StorageChangeItemList(int capacity)
      : base(capacity) {
      // no-op
    }

    #endregion Constructors and parsers

    #region Public methods

    internal new void Add(StorageChangeItem storageChangeItem) {
      base.Add(storageChangeItem);
    }

    public override StorageChangeItem this[int index] {
      get { return base[index]; }
    }

    #endregion Public methods

  } // class StorageChangeItemList

} // namespace Empiria
