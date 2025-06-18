/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Mapper                                  *
*  Type     : OrganizationalUnitMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping services for organizational units.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Parties.Adapters {

  /// <summary>Mapping services for organizational units.</summary>
  static public class OrganizationalUnitMapper {

    static public FixedList<OrganizationalUnitDescriptor> Map(FixedList<OrganizationalUnit> orgUnits) {
      return orgUnits.Select(x => MapToDescriptor(x))
                     .ToFixedList();

    }


    static public OrganizationalUnitDto Map(OrganizationalUnit orgUnit) {
      return new OrganizationalUnitDto {
        UID = orgUnit.UID,
        Code = orgUnit.Code,
        Name = orgUnit.Name,
        FullName = orgUnit.FullName,
        Type = orgUnit.PartyType.MapToNamedEntity(),
        Parent = orgUnit.Parent.MapToNamedEntity(),
        Responsible = Person.Empty.MapToNamedEntity(),
        Level = orgUnit.Level,
        IsLastLevel = true,
        StartDate = orgUnit.StartDate,
        EndDate = orgUnit.EndDate,
        Obsolete = false,
        Status = orgUnit.Status.MapToDto(),
      };
    }


    static public OrganizationalUnitDescriptor MapToDescriptor(OrganizationalUnit orgUnit) {
      return new OrganizationalUnitDescriptor {
        UID = orgUnit.UID,
        Code = orgUnit.Code,
        Name = orgUnit.Name,
        FullName = orgUnit.FullName,
        TypeName = orgUnit.PartyType.DisplayName,
        ParentName = orgUnit.Parent.FullName,
        ResponsibleName = "No determinado",
        Level = orgUnit.Level,
        IsLastLevel = true,
        StartDate = orgUnit.StartDate,
        EndDate = orgUnit.EndDate,
        Obsolete = false,
        StatusName = orgUnit.Status.GetName(),
      };
    }

  }  // class OrganizationalUnitMapper

}  // namespace Empiria.Parties.Adapters
