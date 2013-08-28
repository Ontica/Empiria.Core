﻿/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Messaging Core Services           *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : Queue                                            Pattern  : Abstract Class                    *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Abstract class that represents a messaging queue object.                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

namespace Empiria.Messaging {

  /// <summary>Abstract class that represents a messaging queue object.</summary>
  public abstract class Queue {

    #region Abstract members

    protected abstract Message ImplementsRetrive();
    protected abstract void ImplementsSend(Message message);
    protected abstract void ImplementsStart();

    #endregion Abstract members

    #region Fields

    private string name = null;
    private bool isStarted = false;

    #endregion Fields

    #region Constructors and parsers

    protected Queue(string name) {
      this.name = name;
    }

    #endregion Constructors and parsers

    #region Public properties

    public string FullName {
      get { return this.GetType().FullName + "." + this.name; }
    }

    public bool IsStarted {
      get { return isStarted; }
    }

    public string Name {
      get { return name; }
    }

    #endregion Public properties

    #region Public methods

    public Message Retrive() {
      if (!IsStarted) {
        throw new MessagingCoreException(MessagingCoreException.Msg.QueueNotStarted, this.FullName);
      }
      try {
        return ImplementsRetrive();
      } catch (Exception innerException) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantRetriveMessage, innerException,
                                     this.FullName);
      }
    }

    public void Send(Message message) {
      if (!IsStarted) {
        throw new MessagingCoreException(MessagingCoreException.Msg.QueueNotStarted, this.FullName);
      }
      try {
        ImplementsSend(message);
      } catch (Exception innerException) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantSendMessage, innerException,
                                     this.FullName);
      }
    }

    public void Start() {
      try {
        ImplementsStart();
        isStarted = true;
      } catch (Exception innerException) {
        isStarted = false;
        throw new MessagingCoreException(MessagingCoreException.Msg.CantStartQueue, innerException,
                                     this.FullName);
      }
    }

    #endregion Public methods

  } // abstract class Queue

} // Empiria.Messaging