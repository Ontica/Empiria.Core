/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : BaseObjectCache                                  Pattern  : Cache Collection Class            *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Internal sealed class that represents a cached collection of BaseObject instances.            *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using Empiria.Collections;

namespace Empiria {

  /// <summary>Internal sealed class that represents a cached collection of BaseObject instances.</summary>
  internal sealed class BaseObjectCache : CachedList<BaseObject> {

    #region Fields

    static private readonly int objectCacheSize = ConfigurationData.GetInteger("Empiria.Ontology", "ObjectCache.Size");

    #endregion Fields

    #region Constructors and parsers

    internal BaseObjectCache()
      : base(objectCacheSize) {

    }

    #endregion Constructors and parsers

    #region Public methods

    public new BaseObject GetItem(string typeName, int id) {
      if (!base.Contains(typeName, id)) {
        return null;
      }
      return base.GetItem(typeName, id);
    }

    public new BaseObject GetItem(string typeName, string namedKey) {
      if (!base.Contains(typeName, namedKey)) {
        return null;
      }
      return base.GetItem(typeName, namedKey);
    }

    #endregion Public methods

    #region Internal methods

    internal new void Clear() {
      base.Clear();
    }

    internal void Insert(BaseObject item) {
      string typeInfoName = item.ObjectTypeInfo.Name;

      while (true) {
        if (typeInfoName.LastIndexOf('.') > 0) {
          base.Insert(typeInfoName, item);
          typeInfoName = typeInfoName.Substring(0, typeInfoName.LastIndexOf('.'));
        } else {
          break;
        }
      }
    }

    internal void Insert(BaseObject item, string namedKey) {
      string typeInfoName = item.ObjectTypeInfo.Name;

      while (true) {
        if (typeInfoName.LastIndexOf('.') > 0) {
          base.Insert(typeInfoName, namedKey, item);
          typeInfoName = typeInfoName.Substring(0, typeInfoName.LastIndexOf('.'));
        } else {
          break;
        }
      }
    }

    #endregion Internal methods

  } // class BaseObjectCache

} // namespace Empiria
