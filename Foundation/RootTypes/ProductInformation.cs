/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ProductInformation                               Pattern  : Static class                      *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*																																																						 *
*  Summary   : Static class that holds Empiria product information.                                          *
*																																																						 *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>Static class that holds Empiria product information.</summary>
  static public class ProductInformation {

    #region Constructors and parsers

    static ProductInformation() {
      Assembly assembly = Assembly.GetExecutingAssembly();
      var type = typeof(ProductInformation);

      ProductInformation.CompilationDate = new System.IO.FileInfo(assembly.Location).CreationTime;
      ProductInformation.Version = assembly.GetName().Version.ToString();


      ProductInformation.Description =  ConfigurationData.Get(type, "System.Description", String.Empty);
      ProductInformation.Name = ConfigurationData.Get(type, "System.Name", String.Empty);
      ProductInformation.Url = ConfigurationData.Get(type, "System.Url", String.Empty);
    }

    #endregion Constructors and parsers

    #region Public properties

    static public DateTime CompilationDate {
      get;
      private set;
    }

    static public string Copyright {
      get {
        return "Copyright " + DateTime.Today.Year + ". La Vía Óntica SC, Ontica LLC y colaboradores.";
      }
    }

    static public string CopyrightUrl {
      get { return "http://www.ontica.org/"; }
    }

    static public string Description {
      get;
      private set;
    }

    static public string Name {
      get;
      private set;
    }

    static public string Url {
      get;
      private set;
    }

    static public string Version {
      get;
      private set;
    }

    #endregion Public properties

  } // class ProductInformation

} // namespace Empiria
