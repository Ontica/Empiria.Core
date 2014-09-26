/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Reflection                               Assembly : Empiria.Kernel.dll                *
*  Type      : MethodInvoker                                    Pattern  : Static Class                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Static class that provides services for Empiria method invocation.                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;
using System.Reflection.Emit;

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

    static public object ExecuteStaticProperty(Type type, string propertyName) {
      PropertyInfo property = GetStaticProperty(type, propertyName);

      return property.GetMethod.Invoke(null, null);
    }

    static public Func<object, object> GetPropertyValueMethodDelegate(PropertyInfo propertyInfo) {
      var dynMethod = new DynamicMethod("_get_" + propertyInfo.Name, typeof(object),
                                        new Type[] { typeof(object) },
                                        propertyInfo.DeclaringType.Module, true);

      // Generate the intermediate language.
      ILGenerator codeGenerator = dynMethod.GetILGenerator();
      codeGenerator.Emit(OpCodes.Ldarg_0);
      codeGenerator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
      codeGenerator.Emit(OpCodes.Call, propertyInfo.GetMethod);
      if (propertyInfo.GetMethod.ReturnType.IsValueType) {
        codeGenerator.Emit(OpCodes.Box, propertyInfo.GetMethod.ReturnType);
      }
      codeGenerator.Emit(OpCodes.Ret);

      return (Func<object, object>) dynMethod.CreateDelegate(typeof(Func<object, object>));
    }

    static public Action<object, object> SetPropertyValueMethodDelegate(PropertyInfo propertyInfo) {
      var argumentTypes = new Type[] { typeof(object), typeof(object) };

      var dynMethod = new DynamicMethod("_set_" + propertyInfo.Name, null, argumentTypes,
                                        propertyInfo.DeclaringType.Module, true);

      // Generate the intermediate language.
      ILGenerator codeGenerator = dynMethod.GetILGenerator();
      codeGenerator.Emit(OpCodes.Ldarg_0);
      codeGenerator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
      codeGenerator.Emit(OpCodes.Ldarg_1);
      if (propertyInfo.PropertyType.IsValueType) {
        codeGenerator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
      } else {
        codeGenerator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
      }
      codeGenerator.Emit(OpCodes.Callvirt, propertyInfo.SetMethod);
      codeGenerator.Emit(OpCodes.Ret);

      return (Action<object, object>) dynMethod.CreateDelegate(typeof(Action<object, object>));
    }

    static public Action<object, object> GetFieldValueSetMethodDelegate(FieldInfo fieldInfo) {
      var argumentTypes = new Type[] { typeof(object), typeof(object) };

      var dynMethod = new DynamicMethod("_setField_" + fieldInfo.Name, null, argumentTypes,
                                        fieldInfo.DeclaringType.Module, true);

      // Generate the intermediate language.
      ILGenerator codeGenerator = dynMethod.GetILGenerator();
      codeGenerator.Emit(OpCodes.Ldarg_0);
      codeGenerator.Emit(OpCodes.Castclass, fieldInfo.DeclaringType);
      codeGenerator.Emit(OpCodes.Ldarg_1);
      if (fieldInfo.FieldType.IsValueType) {
        codeGenerator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType); // unbox the value parameter to the value-type
      } else {
        codeGenerator.Emit(OpCodes.Castclass, fieldInfo.FieldType); // cast the value on the stack to the field type
      }
      codeGenerator.Emit(OpCodes.Stfld, fieldInfo);
      codeGenerator.Emit(OpCodes.Ret);

      return (Action<object, object>) dynMethod.CreateDelegate(typeof(Action<object, object>));
    }

    static public Func<object> GetStaticPropertyValueMethodDelegate(PropertyInfo propertyInfo) {
      var dynMethod = new DynamicMethod("_get_" + propertyInfo.Name, typeof(object),
                                        null, propertyInfo.DeclaringType.Module, true);

      // Generate the intermediate language.
      ILGenerator codeGenerator = dynMethod.GetILGenerator();
      codeGenerator.Emit(OpCodes.Call, propertyInfo.GetMethod);
      if (propertyInfo.GetMethod.ReturnType.IsValueType) {
        codeGenerator.Emit(OpCodes.Box, propertyInfo.GetMethod.ReturnType);
      }
      codeGenerator.Emit(OpCodes.Ret);

      return (Func<object>) dynMethod.CreateDelegate(typeof(Func<object>));
    }

    static public PropertyInfo GetStaticProperty(Type type, string propertyName) {
      Assertion.AssertObject(type, "type");
      Assertion.AssertObject(type, "propertyName");

      PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public);
      Assertion.AssertObject(property, String.Format("Type {0} doesn't has a static property with name '{1}'.",
                                                     type.FullName, propertyName));
      return property;
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
