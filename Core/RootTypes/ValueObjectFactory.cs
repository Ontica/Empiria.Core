﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ValueObjectFactory                               Pattern  : Object Factory                    *
*                                                                                                            *
*  Summary   : Provides methods for IValueObject instance creation.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;
using Empiria.Reflection;

namespace Empiria {

  /// <summary>Provides methods for IValueObject instance creation.</summary>
  static public class ValueObjectFactory {

    #region Public methods

    public static FixedList<T> GetFixedList<T>(string sql) where T : IValueObject {
      throw new NotImplementedException();

      //var metamodel = MetaModel<T>.Parse();

      //return metamodel.GetFixedList(sql);
    }

    static public T Parse<T>(string value) where T : IValueObject {
      return (T) GetValueObject(typeof(T), value, true);
    }

    static public T TryParse<T>(string value) where T : IValueObject {
      return (T) GetValueObject(typeof(T), value, false);
    }

    #endregion Public methods

    private static IValueObject GetValueObject(Type type, string value, bool onlyIfRegistered) {
      bool registered = IsRegistered(type, value);

      if (onlyIfRegistered && !registered) {
        Assertion.AssertFail("Unrecognized value '{0}' for value type '{1}'.", value, type.Name);
      }
      return (IValueObject) ObjectFactory.CreateObject(type, new Type[] { typeof(string), typeof(bool) },
                                                             new object[] { value, registered });
    }

    private static bool IsRegistered(Type type, string value) {
      var sql = "SELECT ItemValue FROM Catalogues " +
                "WHERE ItemType = '{0}' AND ItemValue = '{1}'";

      sql = String.Format(sql, type.Name, value);

      return !DataReader.IsEmpty(DataOperation.Parse(sql));
    }


  }  // class ValueObjectFactory

} // namespace Empiria
