/* Empiria Foundation Framework 2015 *************************************************************************
*																																																						 *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : QueueFactory                                     Pattern  : Factory                           *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*																																																						 *
*  Summary   : Factory used for create messaging queues based on message type and content rules.             *
*																																																						 *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Messaging {

  /// <summary>Factory used for create messaging queues based on message type and content rules.</summary>
  internal sealed class QueueFactory : IQueueFactory {

    #region Fields

    static private Queue defaultQueue = null;

    #endregion Fields

    #region Constructors and parsers

    public QueueFactory() {
      // no-op
    }

    #endregion Constructors and parsers

    #region Public methods

    public Queue GetDefaultQueue() {
      if (defaultQueue == null) {
        defaultQueue = CreateDefaultQueue();
      }
      if (!defaultQueue.IsStarted) {
        defaultQueue.Start();
      }
      return defaultQueue;
    }

    private Queue CreateDefaultQueue() {
      return new WindowsEventLogQueue(ExecutionServer.IsSpecialLicense ? ExecutionServer.LicenseName
                                                                       : "Empiria");
    }

    public Queue GetQueue(Message message) {
      return GetDefaultQueue();
    }

    #endregion Public methods

  } // class QueueFactory

} // namespace Empiria.Messaging
