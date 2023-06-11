/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ProductInformation                               Pattern  : Static class                      *
*																																																						 *
*  Summary   : Static class that holds Empiria product information.                                          *
*																																																						 *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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
