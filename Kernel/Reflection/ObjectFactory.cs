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
using System.Reflection;

namespace Empiria.Reflection {

  static public class ObjectFactory {

    #region Public methods

    static public object CreateObject(Type objectType) {
      return CreateObject(objectType, new Type[] { }, new object[] { });
    }

    static public T CreateObject<T>() {
      return (T) CreateObject(typeof(T), new Type[] { }, new object[] { });
    }

    static public object CreateObject(Type objectType, Type[] parametersTypes, object[] parameters) {
      try {
        ConstructorInfo constructor = objectType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                                                BindingFlags.NonPublic,
                                                                null, CallingConventions.HasThis,
                                                                parametersTypes, null);
        return constructor.Invoke(parameters);
      } catch (TargetInvocationException innerException) {
        throw new ReflectionException(ReflectionException.Msg.ConstructorExecutionFails, innerException,
                                      objectType.FullName);
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.ConstructorNotDefined,
                                       innerException, objectType.FullName);
      }
    }

    static public T CreateObject<T>(Type[] parametersTypes, object[] parameters) {
      try {
        ConstructorInfo constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                                               BindingFlags.NonPublic,
                                                               null, CallingConventions.HasThis,
                                                               parametersTypes, null);
        return (T) constructor.Invoke(parameters);
      } catch (TargetInvocationException innerException) {
        throw new ReflectionException(ReflectionException.Msg.ConstructorExecutionFails, innerException,
                                      typeof(T).FullName);
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.ConstructorNotDefined,
                                       innerException, typeof(T).FullName);
      }
    }

    static public object EmptyInstance(Type type) {
      try {
        PropertyInfo property = ObjectFactory.GetEmptyInstanceProperty(type);
        Assertion.AssertObject(property, "Type " + type.FullName + " doesn't has a static Empty property.");
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
     
    static public bool IsLazy(Type type) {
      return (type.IsGenericType && 
              type.GetGenericTypeDefinition() == typeof(LazyObject<>));
    }

    static public bool IsStorable(Type type) {
      return (type.GetInterface("Empiria.IStorable") != null);
    }

    static public object LazyEmptyObject(Type type) {
      Type genericLazy = typeof(LazyObject<>).MakeGenericType(type);
      PropertyInfo property = genericLazy.GetProperty("Empty", BindingFlags.Static | BindingFlags.Public);

      return property.GetMethod.Invoke(null, null);
    }

    static public T ParseObject<T>(int objectId) {
      return (T) ObjectFactory.ParseObject(typeof(T), objectId);
    }

    static public object ParseObject(Type type, int objectId) {
      try {
        MethodInfo method = ObjectFactory.GetParseMethod(type);        
        Assertion.AssertObject(method, "Type " + type.FullName + " doesn't has static Parse(int) method.");
        return method.Invoke(null, new object[] { objectId });
      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName + "[ Id = " + objectId + " ]");    
      } catch (Exception e) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                      type.FullName + "[ Id = " + objectId + " ]");
      }
    }

    static private PropertyInfo GetEmptyInstanceProperty(Type type) {
      return type.GetProperty("Empty", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public);
    }

    static private MethodInfo GetParseMethod(Type type) {
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public,
                            null, CallingConventions.Any, new Type[] { typeof(int) }, null);
    }

    #endregion Public methods

  } // class ObjectFactory

} // namespace Empiria.Reflection
