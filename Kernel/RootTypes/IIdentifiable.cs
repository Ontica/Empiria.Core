/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : IIdentifiable                                    Pattern  : Separated Interface               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Interface that represents an identifiable object throw an integer Id.                         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria {

  /// <summary>Interface that represents an identifiable object throw an integer Id.</summary>
  public interface IIdentifiable {

    #region Members definition

    int Id { get; }

    #endregion Members definition

  } // interface IIdentifiable

} // namespace Empiria