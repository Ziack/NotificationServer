using Insight.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.SqlServer.Config
{
    public class CommandMapper<TEntity>
    {
        private IDictionary<string, Func<TEntity, object>> _paramsConfig = new Dictionary<string, Func<TEntity, object>>();

        public string Sql { get; private set; }

        public CommandType CommandType { get; private set; }

        public ExecutionType ExecutionType { get; set; }

        public CommandMapper(string sql, CommandType type = CommandType.StoredProcedure)
        {
            Sql = sql;
            CommandType = type;
        }

        public CommandMapper<TEntity> AsCommand()
        {
            ExecutionType = ExecutionType.Command;
            return this;
        }

        public CommandMapper<TEntity> AsQuery()
        {
            ExecutionType = ExecutionType.Query;
            return this;
        }

        public CommandMapper<TEntity> Param(string paramName, Func<TEntity, object> paramValue)
        {
            _paramsConfig[paramName] = paramValue;
            return this;
        }

        public FastExpando BuildParameters(TEntity entity)
        {
            var expando = new FastExpando();

            foreach (var param in _paramsConfig)
                expando[param.Key] = param.Value(entity);

            return expando;
        }
    }

    public enum ExecutionType
    {
        Command,
        Query
    }
}
/*
new CommandMapper<Spec>("some.Sp")
    .Param("MyParam", v => v.prop)
    .Param("MyParam", v => v.prop)
    .Param("MyParam", v => v.prop);



*/
