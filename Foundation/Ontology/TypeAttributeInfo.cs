/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : TypeAttributeInfo                                Pattern  : Standard class                    *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Sealed class that represents an ontology type attribute definition.                           *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;

namespace Empiria.Ontology {

  /// <summary>Sealed class that represents an ontology type attribute definition.</summary>
  public sealed class TypeAttributeInfo : TypeRelationInfo {

    #region Fields

    private int size = 0;
    private int precision = 0;
    private int scale = 0;
    private object minValue = null;
    private object maxValue = null;
    private string format = String.Empty;
    private bool isFixedSize = false;

    #endregion Fields

    #region Constructors and parsers

    private TypeAttributeInfo(MetaModelType sourceType)
      : base(sourceType) {

    }

    #endregion Constructors and parsers

    #region Public properties

    public new object DefaultValue {
      get { return base.DefaultValue; }
    }

    public string Format {
      get { return format; }
    }

    public bool IsFixedSize {
      get { return isFixedSize; }
    }

    public object MaxValue {
      get { return maxValue; }
    }

    public object MinValue {
      get { return minValue; }
    }

    public int Precision {
      get { return precision; }
    }

    public int Scale {
      get { return scale; }
    }

    public int Size {
      get { return size; }
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

    protected override void ImplementsLoadObjectData(DataRow row) {
      size = (int) row["AttributeSize"];
      precision = (int) row["AttributePrecision"];
      scale = (int) row["AttributeScale"];
      minValue = this.ImplementsConvert(row["AttributeMinValue"]);
      maxValue = this.ImplementsConvert(row["AttributeMaxValue"]);
      format = (string) row["AttributeDisplayFormat"];
      isFixedSize = (bool) row["AttributeIsFixedSize"];
    }

    #endregion Public methods

  } // class TypeAttributeInfo

} // namespace Empiria.Ontology