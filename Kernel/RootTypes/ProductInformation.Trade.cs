/* Empiria Foundation Framework 2014 *************************************************************************
*																																																						 *
*	 Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*	 Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*	 Type      : ProductInformation                               Pattern  : Static class                      *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*																																																						 *
*  Summary   : Static class that holds Empiria product information.                                          *
*																																																						 *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>Static class that holds Empiria product information.</summary>
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
      get {
        return "Copyright © 1999-" + DateTime.Today.Year + ". La Vía Óntica SC, Ontica LLC y colaboradores.";
      }
    }

    static public string CopyrightUrl {
      get { return "http://www.ontica.org/"; }
    }

    static public string Description {
      get { return "Sistema de suministro y comercio en red"; }
    }

    static public string Name {
      get { return "Empiria Trade 2014"; }
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