using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empiria.Contacts {

      //  var roleType = RoleType.Parse(positionId);
      //var place = GeographicRegion.Parse(placeId);
      //FixedList<Person> list = roleType.GetActors<Person>(place);

      ////var place = GeographicRegion.Parse(placeId);
      ////TypeAssociationInfo role = place.ObjectTypeInfo.Associations[positionId];
      ////FixedList<Person> list = place.GetPeople(role.Name);

  public class RoleType : BaseObject {

    static public RoleType Parse(int id) {
      throw new NotImplementedException();
    }

    static public RoleType Parse(string roleName) {
      throw new NotImplementedException();
    }

    public FixedList<T> GetActors<T>(BaseObject context) where T : Contact {
      throw new NotImplementedException();
    }


    static public RoleType Empty {
      get;
      set;
    }
  } // class RoleType

}  // namespace Empiria.Contacts
