/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : BaseObjectFactory                                Pattern  : Object Factory                    *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides methods for BaseObject instance creation.                                            *
*                                                                                                            *
********************************* Copyright (c) 2014-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

using System.Data;

namespace Empiria {

  /// <summary>Provides methods for BaseObject instance creation.</summary>
  static public class BaseObjectFactory {

    #region Public methods

    static public T Parse<T>(int id) where T : BaseObject {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstance(id);
    }

    static public T Parse<T>(string key) where T : BaseObject {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstance(key);
    }

    static public T Parse<T>(DataRow row) where T : BaseObject {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstance(row);
    }

    static public List<T> Parse<T>(DataTable table) where T : BaseObject {
      var metamodel = MetaModel<T>.Parse();

      List<T> list = new List<T>(table.Rows.Count);
      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(metamodel.GetInstance(table.Rows[i]));
      }
      return list;
    }

    static public T ParseWithQuery<T>(string query) where T : BaseObject {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.GetInstanceWithQuery(query);
    }

    static public T TryParse<T>(string key) where T : BaseObject {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.TryGetInstance(key);
    }

    static public T TryParseWithQuery<T>(string query) where T : BaseObject {
      var metamodel = MetaModel<T>.Parse();

      return metamodel.TryGetInstanceWithQuery(query);
    }

    #endregion Public methods

  }  // class BaseObjectFactory

} // namespace Empiria
