/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Data services                           *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Access Library                     *
*  Type     : StorageData                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data read and write methods for storage instances.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Storage {

  /// <summary>Data read and write methods for storage instances.</summary>
  static public class StorageData {


    static internal void WriteStorageContainer(StorageContainer o) {
      var op = DataOperation.Parse("writeEXFStorageItem",
                    o.Id, o.UID, o.GetEmpiriaType().Id,
                    o.ContainerType, string.Empty, o.Size,
                    o.Storage.Id, -1, -1, o.RelativePath,
                    o.Name, string.Empty, o.Keywords, o.ExtensionData.ToString(),
                    string.Empty, o.PostedBy.Id, o.PostingTime, (char) o.Status,
                    string.Empty);

      DataWriter.Execute(op);
    }


    static internal void WriteStorageFile(StorageFile o) {
      var op = DataOperation.Parse("writeEXFStorageItem",
                    o.Id, o.UID, o.GetEmpiriaType().Id,
                    o.AppContentType, o.MIMEContentType, o.Size,
                    o.Container.Storage.Id, o.Container.Id, -1, o.FilePath,
                    o.Name, o.OriginalFileName, o.Keywords, o.ExtensionData.ToString(),
                    o.HashCode, o.PostedBy.Id, o.PostingTime, (char) o.Status,
                    o.Integrity.GetUpdatedHashCode());

      DataWriter.Execute(op);
    }

  }  // class StorageData

}  // namespace Empiria.Storage
