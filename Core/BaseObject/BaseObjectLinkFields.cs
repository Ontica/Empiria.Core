/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Base Object                                Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Input fields DTO                        *
*  Type     : BaseObjectLinkFields                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update BaseObjectLink instances.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria {

  /// <summary>Input fields DTO used to create and update BaseObjectLink instances.</summary>
  public class BaseObjectLinkFields : NamedEntityFields {

    public string LinkedObjectRole {
      get; set;
    } = string.Empty;


    public DateTime StartDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime EndDate {
      get; set;
    } = ExecutionServer.DateMaxValue;

  }  // class BaseObjectLinkFields



  /// <summary>Extension methods for BaseObjectLinkFields.</summary>
  static public class BaseObjectLinkFieldsExtensions {

    static public void EnsureValid(this BaseObjectLinkFields fields) {

    }

  }  // class BaseObjectLinkFields

}  // namespace Empiria.Documents
