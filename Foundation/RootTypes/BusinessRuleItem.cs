/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : BusinessRuleItem                                 Pattern  : Standard Class                    *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Defines a business rule item.                                                                 *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;

using Empiria.Ontology;

namespace Empiria {

  /// <summary>Defines a business rule item.</summary>
  public class BusinessRuleItem {

    #region Fields

    private int id = 0;
    private BusinessRule parentRule = null;
    private string statement = String.Empty;
    private string variableSymbol = "return";
    private int typeId = 0;
    private int precision = 0;
    private Type type = null;

    #endregion Fields

    #region Constructors and parsers

    public BusinessRuleItem(BusinessRule parentRule) {
      this.parentRule = parentRule;
    }

    static internal BusinessRuleItem ParseWithDataRow(BusinessRule parentRule, DataRow row) {
      BusinessRuleItem businessRuleItem = new BusinessRuleItem(parentRule);

      businessRuleItem.id = (int) row["RuleItemId"];
      businessRuleItem.variableSymbol = (string) row["VariableSymbol"];
      businessRuleItem.statement = (string) row["Statement"];
      businessRuleItem.typeId = (int) row["TypeId"];
      businessRuleItem.precision = (int) row["Precision"];

      return businessRuleItem;
    }

    #endregion Constructors and parsers

    #region Public properties

    public string Statement {
      get { return this.statement; }
    }

    public string VariableSymbol {
      get { return this.variableSymbol; }
    }

    public int Precision {
      get { return this.precision; }
    }

    public Type UnderlyingSystemType {
      get {
        if (this.type == null) {
          FundamentalTypeInfo fundamentalType = FundamentalTypeInfo.Parse(this.typeId);
          this.type = fundamentalType.UnderlyingSystemType;
        }
        return this.type;
      }
    }

    #endregion Public properties

    #region Internal methods

    #endregion Internal methods

  }  // class BusinessRuleItem

} // namespace Empiria
