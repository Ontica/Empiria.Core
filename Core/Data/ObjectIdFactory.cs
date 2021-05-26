/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     License  : Please read LICENSE.txt file      *
*  Type      : ObjectIdFactory                                  Pattern  : Singleton Class                   *
*                                                                                                            *
*  Summary   : Retrives and holds the objects unique identificators (object ids) per domain.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;
using System.Threading;

using Empiria.Collections;

namespace Empiria.Data {

  internal sealed class ObjectIdFactory {

    #region Fields

    static private bool OBJECT_ID_FACTORY_USES_DECIMAL =
                                    ConfigurationData.Get("ObjectIdFactory.UsesDecimal", false);

    static private ObjectIdFactory instance = null;
    static private Mutex mutex = null;

    static private readonly string mutexName = "{Empiria.ObjectIdFactory}.{" + Guid.NewGuid().ToString() + "}";

    private EmpiriaDictionary<string, ObjectIdRule> loadedRules = new EmpiriaDictionary<string, ObjectIdRule>();

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
          if (!loadedRules.ContainsKey(hashCode)) {
            rule = ObjectIdRule.Parse(sourceName, typeId);
            loadedRules.Insert(hashCode, rule);
          } else {
            rule = loadedRules[hashCode];
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

      private readonly int fromId;
      private readonly int toId;
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
        int lowerIdValue = Convert.ToInt32(ruleDataRow["LowerIdValue"]);
        int upperIdValue = Convert.ToInt32(ruleDataRow["UpperIdValue"]);
        int retrievedCurrentId = Convert.ToInt32(RetriveCurrentId(ruleDataRow));

        return new ObjectIdRule(lowerIdValue, upperIdValue, Math.Max(lowerIdValue, retrievedCurrentId));
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

        string sql = $"SELECT MAX({idFieldName}) FROM {(string) ruleDataRow["SourceName"]} " +
                     $"WHERE ({idFieldName} >= {Convert.ToInt32(ruleDataRow["LowerIdValue"])}) AND " +
                     $"({idFieldName} <= {Convert.ToInt32(ruleDataRow["UpperIdValue"])})";

        if (Convert.ToInt32(ruleDataRow["TypeId"]) != 0) {
          sql += $" AND ({(string) ruleDataRow["IdTypeFieldName"]} = {Convert.ToInt32(ruleDataRow["TypeId"])})";
        }
        DataSource dataSource = DataSource.Parse((string) ruleDataRow["SourceName"]);

        if (OBJECT_ID_FACTORY_USES_DECIMAL) {
          return Convert.ToInt32(DataReader.GetScalar<decimal>(DataOperation.Parse(dataSource, sql)));
        } else {
          return DataReader.GetScalar<int>(DataOperation.Parse(dataSource, sql));
        }

      }

      #endregion Private methods

    } // class ObjectIdRule

    #endregion Inner Class ObjectIdRule

  } // class ObjectId

} //namespace Empiria.Data
