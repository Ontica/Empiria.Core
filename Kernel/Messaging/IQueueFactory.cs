/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Core Services           *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : IQueueFactory                                    Pattern  : Separated Interface               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Separated interface that represents a messaging queue factory.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Messaging {

  /// <summary>Separated interface that represents a messaging queue factory.</summary>
  public interface IQueueFactory {

    #region Members definition

    Queue GetDefaultQueue();

    Queue GetQueue(Message message);

    #endregion Members definition

  } // interface IQueueFactory

} // Empiria.Messaging