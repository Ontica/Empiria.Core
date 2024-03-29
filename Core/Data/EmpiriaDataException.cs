﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaDataException                             Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a data access operation fails.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Empiria.Data {

  /// <summary>The exception that is thrown when a data access operation fails.</summary>
  [Serializable]
  public sealed class EmpiriaDataException : EmpiriaException {

    public enum Msg {
      AsynchronousCommitNotCalled,
      ActionQueryDoesntReturnAValue,
      CacheRestoreRemovedObjectCallbackFails,
      CannotCreateDataTask,
      CannotDoPostExecutionTask,
      CannotExecuteActionQuery,
      CannotGetDataReader,
      CannotGetDataTable,
      CannotGetDataView,
      CannotGetFieldValue,
      CannotGetObjectId,
      CannotGetScalar,
      CannotLoadDataIntegrationRules,
      CannotParseDataIntegrationServer,
      CannotRegisterObjectIdFactory,
      CommitFails,
      DataContextOutOfTransaction,
      DataContextTooManyItemsForRemove,
      DataConverterForTypeAlreadyExists,
      DataConverterForTypeNotFound,
      DataIntegrationWSProxyException,
      DataSourceNotDefined,
      DuplicateDataIntegrationEngine,
      DuplicateObjectIdFactory,
      InvalidCacheItemRemovedReason,
      InvalidDatabaseTechnology,
      ObjectIdOutOfValidBounds,
      ObjectIdRuleNotSet,
      ObjectRemovedFromDataCache,
      RollbackFails,
      TransactionAlreadyCommited,
      TransactionCommited,
      UndefinedDataPublishRule,
      WrongQueryParametersNumber,
    }

    static private string resourceBaseName = "Empiria.Data.DataExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of EmpiriaDataException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public EmpiriaDataException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {
      try {
       base.Publish();
      } finally {
        // no-op
      }
    }

    /// <summary>Initializes a new instance of EmpiriaDataException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="exception">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public EmpiriaDataException(Msg message, Exception exception,
                                params object[] args)
      : base(message.ToString(), GetMessage(message, args), exception) {
      try {
       base.Publish();
      } finally {
        // no-op
      }
    }

    public EmpiriaDataException(SerializationInfo info, StreamingContext context)
      : base(info, context) {

    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class EmpiriaDataException

} // namespace Empiria.Data
