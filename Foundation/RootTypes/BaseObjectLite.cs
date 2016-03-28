/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : BaseObjectLite                                   Pattern  : Layer Supertype                   *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : BaseObjectLite is the root type of the object type hierarchy in Empiria Lite Framework.       *
*              All object types that uses the framework must be descendants of this abstract type.           *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

    protected bool IsNew {
      get {
        return (this.Id == 0);
      }
    }

    #endregion Properties

    #region Public methods

    /// <summary>Raised after initialization and after databinding if their type is
    /// marked as IsDatabounded.</summary>
    internal protected abstract void OnLoadObjectData(DataRow row);

    #endregion Public methods

  }  // abstract class BaseObjectLite

} // namespace Empiria
