﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : RulesLibrary                                     Pattern  : Ontology Object Type              *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a collection of business rules belonging to a named library.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System.Data;

using Empiria.Ontology;

namespace Empiria {

  /// <summary>Represents a rules library type.</summary>
  public class RulesLibrary : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.RulesLibrary";

    private BusinessRule[] rules = null;

    #endregion Fields

    #region Constructors and parsers

    public RulesLibrary()
      : this(thisTypeName) {

    }

    protected RulesLibrary(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public RulesLibrary Parse(int id) {
      return BaseObject.ParseId<RulesLibrary>(id);
    }

    static public RulesLibrary Parse(string rulesLibraryName) {
      return BaseObject.ParseKey<RulesLibrary>(rulesLibraryName);
    }

    #endregion Constructors and parsers

    public int Count {
      get { return Rules.Length; }
    }

    public BusinessRule[] Rules {
      get {
        if (rules == null) {
          rules = LoadRules();
        }
        return rules;

      }
    }

    public BusinessRule GetRule(int index) {
      return Rules[index];
    }

    public BusinessRule GetRule(string ruleName) {
      for (int i = 0; i < rules.Length; i++) {
        if (Rules[i].Name.ToLowerInvariant() == ruleName.ToLowerInvariant()) {
          return Rules[i];
        }
      }
      return Rules[0];
    }

    private BusinessRule[] LoadRules() {
      DataTable rulesTable = OntologyData.GetRulesLibrary(this.Id);
      BusinessRule[] rulesArray = new BusinessRule[rulesTable.Rows.Count];

      for (int i = 0; i < rulesTable.Rows.Count; i++) {
        rulesArray[i] = BusinessRule.ParseWithDataRow(rulesTable.Rows[i]);
      }
      return rulesArray;
    }

  } // class RulesLibrary

} //namespace Empiria
