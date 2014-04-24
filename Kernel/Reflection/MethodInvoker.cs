/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Reflection                               Assembly : Empiria.Kernel.dll                *
*  Type      : MethodInvoker                                    Pattern  : Static Class                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Static class that provides services for Empiria method invocation.                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria.Reflection {

  /// <summary>Static class that provides services for Empiria method invocation/// </summary>
  static public class MethodInvoker {

    #region Public methods

    /// <summary>Executes an object instance method with no parameters.</summary>
    static public object Execute(object instance, string methodName) {
      Type type = instance.GetType();
      MethodInfo method = type.GetMethod(methodName, new Type[0]);

      if (method == null) {
        throw new ReflectionException(ReflectionException.Msg.MethodNotFound, type.FullName, methodName);
      }
      try {
        return method.Invoke(instance, null);
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, innerException,
                                      instance.GetType().FullName + "." + methodName);
      }
    }

    /// <summary>Executes an object instance method with the given parameters.</summary>
    static public object Execute(object instance, string methodName, object[] parameters) {
      Type type = instance.GetType();
      MethodInfo method = null;

      if ((parameters == null) || (parameters.Length == 0)) {
        method = type.GetMethod(methodName, new Type[0]);
      } else {
        method = type.GetMethod(methodName, GetTypesArray(parameters));
      }
      if (method == null) {
        throw new ReflectionException(ReflectionException.Msg.MethodNotFound, type.FullName, methodName);
      }
      try {
        return method.Invoke(instance, parameters);
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, innerException,
                                      instance.GetType().FullName + "." + methodName);
      }
    }

    /// <summary>Executes a static method with no parameters.</summary>
    static public object Execute(Type type, string methodName) {
      MethodInfo method = type.GetMethod(methodName, new Type[0]);
      if (method == null) {
        throw new ReflectionException(ReflectionException.Msg.MethodNotFound, type.FullName, methodName);
      }
      try {
        return method.Invoke(null, null);
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, innerException,
                                      type.FullName + "." + methodName);
      }
    }

    /// <summary>Executes a static method with the given parameters.</summary>
    static public object Execute(Type type, string methodName, object[] parameters) {
      MethodInfo method = null;

      if ((parameters == null) || (parameters.Length == 0)) {
        method = type.GetMethod(methodName, new Type[0]);
      } else {
        method = type.GetMethod(methodName, GetTypesArray(parameters));
      }
      if (method == null) {
        throw new ReflectionException(ReflectionException.Msg.MethodNotFound, type.FullName, methodName);
      }
      try {
        return method.Invoke(null, parameters);
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, innerException,
                                      type.FullName + "." + methodName);
      }
    }

    #endregion Public methods

    #region Private methods

    static private Type[] GetTypesArray(object[] objects) {
      Type[] parametersTypes = new Type[objects.Length];

      for (int i = 0; i < objects.Length; i++) {
        parametersTypes[i] = objects[i].GetType();
      }

      return parametersTypes;
    }

    #endregion Private methods

  } // class ObjectFactory

} // namespace Empiria.Reflection