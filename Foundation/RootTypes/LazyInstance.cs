/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : LazyInstance                                     Pattern  : Lazy Load                         *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Performs lazy loading of BaseObject instances of types.                                       *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Ontology;
using Empiria.Reflection;

namespace Empiria {

  /// <summary>Performs lazy loading of BaseObject instances of types.</summary>
  public class LazyInstance<T> : IIdentifiable where T : BaseObject {

    #region Fields

    private int instanceId = -1;
    private T instance = null;
    private bool isCreated = false;

    #endregion Fields

    #region Constructors and parsers

    private LazyInstance(int id) {
      this.instanceId = id;
    }

    private LazyInstance(T instance) {
      this.instanceId = instance.Id;
      this.instance = instance;
      this.isCreated = true;
    }

    static public LazyInstance<T> Parse(int id) {
      return new LazyInstance<T>(id);
    }

    static private readonly LazyInstance<T> _empty = LazyInstance<T>.Parse(ObjectTypeInfo.EmptyInstanceId);
    static public LazyInstance<T> Empty {
      get {
        return _empty;
      }
    }

    #endregion Constructors and parsers

    #region Properties

    public int Id {
      get {
        return instanceId;
      }
    }

    public T Value {
      get {
        if (!isCreated) {      
          if (this.IsEmptyInstance) {
            instance = ObjectTypeInfo.Parse(typeof(T)).GetEmptyInstance<T>();
          } else {
            instance = BaseObject.ParseId<T>(this.instanceId);
          }
          isCreated = true;
        }
        return instance;
      }
      set {
        if (value == null) {
          Assertion.AssertFail("LazyInstance value cannot be set to null.");
        }
        instanceId = value.Id;
        instance = value;
        isCreated = true;
      }
    }

    public bool IsCreated {
      get {
        return isCreated;
      }
    }

    public bool IsEmptyInstance {
      get {
        return (instanceId == ObjectTypeInfo.EmptyInstanceId);
      }
    }

    #endregion Properties

  } // class LazyInstance

} // namespace Empiria
