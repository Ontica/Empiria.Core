/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Core Services           *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : Message                                          Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents an atomic data package that can be transmitted by a message queue.                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

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

    #region Private methods

    #endregion Private methods

  } // class Message

} //namespace Empiria.Messaging