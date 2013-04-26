/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : Structure                                        Pattern  : Abstract Class                    *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : The type Structure is the root of the structure type hierarchy. All structure types must be   *
*              descendants of this type.                                                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Data;
using Empiria.Ontology;

namespace Empiria {

  internal class DynamicState : IStorable {

    #region Fields

    private Dictionary<string, object> state = new Dictionary<string, object>();
    private BaseObject instance = null;

    private bool valuesReaded = false;

    #endregion Fields

    #region Constructors and parsers

    public DynamicState(BaseObject instance) {
      this.instance = instance;
      Initialize();
    }

    #endregion Constructors and parsers

    #region Public methods

    internal T GetMember<T>(string name) {
      if (!valuesReaded) {
        ReadMemberValues();
      }
      if (state.ContainsKey(name)) {
        return (T) state[name];
      } else {
        throw new OntologyException(OntologyException.Msg.RelationMemberNameNotFound, instance.ObjectTypeInfo.Name, name);
      }
    }

    internal void SetMember<T>(string name, T value) {
      if (state.ContainsKey(name)) {
        state[name] = value;
      } else {
        throw new NotSupportedException(name);
      }
    }

    int IIdentifiable.Id {
      get { return instance.Id; }
    }

    DataOperationList IStorable.ImplementsStorageUpdate(StorageContextOperation operation, DateTime timestamp) {
      throw new NotImplementedException();
    }

    void IStorable.ImplementsOnStorageUpdateEnds() {
      throw new NotImplementedException();
    }

    #endregion Public methods

    #region Private methods

    private void Initialize() {
      TypeRelationInfo[] relations = instance.ObjectTypeInfo.GetRelations();
      foreach (TypeRelationInfo relationInfo in relations) {
        state.Add(relationInfo.Name, relationInfo.GetDefaultValue());
      }
      valuesReaded = false;
    }

    private void ReadMemberValues() {
      DataView view = OntologyData.GetObjectAttributes(this.instance.ObjectTypeInfo, this.instance);
      for (int i = 0; i < view.Count; i++) {
        TypeAttributeInfo attribute = instance.ObjectTypeInfo.GetAttribute((int) view[i]["TypeRelationId"]);
        state[attribute.Name] = attribute.Convert(view[i]["AttributeValue"]);
      }
      valuesReaded = true;
    }

    #endregion Private methods

  } // class Structure

} // namespace Empiria.