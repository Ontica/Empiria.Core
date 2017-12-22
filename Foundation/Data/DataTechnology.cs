/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution : Empiria Foundation Framework                     System  : Data Access Library                 *
*  Assembly : Empiria.Foundation.dll                           Pattern : Enumeration                         *
*  Type     : DataSource                                       License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : List with the data technologies used by Empiria solutions.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Data {

  /// <summary>List with the data technologies used by Empiria solutions.</summary>
  public enum DataTechnology {

    SqlServer = 1,

    MySql = 2,

    Oracle = 3,

    PostgreSql = 4,

    OleDb = 5,

    Odbc = 6,

  }  // enum DataTechnology

} //namespace Empiria.Data
