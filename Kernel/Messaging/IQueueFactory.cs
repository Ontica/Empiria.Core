/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Messaging Core Services           *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : IQueueFactory                                    Pattern  : Separated Interface               *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Separated interface that represents a messaging queue factory.                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/

namespace Empiria.Messaging {

  /// <summary>Separated interface that represents a messaging queue factory.</summary>
  public interface IQueueFactory {

    #region Members definition

    Queue GetDefaultQueue();

    Queue GetQueue(Message message);

    #endregion Members definition

  } // interface IQueueFactory

} // Empiria.Messaging