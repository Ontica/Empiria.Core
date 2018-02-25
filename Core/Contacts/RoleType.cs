using System;

namespace Empiria.Contacts {

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
