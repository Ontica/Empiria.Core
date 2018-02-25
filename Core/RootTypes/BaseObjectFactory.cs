/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : BaseObjectFactory                                Pattern  : Object Factory                    *
*                                                                                                            *
*  Summary   : Provides methods for BaseObject instance creation.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Collections.Generic;
using System.Data;

using Empiria.Data;

namespace Empiria {

  /// <summary>Provides methods for BaseObject instance creation.</summary>
  static public class BaseObjectFactory {

    #region Public methods

    public static FixedList<T> GetFixedList<T>(string sql) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetFixedList(sql);
    }

    public static int GetNextId<T>() where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetNextInstanceId();
    }

    static public T Parse<T>(int id) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstance(id);
    }

    static public T Parse<T>(DataOperation dataOperation) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstance(dataOperation);
    }

    static public T Parse<T>(string key) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstance(key);
    }

    static public T Parse<T>(DataRow row) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstance(row);
    }

    static public List<T> Parse<T>(DataTable table) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      List<T> list = new List<T>(table.Rows.Count);
      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(metamodel.GetInstance(table.Rows[i]));
      }
      return list;
    }

    public static T ParseEmptyInstance<T>() where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstance(-1);
    }

    static public T ParseWithFilter<T>(string filter) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstanceWithQuery(filter);
    }

    static public T TryParse<T>(int id) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.TryGetInstance(id);
    }

    static public T TryParse<T>(string key) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.TryGetInstance(key);
    }

    static public T TryParseWithFilter<T>(string filter) where T : BaseObjectLite {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.TryGetInstanceWithFilter(filter);
    }

    #endregion Public methods

  }  // class BaseObjectFactory

} // namespace Empiria
