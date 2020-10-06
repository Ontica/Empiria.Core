/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : TypeAttributeInfo                                Pattern  : Standard class                    *
*                                                                                                            *
*  Summary   : Sealed class that represents an ontology type attribute definition.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Json;

namespace Empiria.Ontology {

  /// <summary>Sealed class that represents an ontology type attribute definition.</summary>
  public sealed class TypeAttributeInfo : TypeRelationInfo {

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

    #region Properties

    public new object DefaultValue { get; private set; }

    public string Format { get; private set; } = String.Empty;

    public bool IsFixedSize { get; private set; }

    public object MaxValue { get; private set; }

    public object MinValue { get; private set; }

    public int Precision { get; private set; }

    public int Scale { get; private set; }

    public int Size { get; private set; }

    #endregion Properties

    #region Public methods

    protected override object ImplementsConvert(object value) {
      Type targetType = this.TargetType.UnderlyingSystemType;
      Type valueType = value.GetType();

      // When types are equal, conversion not necessary
      if (targetType.Equals(valueType)) {
        return value;
      }

      // Process null values
      if (value.Equals(DBNull.Value)) {
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


    protected override void LoadDataRow(DataRow row) {
      base.LoadDataRow(row);
      var attributes = new {
        Size = 0, Precision = 0, Scale = 0,
        MinValue = String.Empty, MaxValue = String.Empty,
        DisplayFormat = "", IsFixedSize = false
      };

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
