/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : EventTypeInfo                                    Pattern  : Type metadata class               *
*                                                                                                            *
*  Summary   : Represents an event type definition.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Ontology {

  public sealed class EventTypeInfo : MetaModelType {

    #region Constructors and parsers

    private EventTypeInfo() : base(MetaModelTypeFamily.MethodType) {

    }

    static public new EventTypeInfo Parse(int id) {
      return MetaModelType.Parse<EventTypeInfo>(id);
    }

    static public new EventTypeInfo Parse(string name) {
      return MetaModelType.Parse<EventTypeInfo>(name);
    }

    #endregion Constructors and parsers

  } // class EventTypeInfo

} // namespace Empiria.Ontology
