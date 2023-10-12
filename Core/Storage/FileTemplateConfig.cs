﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounting                       Component : Common Types                            *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Information Holder                      *
*  Type     : FileTemplateConfig                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds configuration for file templates.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.IO;

namespace Empiria.Storage {

  /// <summary>Holds configuration for file templates.</summary>
  public class FileTemplateConfig : GeneralObject {

    #region Constructors and parsers

    protected FileTemplateConfig() {
      // Required by Empiria Framework
    }


    static public FileTemplateConfig Parse(int id) {
      return BaseObject.ParseId<FileTemplateConfig>(id);
    }


    static public FileTemplateConfig Parse(string uid) {
      return BaseObject.ParseKey<FileTemplateConfig>(uid);
    }


    static public string GeneratedFilesBaseUrl {
      get {
        return ConfigurationData.Get<string>("Reports.BaseUrl");
      }
    }


    static public string GenerationStoragePath {
      get {
        return ConfigurationData.Get<string>("Reports.GenerationStoragePath");
      }
    }


    static private string TemplatesBaseUrl {
      get {
        return ConfigurationData.Get<string>("Reports.TemplatesBaseUrl");
      }
    }


    static public string TemplatesStoragePath {
      get {
        return ConfigurationData.Get<string>("Reports.TemplatesStoragePath");
      }
    }

    #endregion Constructors and parsers

    #region Properties

    private string OriginalFileName {
      get {
        return base.ExtendedDataField.Get<string>("fileName", this.TemplateFileName);
      }
    }


    private string TemplateFileName {
      get {
        return base.ExtendedDataField.Get<string>("templateFile");
      }
    }


    public string TemplateFullPath {
      get {
        return Path.Combine(TemplatesStoragePath, this.TemplateFileName);
      }
    }


    public string TemplateUrl {
      get {
        return $"{FileTemplateConfig.TemplatesBaseUrl}/{this.TemplateFileName}";
      }
    }


    public string Title {
      get {
        return base.ExtendedDataField.Get<string>("title", string.Empty);
      }
    }


    public int FirstRowIndex {
      get {
        return base.ExtendedDataField.Get<int>("firstRowIndex", 10);
      }
    }


    public string CurrentTimeCell {
      get {
        return base.ExtendedDataField.Get<string>("currentTimeCell", string.Empty);
      }
    }


    public string ReportDateCell {
      get {
        return base.ExtendedDataField.Get<string>("reportDateCell", string.Empty);
      }
    }


    public string ReportDateFormat {
      get {
        return base.ExtendedDataField.Get<string>("reportDateFormat", string.Empty);
      }
    }


    public string TitleCell {
      get {
        return base.ExtendedDataField.Get<string>("titleCell", string.Empty);
      }
    }


    public string SubTitleCell {
      get {
        return base.ExtendedDataField.Get<string>("subTitleCell", string.Empty);
      }
    }

    #endregion Properties

    #region Methods

    public string NewFilePath() {
      var copyFileName = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss-") + this.OriginalFileName;

      return Path.Combine(GenerationStoragePath, copyFileName);
    }

    #endregion Methods

  }  // class FileTemplateConfig

}  // namespace Empiria.FinancialAccounting
