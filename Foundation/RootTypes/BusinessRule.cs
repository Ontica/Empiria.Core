/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : BusinessRule                                     Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Defines a business rule.                                                                      *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Ontology;

namespace Empiria {

  public enum BusinessRuleType {
    Method = 'M',
    Textual = 'T'
  }

  /// <summary>Defines a business rule.</summary>
  public class BusinessRule {

    #region Fields

    private int id = 0;
    private BusinessRuleType ruleType = BusinessRuleType.Textual;
    private int typeMethodId = 0;
    private string[] argumentsNames = new string[] { };
    private string name = String.Empty;
    private string documentation = String.Empty;
    private int precision = 0;
    private BusinessRuleItem[] statements = new BusinessRuleItem[] { };
    private int returnTypeId = 0;
    private Type returnType = null;

    #endregion Fields

    #region Constructors and parsers

    public BusinessRule() {

    }

    static public BusinessRule Parse(int businessRuleId) {
      DataRow row = OntologyData.GetRule(businessRuleId);

      return ParseWithDataRow(row);
    }

    static internal BusinessRule ParseWithDataRow(DataRow row) {
      BusinessRule businessRule = new BusinessRule();

      businessRule.id = (int) row["RuleId"];
      businessRule.ruleType = (BusinessRuleType) Convert.ToChar(row["RuleType"]);
      businessRule.typeMethodId = (int) row["TypeMethodId"];
      businessRule.name = (string) row["RuleName"];
      businessRule.documentation = (string) row["Documentation"];
      businessRule.returnTypeId = (int) row["ReturnTypeId"];
      businessRule.precision = (int) row["Precision"];

      businessRule.LoadArguments();
      businessRule.LoadStatements();

      return businessRule;
    }

    #endregion Constructors and parsers

    #region Public properties

    public string[] ArgumentsNames {
      get { return this.argumentsNames; }
    }

    public BusinessRuleType BusinessRuleType {
      get { return this.ruleType; }
    }

    public string Documentation {
      get { return this.documentation; }
    }

    public string Name {
      get { return this.name; }
    }

    public int Precision {
      get { return this.precision; }
    }

    public BusinessRuleItem[] Statements {
      get { return this.statements; }
    }

    public int TypeMethodId {
      get { return this.typeMethodId; }
    }

    #endregion Public properties

    #region Public methods

    public BusinessRuleItem GetStatement(int index) {
      return this.statements[index];
    }

    public Type ReturnValueUnderlyingSystemType {
      get {
        if (this.returnType == null) {
          FundamentalTypeInfo temp = FundamentalTypeInfo.Parse(this.returnTypeId);
          this.returnType = temp.UnderlyingSystemType;
        }
        return this.returnType;
      }
    }

    #endregion Public methods

    #region Internal methods

    private void LoadArguments() {
      DataTable rulesTable = OntologyData.GetRuleItems(this.id, "A");
      argumentsNames = new string[rulesTable.Rows.Count];

      for (int i = 0; i < rulesTable.Rows.Count; i++) {
        argumentsNames[i] = (string) rulesTable.Rows[i]["VariableSymbol"];
      }
    }

    private void LoadStatements() {
      DataTable rulesTable = OntologyData.GetRuleItems(this.id, "S");
      statements = new BusinessRuleItem[rulesTable.Rows.Count];

      for (int i = 0; i < rulesTable.Rows.Count; i++) {
        statements[i] = BusinessRuleItem.ParseWithDataRow(this, rulesTable.Rows[i]);
      }
    }

    #endregion Internal methods

  }  // class BusinessRule

} // namespace Empiria
