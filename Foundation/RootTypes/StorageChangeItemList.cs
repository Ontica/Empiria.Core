/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : StorageChangeItemList                            Pattern  : Empiria List Class                *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents an ordered list of StorageChangeItemList instances.                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/

using Empiria.Collections;

namespace Empiria {

  internal class StorageChangeItemList : EmpiriaList<StorageChangeItem> {

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