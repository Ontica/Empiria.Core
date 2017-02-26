/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : BaseObjectLite                                   Pattern  : Layer Supertype                   *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : BaseObjectLite is the root type of the object type hierarchy in Empiria Lite Framework.       *
*              All object types that uses the framework must be descendants of this abstract type.           *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria {

  public abstract class BaseObjectLite : IIdentifiable {

    #region Constructors and parsers

    protected BaseObjectLite() {

    }

    #endregion Constructors and parsers

    #region Properties

    public int Id {
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
      Assertion.AssertObject(obj, "obj");

      return !this.Equals(obj);
    }

    public override bool Equals(object obj) {
      if (obj == null || this.GetType() != obj.GetType()) {
        return false;
      }
      return base.Equals(obj) && (this.Id == ((BaseObjectLite) obj).Id);
    }

    public bool Equals(BaseObjectLite obj) {
      if (obj == null) {
        return false;
      }
      return (this.GetType() == obj.GetType()) && (this.Id == obj.Id);
    }

    public override int GetHashCode() {
      return (this.GetType().GetHashCode() ^ this.Id);
    }

    /// <summary>Raised after initialization and after databinding if their type is
    /// marked as IsDatabounded.</summary>
    internal protected abstract void OnLoadObjectData(DataRow row);

    #endregion Public methods

  }  // abstract class BaseObjectLite

} // namespace Empiria
