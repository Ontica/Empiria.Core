/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Storage Services                  *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : StorageChangeItemList                            Pattern  : Empiria List Class                *
*                                                                                                            *
*  Summary   : Represents an ordered list of StorageChangeItemList instances.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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
