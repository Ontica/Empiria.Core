/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : Queue                                            Pattern  : Abstract Class                    *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Abstract class that represents a messaging queue object. All messaging queue types must be    *
*              derived from this type.                                                                       *
*																																																						 *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Messaging {

  /// <summary>Abstract class that represents a messaging queue object. All messaging queue types must be
  /// derived from this type.</summary>
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
        throw new MessagingException(MessagingException.Msg.QueueNotStarted, this.FullName);
      }
      try {
        return ImplementsRetrive();
      } catch (Exception innerException) {
        throw new MessagingException(MessagingException.Msg.CantRetriveMessage, innerException,
                                     this.FullName);
      }
    }

    public void Send(Message message) {
      if (!IsStarted) {
        throw new MessagingException(MessagingException.Msg.QueueNotStarted, this.FullName);
      }
      try {
        ImplementsSend(message);
      } catch (Exception innerException) {
        throw new MessagingException(MessagingException.Msg.CantSendMessage, innerException,
                                     this.FullName);
      }
    }

    public void Start() {
      try {
        ImplementsStart();
        isStarted = true;
      } catch (Exception innerException) {
        isStarted = false;
        throw new MessagingException(MessagingException.Msg.CantStartQueue, innerException,
                                     this.FullName);
      }
    }

    #endregion Public methods

  } // abstract class Queue

} // namespace Empiria.Messaging
