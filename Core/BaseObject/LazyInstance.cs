﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Fundamental Types                 *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : LazyInstance                                     Pattern  : Lazy Load                         *
*                                                                                                            *
*  Summary   : Performs lazy loading of BaseObject instances of types.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria {

  /// <summary>Performs lazy loading of BaseObject instances of types.</summary>
  public class LazyInstance<T> where T : BaseObject {

    #region Fields

    private int instanceId = ObjectTypeInfo.EmptyInstanceId;
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

    static public LazyInstance<T> Parse(T instance) {
      Assertion.Require(instance, "instance");

      return new LazyInstance<T>(instance);
    }

    static public LazyInstance<T> Empty {
      get {
        return LazyInstance<T>.Parse(ObjectTypeInfo.EmptyInstanceId);
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
