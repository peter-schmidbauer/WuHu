﻿using System.Data;
using System.Data.Common;

namespace WuHu.Dal.Common
{
    public interface IDatabase
    {
        DbCommand CreateCommand(string commandText);
        int DeclareParameter(DbCommand command, string name, DbType type);
        void SetParameter(DbCommand command, string name, object value);
        void DefineParameter(DbCommand command, string name, DbType type, object value);

        IDataReader ExecuteReader(DbCommand command);
        int ExecuteNonQuery(DbCommand command);
        int ExecuteScalar(DbCommand command);
    }
}
