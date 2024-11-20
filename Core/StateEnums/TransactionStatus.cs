/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : TransactionStatus                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the status of a transaction.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.StateEnums {

  /// <summary>Enumerates the distinct status control values for transactions.</summary>
  public enum TransactionStatus {

    Pending = 'P',

    OnAuthorization = 'A',

    Completed = 'C',

    Deleted = 'X',

    All = '*'

  }  // enum TransactionStatus



  /// <summary>Enumerates the different parties that participates in a transaction.</summary>
  public enum TransactionPartyType {

    RequestedBy,

    RegisteredBy,

    AuthorizedBy,

    CompletedBy,

    None,

  }  // enum TransactionPartyType



  /// <summary>Enumerates the different workflow stages for a transaction.</summary>
  public enum TransactionStage {

    MyInbox,

    Pending,

    ControlDesk,

    Completed,

    All

  }  // enum TransactionStage



  /// <summary>Enumerates the different dates for transactions.</summary>
  public enum TransactionDateType {

    Requested,

    Registered,

    Authorizated,

    Completed,

    None

  }  // enum TransactionDateType



  /// <summary>Input query DTO used to retrieve budget transactions parties.</summary>
  public class TransactionPartiesQuery {

    public TransactionPartyType PartyType {
      get; set;
    }

    public string Keywords {
      get; set;
    }

  }  // class TransactionPartiesQuery



  /// <summary>Extension methods for TransactionStatus type.</summary>
  static public class TransactionStatusExtensions {

    static public string GetName(this TransactionStatus status) {

      switch (status) {
        case TransactionStatus.Pending:
          return "Pendiente";
        case TransactionStatus.OnAuthorization:
          return "En autorización";
        case TransactionStatus.Completed:
          return "Completada";
        case TransactionStatus.Deleted:
          return "Eliminada";
        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized budget transaction status {status}.");
      }
    }


    static public NamedEntityDto MapToNamedEntity(this TransactionStatus status) {
      return new NamedEntityDto(status.ToString(), status.GetName());
    }

  }  // class TransactionStatusExtensions

} // namespace Empiria.StateEnums
