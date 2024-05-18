/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Core                                       Component : Assertions                              *
*  Assembly : Empiria.Core.dll                           Pattern   : Interface                               *
*  Type     : IInvariant                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : A class invariant is an assertion that is be satisfied by every instance at all stable times.  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>A class invariant is an assertion that is be satisfied by every instance
  /// at all stable times.</summary>
  public interface IInvariant {

    void AssertInvariant();

  }  // interface IInvariant

} //namespace Empiria
