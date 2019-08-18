﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : IntegrityValidator                               Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Provides a service to secure entities using a data integrity field, which it is calculted     *
*              as a hash code of a list of the entity's protected fields.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Provides a service to secure entities using a data integrity field, which it is calculted
  /// as a hash code of a list of the entity's protected fields.</summary>
  public class IntegrityValidator {

    private IProtected resource = null;
    private string resourceTypeName = null;

    public IntegrityValidator(IProtected resource) {
      this.resource = resource;
      this.resourceTypeName = resource.GetType().FullName;
    }

    public void Assert(string dataIntegrityFieldValue) {

      //if (dataIntegrityFieldValue.Length == 0) {      // OOJJOO Remove this line to assure integrity check
      //  return;
      //}
      //int version = Convert.ToInt32(dataIntegrityFieldValue.Substring(0, 1), 16);
      //string storedDIFData = dataIntegrityFieldValue.Substring(1);

      //if (!storedDIFData.Equals(this.GetDIFHashCode(version))) {
      //  var e = new SecurityException(SecurityException.Msg.IntegrityValidatorAssertFails,
      //                                resourceTypeName, resource.Id);
      //  e.Publish();  // OOJJOO Exception only published, not throwed
      //  //throw new SecurityException(SecurityException.Msg.IntegrityValidatorAssertFails,
      //  //                            resourceTypeName, resource.Id);
      //}
    }

    public string GetUpdatedHashCode() {
      int version = resource.CurrentDataIntegrityVersion;

      Assertion.Assert(1 <= version && version <= 16, "Invalid version number");

      return version.ToString("X") + this.GetDIFHashCode(version);
    }

    private string GetDIFHashCode(int version) {
      string data = this.GetDIFString(version);

      return FormerCryptographer.CreateHashCode(data, version.ToString("X") + "." + resourceTypeName);
    }

    private string GetDIFString(int version) {
      object[] currentData = resource.GetDataIntegrityFieldValues(version);

      Assertion.AssertObject(currentData, "currentData");
      Assertion.Assert(version == (int) currentData[0],
                       "Invalid version returned by the data integrity field vector.");
      Assertion.Assert((currentData.Length % 2) == 1,
                       "Invalid data integrity field vector for version {0} in protected type {1}.",
                       version, resourceTypeName);

      string dif = "||";
      for (int i = 0; i < currentData.Length; i++) {
        object item = currentData[i];
        if (item is string) {
          dif += (string) item;
        } else if (item is Int32) {
          dif += ((int) item).ToString();
        } else if (item is DateTime) {
          dif += ((DateTime) item).ToString("yyyy-MM-dd HH:mm:ss");
        } else if (item is Boolean) {
          dif += ((Boolean) item).ToString();
        } else if (item is decimal) {
          dif += ((decimal) item).ToString();
        } else if (item is char) {
          dif += ((char) item).ToString();
        } else if (item is IIdentifiable) {
          dif += ((IIdentifiable) item).Id;
        } else {
          throw new SecurityException(SecurityException.Msg.InvalidDIFDataItemDataType,
                                      resourceTypeName, currentData[i],
                                      currentData[i].GetType().FullName, i,
                                      version, resource.GetType());
        }
        dif += "|";
      }
      return dif + "|";
    }

  }  // class IntegrityValidator

} // namespace Empiria.Security
