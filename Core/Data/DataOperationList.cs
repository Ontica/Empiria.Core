/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     License  : Please read LICENSE.txt file      *
*  Type      : DataOperationList                                Pattern  : List Class                        *
*                                                                                                            *
*  Summary   : Represents a list of DataOperation objects.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Collections;

namespace Empiria.Data {

  /// <summary>Represents a list of DataOperation objects.</summary>
  public sealed class DataOperationList : BaseList<DataOperation> {

    #region Constructors and parsers

    public DataOperationList(string name) : base(true) {
      this.Name = name;
    }

    #endregion Constructors and parsers

    #region Public properties

    public new DataOperation this[int index] {
      get {
        return base[index];
      }
    }


    public string Name {
      get;
    }

    #endregion Public properties

    #region Public methods

    public new void Add(DataOperation operation) {
      if (operation == null) {
        return;
      }

      base.Add(operation);
    }

    public void Add(ICollection<DataOperation> operations) {
      base.AddRange(operations);
    }


    public void Add(DataOperationList operationList) {
      base.AddRange(operationList);
    }


    internal void Add(IEnumerable<DataOperation> items) {
      base.AddRange(items);
    }


    public new void Clear() {
      base.Clear();
    }


    public new void Remove(DataOperation operation) {
      base.Remove(operation);
    }


    internal new int RemoveAll(Predicate<DataOperation> match) {
      return base.RemoveAll(match);
    }


    public string[] ToMessagesArray() {
      string[] messages = new string[this.Count + 1];

      messages[0] = this.Name;

      for (int i = 1; i < this.Count; i++) {
        messages[i] = this[i].ToMessage();
      }

      return messages;
    }

    #endregion Public methods

  } //class DataOperationList

} //namespace Empiria.Data
