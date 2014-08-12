/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : LazyObject                                       Pattern  : Standard Class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Performs lazy loading of IIdentifiable objects of types with static Parse(int) method.        *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Reflection;

namespace Empiria {

  /// <summary>Performs lazy loading of IIdentifiable objects of types with static Parse(int) method.</summary>
  public class LazyObject<T> : IIdentifiable where T : class, IIdentifiable {

    #region Fields

    private int instanceId = -1;
    private T instance = null;
    private bool isCreated = false;

    private object threadSafeObject = new object();
    
    #endregion Fields

    #region Constructors and parsers

    private LazyObject(int id) {
      this.instanceId = id;
    }

    private LazyObject(T instance) {
      this.instanceId = instance.Id;
      this.instance = instance;
      this.isCreated = true;
    }

    static public LazyObject<T> Parse(int id) {
      return new LazyObject<T>(id);
    }

    static public LazyObject<T> Empty {
      get {
        return LazyObject<T>.Parse(-1);
      }
    }

    // User-defined conversion from LazyObject<T> to T
    static public implicit operator T(LazyObject<T> t) {
      Assertion.AssertObject(t, "Assignment right operator");

      return t.Instance;
    }

    // User-defined conversion from T to LazyObject<T>
    static public implicit operator LazyObject<T>(T instance) {
      Assertion.AssertObject(instance, "Assignment right operator");
      
      return new LazyObject<T>(instance);
    }

    #endregion Constructors and parsers

    #region Properties

    public int Id {
      get {
        return instanceId;
      }
    }

    public T Instance {
      get {
        if (!isCreated) {
          lock (threadSafeObject) {
            if (!isCreated) {
              instance = ObjectFactory.ParseObject<T>(this.instanceId);
              isCreated = true;
            }
          } // lock
        }
        return instance;
      }
      set {
        Assertion.AssertObject(value, "Instance set property value");
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
        return (this.Id == -1);
      }
    }

    #endregion Properties

  } // class LazyObject

} // namespace Empiria
