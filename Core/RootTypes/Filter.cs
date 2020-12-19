/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Data processing services                     Component : Searching and filtering               *
*  Assembly : Empiria.Core.dll                             Pattern   : Utility class                         *
*  Type     : Filter                                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Utility that allows build object filter expressions.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Text;

namespace Empiria {

  /// <summary>Utility that allows build object filter expressions.</summary>
  public class Filter {

    #region Fields

    private readonly StringBuilder builder = new StringBuilder();

    #endregion Fields


    #region Public Members

    public Filter(string initialValue) {
      if (EmpiriaString.IsEmpty(initialValue)) {
        return;
      }

      this.builder.Append(initialValue);
    }


    public bool IsEmpty {
      get {
        return (builder.Length == 0);
      }
    }

    public void AppendAnd(string expression) {
      if (EmpiriaString.IsEmpty(expression)) {
        return;
      }

      if (this.IsEmpty) {
        this.builder.Append(expression);
      } else {
        this.builder.Append($" AND {expression}");
      }
    }


    public void AppendOr(string expression) {
      if (EmpiriaString.IsEmpty(expression)) {
        return;
      }

      if (this.IsEmpty) {
        this.builder.Append(expression);
      } else {
        this.builder.Append($" OR {expression}");
      }
    }

    public override string ToString() {
      return this.builder.ToString();
    }


    #endregion Public Members

  } // class Filter

} // namespace Empiria
