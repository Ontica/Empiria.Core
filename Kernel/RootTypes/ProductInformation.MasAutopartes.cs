﻿/* Empiria® Foundation Framework 2013 ************************************************************************
*																																																						 *
*	 Solution  : Empiria® Foundation Framework                    System   : Foundation Framework Library      *
*	 Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*	 Type      : ProductInformation                               Pattern  : Static class                      *
*	 Date      : 28/Mar/2013                                      Version  : 5.1      Pattern version: 2.0     *
*																																																						 *
*  Summary   : Static class that holds Empiria® product information.                                         *
*																																																						 *
**************************************************** Copyright © La Vía Óntica SC & Ontica LLC. 1994-2013. **/
using System;
using System.Reflection;

namespace Empiria {
  
  /// <summary>Static class that holds Empiria® product information.</summary>
  static public class ProductInformation {

    #region Fields

    static private readonly DateTime compilationDate;
    static private readonly string version;

    #endregion Fields

    #region Constructors and parsers

    static ProductInformation() {
      Assembly assembly = Assembly.GetExecutingAssembly();

      compilationDate = new System.IO.FileInfo(assembly.Location).CreationTime;
      version = assembly.GetName().Version.ToString();
    }

    #endregion Constructors and parsers

    #region Public properties

    static public DateTime CompilationDate {
      get { return compilationDate; }
    }

    static public string Copyright {
      get { return "Copyright © " + DateTime.Today.Year + "-1994. " + "La Vía Óntica SC / Ontica LLC"; }
    }

    static public string CopyrightUrl {
      get { return "http://www.ontica.org/"; }
    }

    static public string Description {
      get { return "Sistema de suministro y comercio en red"; }
    }

    static public string Name {
      get { return "Empiria® Trade 2013"; }
    }

    static public string Url {
      get { return "http://empiria.ontica.org/trade/"; }
    }

    static public string Version {
      get { return version; }
    }

    #endregion Public properties

  } // class ProductInformation

} // namespace Empiria