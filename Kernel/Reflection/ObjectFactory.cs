/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Reflection                               Assembly : Empiria.Kernel.dll                *
*  Type      : ObjectFactory                                    Pattern  : Static Class                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This class provides services for a empiria type instance creation.                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Empiria.Reflection {

  static public class ObjectFactory {

    #region Public methods

    static public object CreateObject(Type type) {
      return CreateObject(type, new Type[] { }, new object[] { });
    }
    
    static public T CreateObject<T>() {
      return (T) CreateObject(typeof(T), new Type[] { }, new object[] { });
    }

    static public object CreateObject(Type type, Type[] parametersTypes, object[] parameters) {
      try {
        var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | 
                                              BindingFlags.NonPublic, null, CallingConventions.HasThis,
                                              parametersTypes, null);
        return constructor.Invoke(parameters);
      } catch (TargetInvocationException innerException) {
        throw new ReflectionException(ReflectionException.Msg.ConstructorExecutionFails, innerException,
                                      type.FullName);
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.ConstructorNotDefined,
                                       innerException, type.FullName);
      }
    }

    static public object EmptyInstance(Type type) {
      try {
        PropertyInfo property = ObjectFactory.GetEmptyInstanceProperty(type);
        Assertion.AssertObject(property, String.Format("Type {0} doesn't has a static Empty property.",
                                                       type.FullName));
        return property.GetMethod.Invoke(null, null);
      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);
      } catch (Exception e) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                      type.FullName);
      }
    }

    static public object GetPropertyValue(object instance, string propertyName) {
      Type type = instance.GetType();
      PropertyInfo propertyInfo = type.GetProperty(propertyName);

      if (propertyInfo != null) {
        return propertyInfo.GetValue(instance, null);
      } else {
        throw new ReflectionException(ReflectionException.Msg.ObjectPropertyNotFound, propertyName, type.FullName);
      }
    }

    static public Type GetType(string assemblyName, string typeName) {
      Type type = Type.GetType(typeName);
      if (type == null) {
        Assembly assembly = Assembly.Load(assemblyName);
        type = assembly.GetType(typeName, true, true);
      }
      return type;
    }

    static public bool HasEmptyInstance(Type type) {
      return (ObjectFactory.GetEmptyInstanceProperty(type) != null);
    }

    static public bool HasParseWithIdMethod(Type type) {
      return (ObjectFactory.GetParseMethod(type) != null);
    }
    
    static public bool IsConvertible(Type sourceType, Type targetType) {
      try {
        var instanceOfSourceType = Activator.CreateInstance(sourceType);
        Convert.ChangeType(instanceOfSourceType, targetType);
        return true;
      } catch {
        return false;
      }
    }
     
    static public bool IsLazy(Type type) {
      return (type.IsGenericType && 
              type.GetGenericTypeDefinition() == typeof(LazyObject<>));
    }

    static public bool IsStorable(Type type) {
      return (type.GetInterface("Empiria.IStorable") != null);
    }

    static public bool IsValueObject(Type type) {
      return (type.GetInterface("Empiria.IValueObject`1") != null);
    }

    static public object LazyEmptyObject(Type type) {
      Type genericLazy = typeof(LazyObject<>).MakeGenericType(type);
      PropertyInfo property = genericLazy.GetProperty("Empty", BindingFlags.Static | BindingFlags.Public);

      return property.GetMethod.Invoke(null, null);
    }

    static public T InvokeParseMethod<T>(int objectId) {
      return (T) ObjectFactory.InvokeParseMethod(typeof(T), objectId);
    }

    static public T InvokeParseMethod<T>(string value) {
      return (T) ObjectFactory.InvokeParseMethod(typeof(T), value);
    }

    static private Dictionary<int, Delegate> _parseObjectMethods =
                                                new Dictionary<int, Delegate>();
    static public object InvokeParseMethod(Type type, int objectId) {
      int typeId = type.GetHashCode();
      if (!_parseObjectMethods.ContainsKey(typeId)) {
        lock (_parseObjectMethods) {
          if (!_parseObjectMethods.ContainsKey(typeId)) {
            try {
              Delegate method = ObjectFactory.GetParseMethodDelegate(type);
              Assertion.AssertObject(method, String.Format("Type {0} doesn't has static Parse(int) method.",
                                                           type.FullName));
              _parseObjectMethods.Add(typeId, method);
            } catch (TargetException e) {
              throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                            type.FullName + "[ Id = " + objectId + " ]");
            } catch (Exception e) {
              throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                            type.FullName + "[ Id = " + objectId + " ]");
            } // try
          }
        } // lock
      }
      return _parseObjectMethods[typeId].DynamicInvoke(objectId);
    }

    static private Dictionary<string, MethodInfo> _parseValueObjectMethods =
                                               new Dictionary<string, MethodInfo>();
    static public object InvokeParseMethod(Type type, string value) {
      if (!_parseValueObjectMethods.ContainsKey(type.FullName)) {
        lock (_parseValueObjectMethods) {
          if (!_parseValueObjectMethods.ContainsKey(type.FullName)) {
            try {
              MethodInfo method = ObjectFactory.GetParseStringMethod(type);
              Assertion.AssertObject(method, String.Format("Type {0} doesn't has static Parse(string) method.",
                                                           type.FullName));
              _parseValueObjectMethods.Add(type.FullName, method);
            } catch (TargetException e) {
              throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                            type.FullName + "[ Value = " + value + " ]");
            } catch (Exception e) {
              throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                            type.FullName + "[ Value = " + value + " ]");
            }  // try
          }
        } // lock
      }
      return _parseValueObjectMethods[type.FullName].Invoke(null, new object[] { value });
    }

    #endregion Public methods

    #region Private methods

    static private PropertyInfo GetEmptyInstanceProperty(Type type) {
      return type.GetProperty("Empty", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public);
    }

    static private Delegate GetParseMethodDelegate(Type type) {
      MethodInfo parseMethod = GetParseMethod(type);

      ParameterExpression param = Expression.Parameter(typeof(Int32), "id");

      var body = Expression.Call(parseMethod, param);

      return Expression.Lambda(body, param).Compile();
    }

    static private MethodInfo GetParseMethod(Type type) {
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public,
                            null, CallingConventions.Any, new Type[] { typeof(int) }, null);
    }

    static private MethodInfo GetParseStringMethod(Type type) {
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public,
                            null, CallingConventions.Any, new Type[] { typeof(string) }, null);
    }

    #endregion Private methods

  } // class ObjectFactory

} // namespace Empiria.Reflection
