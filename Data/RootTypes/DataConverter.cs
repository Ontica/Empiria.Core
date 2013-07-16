/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataCache                                        Pattern  : Static Class                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Central repository to persist data using the caching mechanism of ASP .NET.                   *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System.Web.Script.Serialization;

namespace Empiria.Data {

  //[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey="8b7fe9c60c0f43bd")]
  static public class DataConverter {

    static public string ToJson(object o) {
      JavaScriptSerializer serializer = new JavaScriptSerializer();

      return serializer.Serialize(o);
    }

    static public string ToJson(object[] array) {
      JavaScriptSerializer serializer = new JavaScriptSerializer();

      return serializer.Serialize(array);
    }

    static public dynamic ToObject(string json) {
      JavaScriptSerializer serializer = new JavaScriptSerializer();

      return serializer.Deserialize<dynamic>(json);
    }

    static public T ToObject<T>(string json) {
      JavaScriptSerializer serializer = new JavaScriptSerializer();

      return serializer.Deserialize<T>(json);
    }

  } // class DataConverter

} // namespace Empiria.Data