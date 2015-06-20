/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : IIdentifiable                                    Pattern  : Separated interface               *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Interface that represents an identifiable object throw an integer Id.                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Messaging {

  /// <summary>Separated interface that represents a messaging queue factory.</summary>
  public interface IQueueFactory {

    #region Members definition

    Queue GetDefaultQueue();

    Queue GetQueue(Message message);

    #endregion Members definition

  } // interface IQueueFactory

} // namespace Empiria.Messaging
