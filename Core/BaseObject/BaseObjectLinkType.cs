/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Base Objects                               Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Powertype                               *
*  Type     : BaseObjectLinkType                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Powertype that describes a link between two objects derived from BaseObject.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria {

  /// <summary>Powertype that describes a link between two objects derived from BaseObject.</summary>
  [Powertype(typeof(BaseObjectLink))]
  public class BaseObjectLinkType : Powertype {

    #region Constructors and parsers

    protected BaseObjectLinkType() {
      // Empiria power types always have this constructor.
    }

    static public new BaseObjectLinkType Parse(int typeId) => Parse<BaseObjectLinkType>(typeId);

    static public new BaseObjectLinkType Parse(string typeName) => Parse<BaseObjectLinkType>(typeName);

    static public BaseObjectLinkType Empty => Parse("ObjectTypeInfo.BaseObjectLink");

    #endregion Constructors and parsers

    #region Properties

    public ObjectTypeInfo BaseObjectType {
      get {
        int id = ExtensionData.Get<int>("baseObjectTypeId");

        return ObjectTypeInfo.Parse(id);
      }
    }

    public ObjectTypeInfo LinkedObjectType {
      get {
        int id = ExtensionData.Get<int>("linkedObjectTypeId");

        return ObjectTypeInfo.Parse(id);
      }
    }

    #endregion Properties

    #region Methods

    internal BaseObject ParseBaseObject(int baseObjectId) {
      return BaseObjectType.ParseObject(baseObjectId);
    }


    internal BaseObject ParseBaseObject(string baseObjectUID) {
      Assertion.Require(baseObjectUID, nameof(baseObjectUID));

      return BaseObjectType.ParseObject(baseObjectUID);
    }


    internal BaseObject ParseLinkedObject(int linkedObjectId) {
      return LinkedObjectType.ParseObject(linkedObjectId);
    }


    internal BaseObject ParseLinkedObject(string linkedObjectUID) {
      Assertion.Require(linkedObjectUID, nameof(linkedObjectUID));

      return LinkedObjectType.ParseObject(linkedObjectUID);
    }

    #endregion Methods

  }  // class BaseObjectLinkType

}  // namespace Empiria
