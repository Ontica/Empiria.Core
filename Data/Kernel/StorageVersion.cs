/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : StorageVersion                                   Pattern  : Value Type                        *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Value type that represents IStorable object's version data.                                   *
*                                                                                                            *
********************************* Copyright (c) 1999-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  public struct StorageVersion {

    #region Fields

    private long onCreateTimestamp;
    private long lastUpdateTimestamp;
    private int lastUpdatedById;

    #endregion Fields

    #region Constructors and parsers

    private StorageVersion(long onCreateTimestamp, long lastUpdateTimestamp, int lastUpdatedById) {
      this.onCreateTimestamp = onCreateTimestamp;
      this.lastUpdateTimestamp = lastUpdateTimestamp;
      this.lastUpdatedById = lastUpdatedById;
    }

    static public StorageVersion Create() {
      long timestamp = DateTime.Now.Ticks;

      return new StorageVersion(timestamp, timestamp, ExecutionServer.CurrentUserId);
    }

    //Creates a new StorageVersion instance with the last update timestamp and user updated.
    public StorageVersion Update() {
      return new StorageVersion(this.onCreateTimestamp, DateTime.Now.Ticks, ExecutionServer.CurrentUserId);
    }

    #endregion Constructors and parsers

    #region Properties

    public long OnLoadVersion {
      get {
        return this.onCreateTimestamp;
      }
    }

    public int UpdatedById {
      get {
        return (this.lastUpdatedById);
      }
    }

    public long Value {
      get {
        return this.lastUpdateTimestamp;
      }
    }

    public bool WasUpdated {
      get {
        return (this.onCreateTimestamp != this.lastUpdateTimestamp);
      }
    }

    #endregion Properties

    #region Operators overloading

    static public bool operator ==(StorageVersion versionA, StorageVersion versionB) {
      return (versionA.onCreateTimestamp == versionB.onCreateTimestamp &&
              versionA.lastUpdateTimestamp == versionB.lastUpdateTimestamp);
    }

    static public bool operator !=(StorageVersion versionA, StorageVersion versionB) {
      return !(versionA == versionB);
    }

    #endregion Operators overloading

    #region Public methods

    public override bool Equals(object o) {
      if (!(o is StorageVersion)) {
        return false;
      }
      StorageVersion temp = (StorageVersion) o;

      return (this.onCreateTimestamp == temp.onCreateTimestamp &&
              this.lastUpdateTimestamp == temp.lastUpdateTimestamp);
    }

    public override int GetHashCode() {
      return (this.lastUpdateTimestamp.GetHashCode());
    }

    public override string ToString() {
      return "Version " + lastUpdateTimestamp.ToString();
    }

    #endregion Public methods

  }  // struct StorageVersion

}  // namespace Empiria 
