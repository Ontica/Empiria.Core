/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : EventTypeInfo                                    Pattern  : Type metadata class               *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents an event type definition.                                                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
