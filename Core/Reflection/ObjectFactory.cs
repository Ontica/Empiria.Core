/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Reflection                               License  : Please read LICENSE.txt file      *
*  Type      : ObjectFactory                                    Pattern  : Static Class                      *
*                                                                                                            *
*  Summary   : This class provides services for a empiria type instance creation.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Empiria.Json;

namespace Empiria.Reflection {

  static public class ObjectFactory {

    #region Public methods

    static public T Convert<T>(object value) {
      var convertToType = typeof(T);

      if (convertToType == typeof(string)) {
        return (T) (object) System.Convert.ToString(value);

      } else if (convertToType == typeof(int)) {
        return (T) (object) System.Convert.ToInt32(value);

      } else if (convertToType == typeof(bool)) {
        return (T) (object) System.Convert.ToBoolean(value);

      } else if (convertToType == typeof(DateTime)) {
        return (T) (object) System.Convert.ToDateTime(value);

      } else if (convertToType == typeof(decimal)) {
        return (T) (object) System.Convert.ToDecimal(value);

      }

      if (convertToType == value.GetType()) {
        return (T) value;

      } else if (convertToType == typeof(object)) {
        return (T) value;

      } else if (ObjectFactory.IsIdentifiable(convertToType)) {

        if (EmpiriaString.IsInteger(value.ToString())) {
          return ObjectFactory.InvokeParseMethod<T>(System.Convert.ToInt32(value));
        } else {
          return ObjectFactory.InvokeParseMethod<T>((string) value);
        }

      } else if (convertToType.IsEnum) {
        return ObjectFactory.ParseEnumValue<T>(value);

      } else if (convertToType == typeof(string) && value is IDictionary<string, object>) {
        object o = JsonObject.Parse((IDictionary<string, object>) value).ToString();

        return (T) o;

      } else if (convertToType != typeof(string) && value is IDictionary<string, object>) {
        object o = JsonObject.Parse((IDictionary<string, object>) value);

        return JsonConverter.ToObject<T>(o.ToString());

      } else {
        return (T) System.Convert.ChangeType(value, convertToType);
      }
    }

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
      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ConstructorNotDefined, e,
                                      type.FullName);
      } catch (TargetInvocationException e) {
        throw e.InnerException ?? e;
      } catch (Exception innerException) {
        throw new ReflectionException(ReflectionException.Msg.ConstructorNotDefined,
                                      innerException, type.FullName);
      }
    }

    static public object EmptyInstance(Type type) {
      try {
        PropertyInfo property = ObjectFactory.TryGetEmptyInstanceProperty(type);

        Assertion.AssertObject(property, "Type {0} doesn't has a static Empty property.", type.FullName);

        return property.GetMethod.Invoke(null, null);

      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);

      } catch (TargetInvocationException e) {
        throw e.InnerException ?? e;

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
      return (ObjectFactory.TryGetEmptyInstanceProperty(type) != null);
    }

    static public bool HasParseWithIdMethod(Type type) {
      return (ObjectFactory.TryGetParseWithIdMethod(type) != null);
    }

    static public bool HasParseWithStringMethod(Type type) {
      return (ObjectFactory.TryGetParseStringMethod(type) != null);
    }

    static public bool IsConvertible(Type sourceType, Type targetType) {
      try {
        var instanceOfSourceType = Activator.CreateInstance(sourceType);

        System.Convert.ChangeType(instanceOfSourceType, targetType);

        return true;

      } catch {
        return false;
      }
    }

    static public bool IsEmpiriaType(Type type) {
      return type.FullName.StartsWith("Empiria.");
    }

    static public bool IsIdentifiable(Type type) {
      return (type.GetInterface("Empiria.IIdentifiable") != null);
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
      return (ObjectFactory.TryGetParseJsonMethod(type) != null);
    }

    static public T InvokeParseMethod<T>(int objectId) {
      return (T) ObjectFactory.InvokeParseMethod(typeof(T), objectId);
    }

    static public T InvokeParseMethod<T>(string value) {
      return (T) ObjectFactory.InvokeParseMethod(typeof(T), value);
    }

    static public T InvokeStaticMethod<T>(string methodName, string value) {
      return (T) ObjectFactory.InvokeParseMethod(typeof(T), value);
    }

    static public T ParseEnumValue<T>(object value) {
      return (T) Enum.Parse(typeof(T), (string) value);
    }

    static public object InvokeParseMethod(Type type, int objectId) {
      try {
        MethodInfo method = ObjectFactory.TryGetParseWithIdMethod(type);

        Assertion.AssertObject(method, "Type {0} doesn't has static Parse(int) method.", type.FullName);

        return method.Invoke(null, new object[] { objectId });

      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);

      } catch (TargetInvocationException e) {
        throw e.InnerException ?? e;

      } catch (Exception e) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                      type.FullName + "[ Id = " + objectId + " ]");

      }
    }

    static public object InvokeParseMethod(Type type, string value) {
      try {
        MethodInfo method = ObjectFactory.TryGetParseStringMethod(type);

        Assertion.AssertObject(method, "Type {0} doesn't has static Parse(string) method.", type.FullName);

        return method.Invoke(null, new object[] { value });

      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);

      } catch (TargetInvocationException e) {
        throw e.InnerException ?? e;

      } catch (Exception e) {
        throw e;

      }
    }


    static public T InvokeTryParseMethod<T>(Type type, string value) {
      try {
        MethodInfo method = ObjectFactory.TryGetTryParseStringMethod(type);

        Assertion.AssertObject(method, "Type {0} doesn't has static TryParse(string) method.", type.FullName);

        return (T) method.Invoke(null, new object[] { value });

      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);

      } catch (TargetInvocationException e) {
        throw e.InnerException ?? e;

      } catch (Exception e) {
        throw e;
      }
    }


    static internal T InvokeParseJsonMethod<T>(Json.JsonObject jsonObject) {
      Type type = typeof(T);
      try {
        MethodInfo method = TryGetParseJsonMethod(type);

        Assertion.AssertObject(method,
                               "Type {0} doesn't has static Parse(JsonObject) method.", type.FullName);

        return (T) method.Invoke(null, new object[] { jsonObject });

      } catch (TargetException e) {
        throw new ReflectionException(ReflectionException.Msg.ParseMethodNotDefined, e,
                                      type.FullName);

      } catch (TargetInvocationException e) {
        throw e.InnerException ?? e;

      } catch (Exception e) {
        throw new ReflectionException(ReflectionException.Msg.MethodExecutionFails, e,
                                      type.FullName + "JsonObject =  " + jsonObject.ToString());
      }
    }

    #endregion Public methods

    #region Private methods

    static public PropertyInfo TryGetEmptyInstanceProperty(Type type) {
      return type.GetProperty("Empty", BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public);
    }

    static public Delegate GetParseWithIdMethodDelegate(Type type) {
      MethodInfo parseMethod = TryGetParseWithIdMethod(type);

      ParameterExpression param = Expression.Parameter(typeof(Int32), "id");

      var body = Expression.Call(parseMethod, param);

      return Expression.Lambda(body, param).Compile();
    }

    static public Delegate GetParseWithStringMethodDelegate(Type type) {
      MethodInfo parseMethod = TryGetParseStringMethod(type);

      ParameterExpression param = Expression.Parameter(typeof(string), "value");

      var body = Expression.Call(parseMethod, param);

      return Expression.Lambda(body, param).Compile();
    }

    static private Delegate GetPropertyGetDelegate(Type type, PropertyInfo property) {
      var body = Expression.Call(property.GetMethod);

      return Expression.Lambda(body).Compile();
    }

    static public MethodInfo TryGetParseWithIdMethod(Type type) {
      if (!IsEmpiriaType(type)) {
        return null;
      }
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy |
                            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                            null, CallingConventions.Any, new Type[] { typeof(int) }, null);
    }

    static private MethodInfo TryGetParseJsonMethod(Type type) {
      if (!IsEmpiriaType(type)) {
        return null;
      }
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static |
                            BindingFlags.Public | BindingFlags.NonPublic,
                            null, CallingConventions.Any, new Type[] { typeof(Json.JsonObject) }, null);
    }

    static public MethodInfo TryGetParseStringMethod(Type type) {
      if (!IsEmpiriaType(type)) {
        return null;
      }
      return type.GetMethod("Parse", BindingFlags.ExactBinding | BindingFlags.Static |
                            BindingFlags.Public | BindingFlags.NonPublic,
                            null, CallingConventions.Any, new Type[] { typeof(string) }, null);
    }

    static public MethodInfo TryGetTryParseStringMethod(Type type) {
      if (!IsEmpiriaType(type)) {
        return null;
      }
      return type.GetMethod("TryParse", BindingFlags.ExactBinding | BindingFlags.Static |
                            BindingFlags.Public | BindingFlags.NonPublic,
                            null, CallingConventions.Any, new Type[] { typeof(string) }, null);
    }

    #endregion Private methods

  } // class ObjectFactory

} // namespace Empiria.Reflection
