/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : OperationSource                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes an operation or transaction source.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Describes an operation or transaction source.</summary>
  public class OperationSource : CommonStorage {

    #region Constructors and parsers

    static public OperationSource Parse(int id) => ParseId<OperationSource>(id);

    static public OperationSource Parse(string uid) => ParseKey<OperationSource>(uid);

    static public OperationSource ParseNamedKey(string namedKey) => ParseNamedKey<OperationSource>(namedKey);

    static public FixedList<OperationSource> GetList() {
      return BaseObject.GetList<OperationSource>(string.Empty, "Object_Name")
                       .ToFixedList();
    }

    static public OperationSource Empty => ParseEmpty<OperationSource>();

    static public OperationSource Default => ParseNamedKey("DEFAULT");

    #endregion Constructors and parsers

  }  // class OperationSource

}  // namespace Empiria.Parties
