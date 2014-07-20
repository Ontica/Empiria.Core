using System;
using System.Collections.Generic;

namespace Empiria.Data.Modeler {

  public class DbQueryParameter {

    #region Constructors and parsers

    private DbQueryParameter(string queryName) {
      this.QueryName = queryName;
      this.Index = 0;
      this.Name = string.Empty;
      this.SqlDbType = 0;
      this.Direction = 0;
      this.Size = 0;
      this.Scale = 0;
      this.Precision = 0;
      this.DefaultValue = string.Empty;
    }

    static public DbQueryParameter Parse(string queryName){
      var queryParameter = new DbQueryParameter(queryName);
      return queryParameter;
    }

    #endregion Constructors and parsers

    #region Public properties

    public string QueryName {
      get;
      private set;
    }

    public int Index {
      get;
      internal set;

    }

    public string Name {
      get;
      internal set;
    }

    public int SqlDbType {
      get;
      internal set;
    }

    public int Direction {
      get;
      internal set;
    }

    public int Size {
      get;
      internal set;
    }

    public int Scale {
      get;
      internal set;
    }

    public int Precision {
      get;
      internal set;
    }

    public string DefaultValue {
      get;
      internal set;
    }

    #endregion Public properties

  }
}
