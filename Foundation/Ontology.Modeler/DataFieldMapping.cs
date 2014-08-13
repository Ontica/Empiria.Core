/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology.Modeler                         Assembly : Empiria.dll                       *
*  Type      : DataFieldMapping                                 Pattern  : Standard class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Mapping rule between a type field and a data source element.                                  *
*                                                                                                            *
********************************* Copyright (c) 2014-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria.Ontology.Modeler {

  /// <summary>Mapping rule between a type field and a data source element.</summary>
  internal class DataFieldMapping : DataMapping {

    #region Fields

    private FieldInfo fieldInfo = null;
    private Type memberType = null;

    #endregion Fields

    #region Constuctors and parsers

    internal DataFieldMapping(FieldInfo fieldInfo) {
      this.fieldInfo = fieldInfo;
      this.memberType = fieldInfo.FieldType;
    }

    #endregion Constructors and parsers

    #region Public properties

    internal override MemberInfo MemberInfo {
      get { return fieldInfo; }
    }

    internal override Type MemberType {
      get { return memberType; }
    }

    #endregion Public properties

    #region Public methods

    protected override object ImplementsGetValue(object instance) {
      return fieldInfo.GetValue(instance);
    }

    protected override void ImplementsSetValue(object instance, object value) {
      fieldInfo.SetValue(instance, value);
    }

    #endregion Public methods

  } // class DataFieldMapping

} // namespace Empiria.Ontology.Modeler
