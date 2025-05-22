using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services
{
    public enum SqlCommandType
    {
        SqlText = 0,
        SqlStoredProc = 1
    }

    public class SqlCommandExecutor
    {
        public SqlCommandType Type { get; set; }

        public string SqlText { get; set; }

        public List<DataParameter> Parameters { get; set; }

        protected SqlRepository Repo { get; set; }

        public SqlCommandExecutor(string sqlText, SqlCommandType type, SqlRepository repo)
        {
            SqlText = sqlText;
            Type = type;
            Repo = repo;
            Parameters = new List<DataParameter>();
        }

        public SqlCommandExecutor AddParameter(string name, object value, DataType? type = null)
        {
            if (type == null)
                Parameters.Add(new DataParameter(name, value));
            else
                Parameters.Add(new DataParameter(name, value, type.Value));

            return this;
        }

        public SqlCommandExecutor AddParameter(DataParameter parameter)
        {
            Parameters.Add(parameter);
            return this;
        }

        public SqlCommandExecutor AddParameter(List<DataParameter> parameters)
        {
            for (int i = 0; parameters.IsNotEmpty() && i < parameters.Count; i++)
            {
                Parameters.Add(parameters[i]);
            }
            return this;
        }

        public int ExecuteNonQuery(UnitOfWork uow = null)
        {
            if (Type == SqlCommandType.SqlText)
                return Repo.ExecuteNonQuery(SqlText, Parameters.ToArray(), uow);
            else
                return Repo.ExecuteStoredProc(SqlText, Parameters.ToArray(), uow);
        }

        public async Task<int> ExecuteNonQueryAsync(UnitOfWork uow = null)
        {
            if (Type == SqlCommandType.SqlText)
                return await Repo.ExecuteNonQueryAsync(SqlText, Parameters.ToArray(), uow);
            else
                return await Repo.ExecuteStoredProcAsync(SqlText, Parameters.ToArray(), uow);
        }

        public virtual T ToSingle<T>(UnitOfWork uow = null)
        {
            if (Type == SqlCommandType.SqlText)
                return Repo.Execute<T>(SqlText, Parameters.ToArray(), uow);
            else
                return Repo.ExecuteStoredProc<T>(SqlText, Parameters.ToArray(), uow);
        }

        public virtual async Task<T> ToSingleAsync<T>(UnitOfWork uow = null)
        {
            if (Type == SqlCommandType.SqlText)
                return await Repo.ExecuteAsync<T>(SqlText, Parameters.ToArray(), uow);
            else
                return await Repo.ExecuteStoredProcAsync<T>(SqlText, Parameters.ToArray(), uow);
        }

        public virtual List<T> Query<T>(UnitOfWork uow = null)
        {
            if (Type == SqlCommandType.SqlText)
                return Repo.Query<T>(SqlText, Parameters.ToArray(), uow);
            else
                return Repo.QueryStoredProc<T>(SqlText, Parameters.ToArray(), uow);
        }

        public virtual async Task<List<T>> QueryAsync<T>(UnitOfWork uow = null)
        {
            if (Type == SqlCommandType.SqlText)
                return await Repo.QueryAsync<T>(SqlText, Parameters.ToArray(), uow);
            else
                return await Repo.QueryStoredProcAsync<T>(SqlText, Parameters.ToArray(), uow);
        }
    }
}
