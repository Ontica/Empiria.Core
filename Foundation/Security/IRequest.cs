using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Empiria.Security {

  public interface IRequest {

    Guid Guid {
      get;
    }

    EmpiriaPrincipal Principal {
      get;
    }

    DateTime StartTime {
      get;
    }

    int AppliedToId {
      get;
    }

  }  // interface IRequest

}  // namespace Empiria.Security
