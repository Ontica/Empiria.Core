/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : CachedObject                                     Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This type represents an object that can be cached using the wrapper type DataCache.           *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Web.Caching;

namespace Empiria.Data {

  public sealed class CachedObject : IDisposable {

    #region Fields

    DataOperation dataOperation = null;
    string tableName = null;
    private CacheDependency dependencies = null;
    private DateTime absoluteExpiration = Cache.NoAbsoluteExpiration;
    private TimeSpan slidingExpiration = Cache.NoSlidingExpiration;
    private CacheItemPriority priority = CacheItemPriority.Default;
    private object cachedValue = null;
    private ReloadDataCallback reloadCallback = null;

    #endregion Fields

    #region Constructors and parsers

    private CachedObject() {
      // instance creation of this class only allowed using the Build static internal method.
    }

    static internal CachedObject Parse(DataOperation dataOperation, string tableName,
                                       object value, string[] dependencies,
                                       DateTime absoluteExpiration, TimeSpan slidingExpiration,
                                       CacheItemPriority priority, ReloadDataCallback callback) {
      CachedObject cachedObject = new CachedObject();

      cachedObject.dataOperation = dataOperation;
      cachedObject.tableName = tableName;
      cachedObject.cachedValue = value;
      if (dependencies != null) {
        cachedObject.dependencies = new CacheDependency(null, dependencies);
      } else {
        cachedObject.dependencies = null;
      }
      cachedObject.absoluteExpiration = absoluteExpiration;
      cachedObject.slidingExpiration = slidingExpiration;
      cachedObject.priority = priority;
      cachedObject.reloadCallback = callback;

      return cachedObject;
    }

    #endregion Constructors and parsers

    #region Public properties

    public object CachedValue {
      get { return cachedValue; }
      set { cachedValue = value; }
    }

    public DataOperation DataOperation {
      get { return dataOperation; }
    }

    public CacheDependency Dependencies {
      get { return dependencies; }
    }

    public DateTime AbsoluteExpiration {
      get { return absoluteExpiration; }
    }

    public CacheItemPriority Priority {
      get { return priority; }
    }

    public ReloadDataCallback ReloadCallback {
      get { return reloadCallback; }
    }

    public TimeSpan SlidingExpiration {
      get { return slidingExpiration; }
    }

    public string TableName {
      get { return tableName; }
    }

    public bool UseSlidingExpiration {
      get { return (absoluteExpiration == Cache.NoAbsoluteExpiration); }
    }

    #endregion Public properties

    #region IDisposable Members

    void IDisposable.Dispose() {
      dependencies.Dispose();
    }

    #endregion IDisposable Members

  } // class CachedObject

} // namespace Empiria.Data