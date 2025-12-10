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

    Requested = 'Q',

    OnAuthorization = 'T',

    Authorized = 'A',

    Signed = 'G',

    Closed = 'C',

    Returned = 'R',

    Rejected = 'J',

    Suspended = 'S',

    Canceled = 'D',

    Deleted = 'X',

    All = '*'

  }  // enum TransactionStatus



  /// <summary>Enumerates the different roles for parties that participates in a transaction.</summary>
  public enum TransactionPartyRole {

    RequestedBy,

    RegisteredBy,

    AuthorizedBy,

    SignedBy,

    ClosedBy,

    None,

  }  // enum TransactionPartyRole



  /// <summary>Enumerates the different workflow stages for a transaction.</summary>
  public enum TransactionStage {

    MyInbox,

    Pending,

    ControlDesk,

    Closed,

    All

  }  // enum TransactionStage



  /// <summary>Enumerates the different dates for transactions.</summary>
  public enum TransactionDateType {

    Requested,

    Registered,

    Authorizated,

    Signed,

    Completed,

    None

  }  // enum TransactionDateType



  /// <summary>Input query DTO used to retrieve transactions parties.</summary>
  public class TransactionPartiesQuery {

    public TransactionPartyRole PartyType {
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

        case TransactionStatus.Requested:
          return "Solicitada";

        case TransactionStatus.OnAuthorization:
          return "En autorización";

        case TransactionStatus.Authorized:
          return "Autorizada";

        case TransactionStatus.Signed:
          return "Firmada";

        case TransactionStatus.Closed:
          return "Cerrada";

        case TransactionStatus.Returned:
          return "Regresada";

        case TransactionStatus.Rejected:
          return "Rechazada";

        case TransactionStatus.Suspended:
          return "Suspendida";

        case TransactionStatus.Canceled:
          return "Cancelada";

        case TransactionStatus.Deleted:
          return "Eliminada";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized transaction status {status}.");
      }
    }



    static public NamedEntityDto MapToNamedEntity(this TransactionStatus status) {
      return new NamedEntityDto(status.ToString(), status.GetName());
    }

  }  // class TransactionStatusExtensions

} // namespace Empiria.StateEnums
