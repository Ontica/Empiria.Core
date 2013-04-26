/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : EventTypeInfo                                    Pattern  : Type metadata class               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents an event type definition.                                                          *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/


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
