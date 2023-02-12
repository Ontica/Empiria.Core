/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information holder                    *
*  Type     : Feature                                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds information about a system feature.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Items {

  /// <summary>Holds information about a system feature.</summary>
  internal class Feature : SecurityItem {

    #region Constructors and parsers

    private Feature(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal new Feature Parse(int id) {
      return BaseObject.ParseId<Feature>(id);
    }


    static internal Feature Parse(string featureKey) {
      var feature = BaseObject.TryParse<Feature>($"SecurityItemKey = '{featureKey}'");

      if (feature != null) {
        return feature;
      }

      return Feature.Empty;
    }


    static internal FixedList<Feature> GetList(ClientApplication app) {
      return SecurityItemsDataReader.GetContextItems<Feature>(app, SecurityItemType.ClientAppFeature);
    }


    static internal FixedList<Feature> GetList(ClientApplication app, IIdentifiable subject) {
      return SecurityItemsDataReader.GetSubjectTargetItems<Feature>(app, subject,
                                                                    SecurityItemType.SubjectFeature);
    }

    public static Feature Empty => ParseEmpty<Feature>();

    #endregion Constructors and parsers

    #region Properties

    public string Key {
      get {
        return base.BaseKey;
      }
    }


    public string Name {
      get {
        return ExtensionData.Get("featureName", this.Key);
      }
    }


    public bool IsAssignable {
      get {
        return ExtensionData.Get("assignable", true);
      }
    }


    public ObjectAccessRule[] ObjectsGrants {
      get {
        return ExtensionData.GetList<ObjectAccessRule>("objectsGrants", false)
                            .ToArray();
      }
    }


    public Feature[] Requires {
      get {
        return ExtensionData.GetList<Feature>("requires", false)
                            .ToArray();
      }
    }

    #endregion Properties

  }  // class Feature

}  // namespace Empiria.Security.Items
