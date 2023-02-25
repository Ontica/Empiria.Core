/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaSession                                   Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : This internal class represents an Empiria system user session.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  public interface IEmpiriaSession {

    int Id {
      get;
    }

    string Token {
      get;
    }


    int ClientAppId {
      get;
    }


    int UserId {
      get;
    }


    string TokenType {
      get;
    }


    int ExpiresIn {
      get;
    }


    string RefreshToken {
      get;
    }


    bool IsStillActive {
      get;
    }


    void Close();


    void UpdateEndTime();


  }  // interface IEmpiriaSession

} //namespace Empiria.Security
