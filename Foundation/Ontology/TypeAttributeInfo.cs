/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : TypeAttributeInfo                                Pattern  : Standard class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Sealed class that represents an ontology type attribute definition.                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Json;

namespace Empiria.Ontology {

  /// <summary>Sealed class that represents an ontology type attribute definition.</summary>
  public sealed class TypeAttributeInfo : TypeRelationInfo {

    #region Fields

    //private int size = 0;
    //private int precision = 0;
    //private int scale = 0;
    //private object minValue = null;
    //private object maxValue = null;
    //private string format = String.Empty;
    //private bool isFixedSize = false;

    #endregion Fields

    #region Constructors and parsers

    private TypeAttributeInfo(MetaModelType sourceType)
      : base(sourceType) {

    }

    static internal TypeAttributeInfo Parse(MetaModelType sourceType, DataRow dataRow) {
      TypeAttributeInfo attributeInfo = new TypeAttributeInfo(sourceType);

      attributeInfo.LoadDataRow(dataRow);

      return attributeInfo;
    }

    #endregion Constructors and parsers

    #region Public properties

    public new object DefaultValue {
      //get { return base.DefaultValue; }
      get;
      set;
    }

    public string Format {
      //get { return attributeData.IsFixedSize; }
      get;
      set;
    }

    public bool IsFixedSize {
      //get { return attributeData.IsFixedSize; }
      get;
      set;
    }

    public object MaxValue {
      //get { return attributeData.IsFixedSize; }
      get;
      set;
    }

    public object MinValue {
      //get { return attributeData.IsFixedSize; }
      get;
      set;
    }

    public int Precision {
      //get { return attributeData.Precision; }
      get;
      set;
    }

    public int Scale {
      //get { return attributeData.IsFixedSize; }
      get;
      set;
    }

    public int Size {
      //get { return attributeData.IsFixedSize; }
      get;
      set;
    }

    #endregion Public properties

    #region Public methods

    protected override object ImplementsConvert(object value) {
      Type targetType = this.TargetType.UnderlyingSystemType;
      Type valueType = value.GetType();

      // When types are equal, conversion not necessary
      if (targetType.Equals(valueType)) {
        return value;
      }
      // Process null values
      if (value == null || value.Equals(System.DBNull.Value)) {
        if (targetType.Equals(typeof(string))) { // Nulls converted to String.Empty if target type is string
          return String.Empty;
        } else { // Nulls converted to default instance value if target != string
          return Activator.CreateInstance(targetType);
        }
      }
      // Process Target.Type == string and value.Type != string
      if (targetType.Equals(typeof(string))) {
        return System.Convert.ToString(value);
      }
      // Process Target.Type != string and value.Type == string
      if (valueType.Equals(typeof(string)) && String.IsNullOrWhiteSpace((string) value)) {
        return Activator.CreateInstance(targetType);
      }
      return System.Convert.ChangeType(value, targetType);
    }

    //private Structure<TypeAttributeInfo> attributeData = new Structure<TypeAttributeInfo>();

    protected override void LoadDataRow(DataRow row) {
      base.LoadDataRow(row);
      var attributes = new {
        Size = 0, Precision = 0, Scale = 0,
        MinValue = String.Empty, MaxValue = String.Empty,
        DisplayFormat = "", IsFixedSize = false
      };

      //opcion 1: Structure.Parse(this, (string) row["TypeRelationExtensionData"]);

      //opcion 2: this.attributeData.Parse((string) row["TypeRelationExtensionData"]);

      //opcion 3: 

      dynamic o = JsonConverter.ToObject((string) row["TypeRelationExtensionData"], attributes);

      this.Size = (int) row["AttributeSize"];
      this.Precision = (int) row["AttributePrecision"];
      this.Scale = (int) row["AttributeScale"];
      this.MinValue = this.ImplementsConvert(row["AttributeMinValue"]);
      this.MaxValue = this.ImplementsConvert(row["AttributeMaxValue"]);
      this.Format = (string) row["AttributeDisplayFormat"];
      this.IsFixedSize = (bool) row["AttributeIsFixedSize"];
    }

    #endregion Public methods

  } // class TypeAttributeInfo

} // namespace Empiria.Ontology
