/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataOperationList                                Pattern  : List Class                        *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a synchronized and serializable list of Operation type objects.                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using Empiria.Collections;

namespace Empiria.Data {

  public sealed class DataOperationList : BaseList<DataOperation> {

    #region Constructors and parsers

    public DataOperationList(string name) : base(true) {
      this.Name = name;
    }

    static public DataOperationList Parse(string[] messagesArray) {
      DataOperationList list = new DataOperationList(messagesArray[0]);

      for (int i = 1; i < messagesArray.Length; i++) {
        list.Add(DataOperation.ParseFromMessage(messagesArray[i]));
      }

      return list;
    }

    #endregion Constructors and parsers

    #region Public properties

    public new DataOperation this[int index] {
      get { return (DataOperation) base[index]; }
    }

    public string Name {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    public new void Add(DataOperation operation) {
      if (operation != null) {
        base.Add(operation);
      }
    }

    public void Add(DataOperationList operationList) {
      if (operationList != null) {
        for (int i = 0, j = operationList.Count; i < j; i++) {
          base.Add(operationList[i]);
        }
      }
    }

    public new void Clear() {
      base.Clear();
    }

    public void Execute(string contextName) {
      using (DataWriterContext context = new DataWriterContext(contextName)) {
        if (this.Count > 1) {
          ITransaction transaction = context.BeginTransaction();
          context.Add(this);
          context.Update();
          transaction.Commit();
        } else if (this.Count == 1) {
          context.Add(this);
          context.Update();
        }
      }
    }

    public new void RemoveLast(int count) {
      base.RemoveLast(count);
    }

    public new void RemoveRange(int index, int count) {
      base.RemoveRange(index, count);
    }

    public new void Reverse() {
      base.Reverse();
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
