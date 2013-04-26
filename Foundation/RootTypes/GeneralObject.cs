/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : GeneralObject                                    Pattern  : Storage Item                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Abstract type that holds basic object instances which are stored in a general common table.   *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;

using Empiria.Contacts;

namespace Empiria {

  /// <summary>Abstract type that holds basic object instances which are 
  /// stored in a general common table</summary>
  public abstract class GeneralObject : BaseObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject";

    private string namedKey = String.Empty;
    private string name = String.Empty;
    private string description = String.Empty;
    private string keywords = String.Empty;
    private Contact postedBy = Person.Empty;
    private int replacedById = 0;
    private GeneralObjectStatus status = GeneralObjectStatus.Active;
    private DateTime startDate = DateTime.Today;
    private DateTime endDate = ExecutionServer.DateMaxValue;

    #endregion Fields

    #region Constructors and parsers

    protected GeneralObject(string typeName)
      : base(typeName) {
      // Empiria Object Type pattern classes always has this constructor. Don't delete
    }

    #endregion Constructors and parsers

    #region Public properties

    protected string Description {
      get { return description; }
      set { description = EmpiriaString.TrimAll(value); }
    }

    protected DateTime EndDate {
      get { return endDate; }
      set { endDate = value; }
    }

    protected string Keywords {
      get { return keywords; }
      set { keywords = EmpiriaString.TrimAll(value); }
    }

    public string Name {
      get { return name; }
      protected set { name = EmpiriaString.TrimAll(value); }
    }

    protected string NamedKey {
      get { return namedKey; }
      set { namedKey = EmpiriaString.TrimAll(value); }
    }

    protected Contact PostedBy {
      get { return postedBy; }
      set { postedBy = value; }
    }

    protected int ReplacedById {
      get { return replacedById; }
    }

    protected DateTime StartDate {
      get { return startDate; }
      set { startDate = value; }
    }

    public GeneralObjectStatus Status {
      get { return status; }
      protected set { status = value; }
    }

    public string StatusName {
      get {
        switch (status) {
          case GeneralObjectStatus.Active:
            return "Activo";
          case GeneralObjectStatus.Deleted:
            return "Eliminado";
          case GeneralObjectStatus.Pending:
            return "Pendiente";
          case GeneralObjectStatus.Suspended:
            return "Suspendido";
          default:
            return "No determinado";
        }
      }
    }

    #endregion Public properties

    #region Public methods

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.namedKey = (string) row["GeneralObjectNamedKey"];
      this.name = (string) row["GeneralObjectName"];
      this.description = (string) row["GeneralObjectDescription"];
      this.keywords = (string) row["GeneralObjectKeywords"];
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.replacedById = (int) row["ReplacedById"];
      this.status = (GeneralObjectStatus) Convert.ToChar(row["GeneralObjectStatus"]);
      this.startDate = (DateTime) row["StartDate"];
      this.endDate = (DateTime) row["EndDate"];
    }

    protected override void ImplementsSave() {

    }

    #endregion Public methods

  } // class GeneralObject

} // namespace Empiria