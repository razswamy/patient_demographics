using Dapper;
using System.Collections.Generic;
using System.Data;

namespace patient.demography.helpers
{
    public static class dbextensions
    {
        public static int Execute(this IDbConnection cnn, string sql, object param = null, IDbTransaction Transaction = null, CommandType? CommandType = null)
        {
            if (Transaction == null)
            {
                return cnn.Execute(sql, param == null ? new { } : param, commandType: CommandType);
            }
            else
            {
                return cnn.Execute(sql, param == null ? new { } : param, transaction: Transaction, commandType: CommandType);
            }
        }

        public static T ExecuteScalar<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction Transaction = null, CommandType? CommandType = null)
        {
            if (Transaction == null)
            {
                return cnn.ExecuteScalar<T>(sql, param == null ? new { } : param, commandType: CommandType);
            }
            else
            {
                return cnn.ExecuteScalar<T>(sql, param == null ? new { } : param, transaction: Transaction, commandType: CommandType);
            }
        }

        public static SqlMapper.GridReader QueryMultiple(this IDbConnection cnn, string sql, object param = null, IDbTransaction Transaction = null, CommandType? CommandType = null)
        {
            if (Transaction == null)
            {
                return cnn.QueryMultiple(sql, param == null ? new { } : param, commandType: CommandType);
            }
            else
            {
                return cnn.QueryMultiple(sql, param == null ? new { } : param, transaction: Transaction, commandType: CommandType);
            }
        }

        public static IEnumerable<T> Query<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction Transaction = null, CommandType? CommandType = null)
        {
            if (Transaction == null)
            {
                return cnn.Query<T>(sql, param == null ? new { } : param, commandType: CommandType);
            }
            else
            {
                return cnn.Query<T>(sql, param == null ? new { } : param, transaction: Transaction, commandType: CommandType);
            }
        }

        public static T QueryFirstOrDefault<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction Transaction = null, CommandType? CommandType = null)
        {
            if (Transaction == null)
            {
                return cnn.QueryFirstOrDefault<T>(sql, param == null ? new { } : param, commandType: CommandType);
            }
            else
            {
                return cnn.QueryFirstOrDefault<T>(sql, param == null ? new { } : param, transaction: Transaction, commandType: CommandType);
            }
        }

        public static T QuerySingle<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction Transaction = null, CommandType? CommandType = null)
        {
            if (Transaction == null)
            {
                return cnn.QuerySingle<T>(sql, param == null ? new { } : param, commandType: CommandType);
            }
            else
            {
                return cnn.QuerySingle<T>(sql, param == null ? new { } : param, transaction: Transaction, commandType: CommandType);
            }
        }
    }
}