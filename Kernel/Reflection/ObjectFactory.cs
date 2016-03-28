/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Reflection                               Assembly : Empiria.Kernel.dll                *
*  Type      : ObjectFactory                                    Pattern  : Static Class                      *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : This class provides services for a empiria type instance creation.                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
        Assertion.AssertObject(property, "Type {0} doesn't has a static Empty property.", type.FullName);
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
        throw new ReflectionException(ReflectionException.Msg.ObjectPropertyNotFound,
                                      propertyName, type.FullName);
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

    static public bool IsStorable(Type type) {
      return (type.GetInterface("Empiria.IStorable") != null);
    }

    static public bool IsValueObject(Type type) {
      return (type.GetInterface("Empiria.IValueObject`1") != null);
    }

    static public T GetEmptyInstance<T>() {
      return (T) ObjectFactory.InvokeParseMethod(typeof(T), -1);
    }

    static public Type GetType(string typeName) {
      Type type = Type.GetType(typeName);
      if (type == null) {
        string assemblyName = typeName.Substring(0, typeName.LastIndexOf("."));
        Assembly assembly = Assembly.Load(assemblyName);
        type = assembly.GetType(typeName, true, true);
      }
      return type;
    }

    static public bool HasJsonParser(Type type) {
      return (ObjectFactory.GetParseJsonMethod(type) != null);
    }

    static public T InvokeParseMethod<T>(int objectId) {
      return (T) ObjectFactory.InvokeParseMethod(typeof(T), objectId);
    }

    static public T InvokeParseMethod<T>(string value) {
      return (T) ObjectFactory.InvokeParseMethod(typeof(T), value);
    }

    static public T ParseEnumValue<T>(object value) {
      return (T) Enum.Parse(typeof(T), (string) value);
    }

    static public object InvokeParseMethod(Type type, int objectId) {
      try {
        MethodInfo method = ObjectFactory.GetParseMethod(type);
        Assertion.AssertObject(method, "Type {0} doesn't has static Parse(int) method.", type.FullName);
        return method.Invoke(null, new object[] { objectId });
      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);
      } catch (Exception e) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                      type.FullName + "[ Id = " + objectId + " ]");
      }
    }

    static public object InvokeParseMethod(Type type, string value) {
      try {
        MethodInfo method = ObjectFactory.GetParseStringMethod(type);
        Assertion.AssertObject(method, "Type {0} doesn't has static Parse(string) method.", type.FullName);
        return method.Invoke(null, new object[] { value });
      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);
      } catch (Exception e) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                      type.FullName + "[ Value = " + value + " ]");
      }
    }

    static internal T InvokeParseJsonMethod<T>(Json.JsonObject jsonObject) {
      Type type = typeof(T);
      try {
        MethodInfo method = GetParseJsonMethod(type);
        Assertion.AssertObject(method,
                               "Type {0} doesn't has static Parse(JsonObject) method.", type.FullName);
        return (T) method.Invoke(null, new object[] { jsonObject });
      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);
      } catch (Exception e) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                      type.FullName + "JsonObject =  " + jsonObject.ToString());
      }
    }

    #endregion Public methods

    #region Private methods

    static public PropertyInfo GetEmptyInstanceProperty(Type type) {
      return type.GetProperty("Empty", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public);
    }

    static public Delegate GetParseMethodDelegate(Type type) {
      MethodInfo parseMethod = GetParseMethod(type);

      ParameterExpression param = Expression.Parameter(typeof(Int32), "id");

      var body = Expression.Call(parseMethod, param);

      return Expression.Lambda(body, param).Compile();
    }

    static private Delegate GetPropertyGetDelegate(Type type, PropertyInfo property) {
      var body = Expression.Call(property.GetMethod);

      return Expression.Lambda(body).Compile();
    }

    static public MethodInfo GetParseMethod(Type type) {
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public,
                            null, CallingConventions.Any, new Type[] { typeof(int) }, null);
    }

    static private MethodInfo GetParseJsonMethod(Type type) {
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static |
                            BindingFlags.Public | BindingFlags.NonPublic,
                            null, CallingConventions.Any, new Type[] { typeof(Json.JsonObject) }, null);
    }

    static private MethodInfo GetParseStringMethod(Type type) {
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public,
                            null, CallingConventions.Any, new Type[] { typeof(string) }, null);
    }

    #endregion Private methods

  } // class ObjectFactory

} // namespace Empiria.Reflection
