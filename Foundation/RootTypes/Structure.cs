/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : Structure                                        Pattern  : Abstract Class                    *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : The type Structure is the root of the structure type hierarchy. All structure types must be   *
*              descendants of this type.                                                                     *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Data;
using Empiria.Ontology;

namespace Empiria {

  public abstract class Structure {

    #region Fields

    private StructureTypeInfo structureTypeInfo = null;

    #endregion Fields

    #region Constructors and parsers

    protected Structure(string typeName) {
      if (typeName.Length != 0) {   // If typeName.Length == 0, is invoked with Parsing using reflection
        this.structureTypeInfo = StructureTypeInfo.Parse(typeName);
        //dynamicState = new DynamicState(this);
      }
    }

    //static public T Create<T>(StructureTypeInfo typeInfo) where T : Structure {
    //  T item = (T) BaseObject.InvokeBaseObjectConstructor(typeInfo);
    //  item.objectTypeInfo = typeInfo;
    //  item.dynamicState = new DynamicState(item);

    //  return item;
    //}

    static public T Parse<T>(string json) where T : Structure {
      return JsonConverter.ToObject<T>(json);
    }

    #endregion Constructors and parsers

    #region Public properties

    public StructureTypeInfo StructureTypeInfo {
      get { return this.structureTypeInfo; }
    }

    #endregion Public properties

  } // class Structure

} // namespace Empiria
