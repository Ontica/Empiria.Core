﻿/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Framework Library      *
*  Namespace : Empiria.Reflection                               Assembly : Empiria.Kernel.dll                *
*  Type      : ObjectFactory                                    Pattern  : Static Class                      *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : This class provides services for a empiria type instance creation.                            *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
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

    static public T ParseObject<T>(int objectId) {
      return (T) ObjectFactory.ParseObject(typeof(T), objectId);
    }

    static public object ParseObject(Type type, int objectId) {
      try {
        MethodInfo method = type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public,
                                           null, CallingConventions.Any, new Type[] { typeof(int) }, null);
        return method.Invoke(null, new object[] { objectId });
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, innerException,
                                      type.FullName + "[ Id = " + objectId + " ]");
      }
    }

    #endregion Public methods

  } // class ObjectFactory

} // namespace Empiria.Reflection