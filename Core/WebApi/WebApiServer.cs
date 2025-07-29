/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                                 Component : Web Api Services                      *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain class                          *
*  Type     : WebApiServer                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes a web api server used to invoke web APIs from client apps.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DataTypes;
using Empiria.Security;

namespace Empiria.WebApi {

  /// <summary>Describes a web api server used to invoke web APIs from client apps.</summary>
  public class WebApiServer : GeneralObject {

    #region Constructors and parsers

    static public WebApiServer Parse(int id) => ParseId<WebApiServer>(id);

    static public WebApiServer Parse(string uid) => ParseKey<WebApiServer>(uid);

    static public WebApiServer Empty => ParseEmpty<WebApiServer>();

    static public FixedList<WebApiServer> GetList() {
      return GetFullList<WebApiServer>()
            .FindAll(x => x.Status != StateEnums.EntityStatus.Deleted);
    }

    #endregion Constructors and parsers

    #region Properties

    public string BaseAddress {
      get {
        return ExtendedDataField.Get("baseAddress", string.Empty);
      }
    }


    public bool IsEmpiriaBased {
      get {
        return ExtendedDataField.Get("isEmpiriaBased", false);
      }
    }


    public FixedList<KeyValue> ConfigData {
      get {
        return ExtendedDataField.GetFixedList<KeyValue>("configData", false);
      }
    }


    public FixedList<KeyValue> Headers {
      get {
        return ExtendedDataField.GetFixedList<KeyValue>("headers", false);
      }
    }


    public IUserCredentials Credentials {
      get {
        return ExtendedDataField.Get("credentials", new UserCredentialsDto());
      }
    }

    #endregion Properties

  } // class WebApiServer

} // namespace Empiria.WebApi
