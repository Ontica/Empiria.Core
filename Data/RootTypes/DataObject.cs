/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Modeler Library              *
*  Namespace : Empiria.Data.Modeler                             Assembly : Empiria.Data.dll                  *
*  Type      : DataObject                                       Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This type represents a data object metadata.                                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Data.Modeler {

  public enum DataObjectType {
    Undefined = 'U',
    Table = 'T',
    View = 'V',
    Query = 'Q',
    Field = 'F',
    Parameter = 'P'
  }

  public sealed class DataObject : IIdentifiable {

    #region Fields

    private int id = 0;
    private DataObjectType dataObjectType = DataObjectType.Undefined;
    private string name = string.Empty;
    private DataSource dataSource = DataSource.Default;
    private string documentation = String.Empty;
    private string queryString = String.Empty;
    private string fieldParameterName = String.Empty;
    private int fieldParameterIndex = -1;
    private int fieldParameterType = -1;
    private bool fieldParameterIsNullable = false;
    private int fieldParameterDirection = -1;
    private int fieldParameterSize = -1;
    private int fieldParameterScale = -1;
    private int fieldParameterPrecision = -1;
    private object fieldParameterDefaultValue = String.Empty;
    private DataObject parent = null;

    #endregion Fields

    #region Constructors and parsers

    private DataObject() {
      // instance creation of this class only allowed using the Build static internal method.
    }

    #endregion Constructors and parsers

    #region Public properties

    public int Id {
      get { return id; }
    }

    public DataObjectType DataObjectType {
      get { return dataObjectType; }
      set { dataObjectType = value; }
    }

    public string Name {
      get { return name; }
      set { name = value; }
    }

    internal DataSource DataSource {
      get { return dataSource; }
      set { dataSource = value; }
    }

    public string Documentation {
      get { return documentation; }
      set { documentation = value; }
    }

    public string QueryString {
      get { return queryString; }
      set { queryString = value; }
    }

    public string FieldParameterName {
      get { return fieldParameterName; }
      set { fieldParameterName = value; }
    }

    public int FieldParameterIndex {
      get { return fieldParameterIndex; }
      set { fieldParameterIndex = value; }
    }

    public int FieldParameterType {
      get { return fieldParameterType; }
      set { fieldParameterType = value; }
    }

    public bool FieldParameterIsNullable {
      get { return fieldParameterIsNullable; }
      set { fieldParameterIsNullable = value; }
    }

    public int FieldParameterDirection {
      get { return fieldParameterDirection; }
      set { fieldParameterDirection = value; }
    }

    public int FieldParameterSize {
      get { return fieldParameterSize; }
      set { fieldParameterSize = value; }
    }

    public int FieldParameterScale {
      get { return fieldParameterScale; }
      set { fieldParameterScale = value; }
    }

    public int FieldParameterPrecision {
      get { return fieldParameterPrecision; }
      set { fieldParameterPrecision = value; }
    }

    public object FieldParameterDefaultValue {
      get { return fieldParameterDefaultValue; }
      set { fieldParameterDefaultValue = value; }
    }

    public DataObject Parent {
      get { return parent; }
      set { parent = value; }
    }

    #endregion Public properties

  } // class DataObject

} // namespace Empiria.Data.Modeler