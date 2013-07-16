/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : Structure                                        Pattern  : Abstract Class                    *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : The type Structure is the root of the structure type hierarchy. All structure types must be   *
*              descendants of this type.                                                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

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

    #endregion Constructors and parsers

    #region Public properties

    public StructureTypeInfo StructureTypeInfo {
      get { return this.structureTypeInfo; }
    }

    #endregion Public properties

  } // class Structure

} // namespace Empiria
