/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : IIdentifiable                                    Pattern  : Loose coupling interface          *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Interface that represents an identifiable object throw an integer Id.                         *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria {

  /// <summary>Interface that represents an identifiable object throw an integer Id.</summary>
  public interface IIdentifiable {

    #region Members definition

    int Id { get; }

    #endregion Members definition

  } // interface IIdentifiable

} // namespace Empiria