/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : ObjectIdFactory                                  Pattern  : Singleton Class                   *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Retrives and holds the objects unique identificators (object ids) per domain.                 *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Empiria.Data {

  //[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "8b7fe9c60c0f43bd")]
  internal sealed class ObjectIdFactory {

    #region Fields

    static private ObjectIdFactory instance = null;
    static private Mutex mutex = null;

    static private readonly string mutexName = "{Empiria.ObjectIdFactory}.{" + Guid.NewGuid().ToString() + "}";

    private Dictionary<string, ObjectIdRule> loadedRules = new Dictionary<string, ObjectIdRule>();

    #endregion Fields

    #region Constructors and parsers

    private ObjectIdFactory() {
      // Object creation of singleton classes not allowed
    }

    static public ObjectIdFactory Instance {
      get {
        try {
          if (mutex == null) {
            CreateMutex();
          }
          if (mutex.WaitOne()) {
            if (instance == null) {
              instance = new ObjectIdFactory();
            }
            return instance;
          } else {
            throw new EmpiriaDataException(EmpiriaDataException.Msg.DuplicateObjectIdFactory);
          }
        } catch {
          throw;
        } finally {
          mutex.ReleaseMutex();
        } // try
      } // get
    }

    #endregion Constructors and parsers

    #region Internal methods

    internal int GetNextId(string sourceName, int typeId) {
      try {
        if (mutex.WaitOne()) {
          string hashCode = sourceName + "." + typeId.ToString();
          hashCode = hashCode.ToUpperInvariant();
          ObjectIdRule rule = null;
          if (!loadedRules.TryGetValue(hashCode, out rule)) {
            rule = ObjectIdRule.Parse(sourceName, typeId);
            loadedRules.Add(hashCode, rule);
          }
          return rule.GetNextId();
        } else {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.DuplicateObjectIdFactory);
        }
      } catch {
        throw;
      } finally {
        mutex.ReleaseMutex();
      }
    }

    internal void Reset() {
      try {
        mutex.WaitOne();
        loadedRules.Clear();
      } catch {
        throw;
      } finally {
        mutex.ReleaseMutex();
      }
    }

    #endregion Internal methods

    #region Private methods

    static private void CreateMutex() {
      try {
        bool createdNew = false;
        mutex = new Mutex(false, mutexName, out createdNew);
        if (!createdNew) {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.DuplicateObjectIdFactory);
        }
        GC.KeepAlive(mutex);
      } catch (EmpiriaDataException) {
        GC.Collect();
        throw;
      } catch (Exception e) {
        GC.Collect();
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DuplicateObjectIdFactory, e);
      }
    }

    #endregion Private methods

    #region Inner Class ObjectIdRule

    private class ObjectIdRule {

      #region Fields

      private int fromId;
      private int toId;
      private int currentId;

      #endregion Fields

      #region Constructors and parsers

      private ObjectIdRule(int fromId, int toId, int currentId) {
        this.fromId = fromId;
        this.toId = toId;
        this.currentId = currentId;
      }

      static internal ObjectIdRule Parse(string sourceName, int typeId) {
        DataRow ruleDataRow = GetDataRuleRow(sourceName, typeId);

        if (ruleDataRow == null) {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.ObjectIdRuleNotSet, sourceName + "." + typeId);
        }
        int lowerIdValue = (int) ruleDataRow["LowerIdValue"];
        int upperIdValue = (int) ruleDataRow["UpperIdValue"];
        int currentId = RetriveCurrentId(ruleDataRow);

        return new ObjectIdRule(lowerIdValue, upperIdValue, Math.Max(lowerIdValue, currentId));
      }

      #endregion Constructors and parsers

      #region Internal methods

      internal int GetNextId() {
        currentId++;
        if ((fromId <= currentId) && (currentId <= toId)) {
          return currentId;
        } else {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.ObjectIdOutOfValidBounds, currentId);
        }
      }

      #endregion Internal methods

      #region Private methods

      static private DataRow GetDataRuleRow(string sourceName, int typeId) {
        DataOperation operation = DataOperation.Parse("qryDBRule", 'I', ExecutionServer.ServerId, sourceName, typeId);

        return DataReader.GetDataRow(operation);
      }

      static private int RetriveCurrentId(DataRow ruleDataRow) {
        string idFieldName = (string) ruleDataRow["IdFieldName"];

        string sql = "SELECT MAX([" + idFieldName + "]) FROM " + (string) ruleDataRow["SourceName"] +
                     " WHERE ([" + idFieldName + "] >= " + (int) ruleDataRow["LowerIdValue"] + ") AND" +
                     " ([" + idFieldName + "] <= " + (int) ruleDataRow["UpperIdValue"] + ")";
        if ((int) ruleDataRow["TypeId"] != 0) {
          sql += " AND ([" + (string) ruleDataRow["IdTypeFieldName"] + "] = " + (int) ruleDataRow["TypeId"] + ")";
        }
        DataSource dataSource = DataSource.Parse((string) ruleDataRow["SourceName"]);
        
        return DataReader.GetScalar<int>(DataOperation.Parse(dataSource, sql));
      }

      #endregion Private methods

    } // class ObjectIdRule

    #endregion Inner Class ObjectIdRule

  } // class ObjectId

} //namespace Empiria.Data