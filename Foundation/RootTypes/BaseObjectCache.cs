/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : BaseObjectCache                                  Pattern  : Cache Collection Class            *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Internal sealed class that represents a cached collection of BaseObject instances.            *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using Empiria.Collections;

namespace Empiria {

  /// <summary>Internal sealed class that represents a cached collection of BaseObject instances.</summary>
  internal sealed class BaseObjectCache : CachedList<BaseObject> {

    #region Fields

    static private readonly int objectCacheSize = 
                                      ConfigurationData.GetInteger("Empiria.Ontology", "ObjectCache.Size");

    #endregion Fields

    #region Constructors and parsers

    internal BaseObjectCache() : base(objectCacheSize) {

    }

    #endregion Constructors and parsers

    #region Public methods

    internal T GetItem<T>(string typeName, int id) where T : BaseObject {
      return (T) base.GetItem(typeName, id);
    }

    public T GetItem<T>(string typeName, string namedKey) where T : BaseObject {
      return (T) base.GetItem(typeName, namedKey);
    }

    #endregion Public methods

    #region Internal methods

    internal new void Clear() {
      base.Clear();
    }

    internal void Insert(BaseObject item) {
      string typeInfoName = item.ObjectTypeInfo.Name;

      // Empty and Unknown instances are unique per type, so don't create their base objects inside
      // the inheritance hierarchy.
      // Example: ReadItem.Book.Fiction.Empty generates only one item in the cache
      // ('-1.ReadItem.Book.Fiction'), but not '-1.ReadItem.Book' That's because 
      // ReadItem.Book.Empty could be defined and also it will use the same id but for its own type: 
      // '-1.ReadItem.Book' of type Book. SpecialCase instances are unique and static per type
      if (item.IsSpecialCase) {
        base.Insert(typeInfoName, item);
        return;
      }

      // Stores item object into the objects cache and additionally insert it again for each
      // parent class defined on that item's inheritance hierarchy.
      // Example: An object with typeInfoName = 'ReadItem.Book.Fiction' and Id = 1001 generates
      // three items in the cache: '1001.ReadItem.Book.Fiction', '1001.ReadItem.Book' and '1001.ReadItem'
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
