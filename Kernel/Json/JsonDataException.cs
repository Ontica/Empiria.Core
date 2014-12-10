/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Json                                     Assembly : Empiria.Kernel.dll                *
*  Type      : JsonDataException                                Pattern  : Exception Class                   *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : The exception that is thrown when a JSON data operation fails.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. Ontica LLC, La Vía Óntica SC and contributors.  **/
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Empiria.Json {

  /// <summary>The exception that is thrown when a JSON data operation fails.</summary>
  [Serializable]
  public sealed class JsonDataException : EmpiriaException {

    public enum Msg {
      JsonConverterForTypeAlreadyExists,
      JsonConverterForTypeNotFound,
      JsonPathItemNotFound,
      JsonSlicePathNotFound,
    }

    static private string resourceBaseName = "Empiria.RootTypes.KernelExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of JsonDataException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public JsonDataException(Msg message, params object[] args)
                             : base(message.ToString(), GetMessage(message, args)) {
      try {
        base.Publish();
      } finally {
        // no-op
      }
    }

    /// <summary>Initializes a new instance of JsonDataException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="exception">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public JsonDataException(Msg message, Exception exception,
                             params object[] args) :
                             base(message.ToString(), GetMessage(message, args), exception) {
      try {
        base.Publish();
      } finally {
        // no-op
      }
    }

    public JsonDataException(SerializationInfo info, StreamingContext context)
                             : base(info, context) {

    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class JsonDataException

} // namespace Empiria.Data
