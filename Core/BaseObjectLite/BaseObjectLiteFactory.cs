/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Base Objects                                 Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : BaseObjectLiteFactory                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides public static methods for Empiria Lite Framework BaseObjectLite objects.              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Data;

using Empiria.Data;

namespace Empiria {

  /// <summary>Provides methods for BaseObjectLite instance creation.</summary>
  static public class BaseObjectLiteFactory {

    #region Public methods

    static public FixedList<T> GetFixedList<T>(string sql) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.GetFixedList(sql);
    }


    static public int GetNextId<T>() where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.GetNextInstanceId();
    }


    static public T Parse<T>(int id) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.GetInstance(id);
    }


    static public T Parse<T>(DataOperation dataOperation) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.GetInstance(dataOperation);
    }


    static public T Parse<T>(string key) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.GetInstance(key);
    }


    static public T Parse<T>(DataRow row) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.GetInstance(row);
    }


    static public List<T> Parse<T>(DataTable table) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      List<T> list = new List<T>(table.Rows.Count);
      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(metamodel.GetInstance(table.Rows[i]));
      }
      return list;
    }


    static public T ParseEmptyInstance<T>() where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.GetInstance(-1);
    }


    static public T ParseWithFilter<T>(string filter) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.GetInstanceWithQuery(filter);
    }


    static public T TryParse<T>(int id) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.TryGetInstance(id);
    }


    static public T TryParse<T>(string key) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.TryGetInstance(key);
    }


    static public T TryParseWithFilter<T>(string filter) where T : BaseObjectLite {
      var metamodel = BaseObjectLiteMetaModel<T>.Parse();

      return metamodel.TryGetInstanceWithFilter(filter);
    }

    #endregion Public methods

  }  // class BaseObjectLiteFactory

} // namespace Empiria
