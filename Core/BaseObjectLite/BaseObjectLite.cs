/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Base Objects                                 Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : BaseObjectLite                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : BaseObjectLite is the root type of the object type hierarchy in Empiria Lite Framework.        *
*             All object types that uses the framework must be descendants of this abstract type.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

namespace Empiria {

  public abstract class BaseObjectLite : IIdentifiable {

    #region Constructors and parsers

    protected BaseObjectLite() {
      // no-op
    }

    #endregion Constructors and parsers

    #region Properties

    public int Id {
      get;
      protected internal set;
    }


    public virtual string UID {
      get;
      protected internal set;
    }


    [Newtonsoft.Json.JsonIgnore]
    public bool IsEmptyInstance {
      get {
        return (this.Id == -1);
      }
    }

    protected bool IsNew {
      get {
        return (this.Id == 0);
      }
    }

    #endregion Properties

    #region Public methods

    public bool Distinct(BaseObjectLite obj) {
      Assertion.Require(obj, "obj");

      return !this.Equals(obj);
    }


    public override bool Equals(object obj) => this.Equals(obj as BaseObjectLite);

    public bool Equals(BaseObjectLite obj) {
      if (obj == null) {
        return false;
      }
      if (Object.ReferenceEquals(this, obj)) {
        return true;
      }
      if (this.GetType() != obj.GetType()) {
        return false;
      }

      return (this.Id == obj.Id);
    }


    public override int GetHashCode() {
      return (this.GetType().GetHashCode(), this.Id).GetHashCode();
    }

    /// <summary>Raised after initialization and after databinding if their type is
    /// marked as IsDatabounded.</summary>
    internal protected abstract void OnLoadObjectData(DataRow row);

    #endregion Public methods

  }  // abstract class BaseObjectLite

} // namespace Empiria
