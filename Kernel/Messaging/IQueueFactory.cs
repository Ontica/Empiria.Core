/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Core Services           *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : IQueueFactory                                    Pattern  : Separated Interface               *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Separated interface that represents a messaging queue factory.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Messaging {

  /// <summary>Separated interface that represents a messaging queue factory.</summary>
  public interface IQueueFactory {

    #region Members definition

    Queue GetDefaultQueue();

    Queue GetQueue(Message message);

    #endregion Members definition

  } // interface IQueueFactory

} // Empiria.Messaging
