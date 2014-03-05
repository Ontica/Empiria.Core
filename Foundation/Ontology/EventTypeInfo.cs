/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : EventTypeInfo                                    Pattern  : Type metadata class               *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents an event type definition.                                                          *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  public sealed class EventTypeInfo : MetaModelType {

    #region Constructors and parsers

    private EventTypeInfo(int id)
      : base(MetaModelTypeFamily.MethodType, id) {

    }

    private EventTypeInfo(string name)
      : base(MetaModelTypeFamily.MethodType, name) {

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
