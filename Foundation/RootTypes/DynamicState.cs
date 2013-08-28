/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : DynamicState                                     Pattern  : Abstract Class                    *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : DynamicState holds a bag of attributes for an object or type.                                 *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Data;
using Empiria.Ontology;

namespace Empiria {

  /// <summary>DynamicState holds a bag of attributes for an object or type.</summary>
  internal class DynamicState : IStorable {

    #region Fields

    private Dictionary<string, object> state = null;

    MetaModelType metaModelType = null;
    private IStorable instance = null;

    #endregion Fields

    #region Constructors and parsers

    public DynamicState(BaseObject instance) {
      this.metaModelType = instance.ObjectTypeInfo;
      this.instance = instance;
    }

    public DynamicState(MetaModelType instance) {
      this.metaModelType = instance;
      this.instance = instance;
    }

    #endregion Constructors and parsers

    #region Public methods

    internal T GetValue<T>(string name) {
      if (state == null) {
        lock (instance) {
          ReadMemberValues();
        }
      }
      if (state.ContainsKey(name)) {
        return (T) state[name];
      } else {
        throw new OntologyException(OntologyException.Msg.RelationMemberNameNotFound,
                                    this.metaModelType.Name, name);
      }
    }

    internal void SetValue<T>(string name, T value) {
      if (state == null) {
        lock (instance) {
          ReadMemberValues();
        }
      }
      if (state.ContainsKey(name)) {
        state[name] = value;
      } else {
        throw new NotSupportedException(name);
      }
    }

    int IIdentifiable.Id {
      get { return instance.Id; }
    }

    DataOperationList IStorable.ImplementsStorageUpdate(StorageContextOperation operation,
                                                        DateTime timestamp) {
      throw new NotImplementedException();
    }

    void IStorable.ImplementsOnStorageUpdateEnds() {
      throw new NotImplementedException();
    }

    #endregion Public methods

    #region Private methods

    private void ReadMemberValues() {
      if (state != null) {
        return;
      }
      var array = this.metaModelType.GetAttibuteKeyValues();
      state = new Dictionary<string,object>(array.Length);
      foreach (KeyValuePair<string, object> item in array) {
        state.Add(item.Key, item.Value);
      }
      DataTable table = OntologyData.GetObjectAttributes(this.metaModelType, instance);
      foreach(DataRow row in table.Rows) {
        TypeAttributeInfo attribute = this.metaModelType.Attributes[(int) row["TypeRelationId"]];
        state[attribute.Name] = attribute.Convert(row["AttributeValue"]);
      }
    }

    #endregion Private methods

  } // class DynamicState

} // namespace Empiria