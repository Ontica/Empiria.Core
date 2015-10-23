/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : Message                                          Pattern  : Standard Class                    *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents an atomic data package that can be transmitted by a message queue.                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Messaging {

  /// <summary>Classifies message types.</summary>
  public enum MessageType {
    CommandMessage = 'C',
    EventMessage = 'E',
    DocumentMessage = 'D',
  }

  /// <summary>Represents an atomic data package that can be transmitted by a message queue.</summary>
  public class Message {

    #region Fields

    private object body = null;
    private MessageType messageType = MessageType.EventMessage;

    public Message(object body) {
      this.body = body;
    }

    public Message(MessageType messageType, object body) {
      this.messageType = messageType;
      this.body = body;
    }

    #endregion Fields

    #region Constructors and parsers

    #endregion Constructors and parsers

    #region Public properties

    public object Body {
      get { return body; }
    }

    public MessageType MessageType {
      get { return messageType; }
      set { messageType = value; }
    }

    #endregion Public properties

  } // class Message

} //namespace Empiria.Messaging
