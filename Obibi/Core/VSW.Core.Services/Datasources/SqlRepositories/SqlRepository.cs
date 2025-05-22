using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;
using LinqToDB;
using System.Net.WebSockets;
using LinqToDB.Data;
using LinqToDB.Mapping;
using LinqToDB.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Threading.Tasks;

namespace VSW.Core.Services
{
    public class SqlRepository
    {
        protected DataProvider _dataProvider;

        private ISqlDataHandle _sqlDataHandler;

        protected IWorkingContext Context { get; set; }

        public string DbName { get; protected set; }

        protected DatasourceItem Datasource { get; set; }

        protected List<ConnectionItem> Connections { get; set; }

        public SqlRepositoryOptions Options { get; protected set; }

        public virtual UnitDatasource GetUnitDataSource(UnitOfWork uow = null, ReadMode mode = ReadMode.Master)
        {
            UnitDatasource rs;
            if (uow != null && uow.Sources.Contains(Datasource.Name))
            {
                rs = uow.Sources[Datasource.Name];
            }
            else
            {
                var connection = _dataProvider.CreateDataConnection(mode);

                rs = new UnitDatasource
                {
                    Connection = connection,
                    Name = Datasource.Name
                };
            }
            rs.Connection.Connection.EnsureOpen();
            return rs;
        }

        public SqlRepository(string dbName, SqlRepositoryOptions options, IWorkingContext context = null)
        {
            Options = options;
            if (Options == null)
            {
                Options = new SqlRepositoryOptions();
            }

            DbName = dbName;
            Context = context;
            InitConfig();
        }

        protected virtual void InitConfig()
        {
            var item = DatasourceExtensions.GetDatasouceWithConnections(DbName);
            Datasource = item.Item1;
            Connections = item.Item2;
            _dataProvider = new DataProvider(DbName, Datasource, Connections);

            _sqlDataHandler = CoreService.UseDependency ? CoreService.ServiceProvider.GetService<ISqlDataHandle>() : null;
        }


        #region Execute Method

        public virtual int ExecuteNonQuery(string sqlText, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var command = _dataProvider.GetCommand(cnn.Connection, sqlText, parameters);
                return command.Execute();
            }
        }

        public virtual async Task<int> ExecuteNonQueryAsync(string sqlText, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var command = _dataProvider.GetCommand(cnn.Connection, sqlText, parameters);
                return await command.ExecuteAsync();
            }
        }


        public virtual int ExecuteStoredProc(string procName, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var command = _dataProvider.GetCommand(cnn.Connection, procName, parameters);
                return command.ExecuteProc();
            }
        }

        public virtual async Task<int> ExecuteStoredProcAsync(string procName, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var command = _dataProvider.GetCommand(cnn.Connection, procName, parameters);
                return await command.ExecuteProcAsync();
            }
        }

        public virtual T Execute<T>(string sqlText, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var command = _dataProvider.GetCommand(cnn.Connection, sqlText, parameters);
                var rs = command.Execute<T>();
                return rs;
            }
        }

        public virtual async Task<T> ExecuteAsync<T>(string sqlText, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var command = _dataProvider.GetCommand(cnn.Connection, sqlText, parameters);
                var rs = await command.ExecuteAsync<T>();
                return rs;
            }
        }


        public virtual T ExecuteStoredProc<T>(string procName, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var command = _dataProvider.GetCommand(cnn.Connection, procName, parameters);
                var rs = command.ExecuteProc<T>();
                return rs;
            }
        }

        public virtual async Task<T> ExecuteStoredProcAsync<T>(string procName, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var command = _dataProvider.GetCommand(cnn.Connection, procName, parameters);
                var rs = await command.ExecuteProcAsync<T>();
                return rs;
            }
        }

        public virtual List<T> Query<T>(string sqlText, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var rs = cnn.Connection.Query<T>(sqlText, parameters).ToList();
                return rs;
            }
        }

        public virtual async Task<List<T>> QueryAsync<T>(string sqlText, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var rs = await cnn.Connection.QueryToListAsync<T>(sqlText, parameters);
                return rs;
            }
        }

        public virtual List<T> QueryStoredProc<T>(string procName, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var rs = cnn.Connection.QueryProc<T>(procName, parameters).ToList();
                return rs;
            }
        }

        public virtual async Task<List<T>> QueryStoredProcAsync<T>(string procName, DataParameter[] parameters, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                ProcessUpdateValues(parameters);
                var rs = (await cnn.Connection.QueryProcAsync<T>(procName, parameters)).ToList();
                return rs;
            }
        }

        #endregion

        #region Handler Method
        protected void ProcessUpdateValue(object obj)
        {
            if (_sqlDataHandler == null || obj == null)
            {
                return;
            }

            _sqlDataHandler.HandleUpdateValue(obj);
        }

        protected void ProcessUpdateValues(IEnumerable lst)
        {
            if (_sqlDataHandler == null || lst == null)
            {
                return;
            }

            foreach (var obj in lst)
                _sqlDataHandler.HandleUpdateValue(obj);
        }

        #endregion
    }

    public class SqlRepository<TEntity, TPrimaryKey> : SqlRepository where TEntity : class
    {

        protected EntityDescriptor EntityDescriptor { get; set; }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> GetTable(ReadMode mode = ReadMode.Master, UnitOfWork uow = null)
        {
            return _dataProvider.GetTable<TEntity>(mode, uow).With("NOLOCK");
        }

        protected ColumnDescriptor PrimaryKeyField { get; set; }

        protected bool HasIdentity
        {
            get { return PrimaryKeyField != null && PrimaryKeyField.IsIdentity; }
        }

        public SqlRepository(string dbName, SqlRepositoryOptions options, IWorkingContext context = null) : base(dbName, options, context)
        {
            EntityDescriptor = _dataProvider.GetEntityDescriptor<TEntity>();
            PrimaryKeyField = EntityDescriptor.Columns.FirstOrDefault(x => x.IsPrimaryKey);
        }

        public TEntity Get(TPrimaryKey key, UnitOfWork uow = null)
        {
            var exp = ExpressionExtensions.BuildPrimaryEqual<TEntity>(PrimaryKeyField.MemberName, key);
            var rs = GetTable(uow: uow).Where(exp).FirstOrDefault();
            return rs;
        }
        public async Task<TEntity> GetAsync(TPrimaryKey key, UnitOfWork uow = null)
        {
            var exp = ExpressionExtensions.BuildPrimaryEqual<TEntity>(PrimaryKeyField.MemberName, key);
            var rs = await GetTable(uow: uow).Where(exp).FirstOrDefaultAsync();
            return rs;
        }

        public bool Insert(TEntity entity, UnitOfWork uow = null)
        {
            ProcessUpdateValue(entity);
            using (var cnn = GetUnitDataSource(uow))
            {
                if (HasIdentity)
                {
                    var propId = entity.GetProperty(PrimaryKeyField.MemberName);
                    if (propId.GetPropertyType() == typeof(int))
                    {
                        var id = cnn.Connection.InsertWithInt32Identity(entity);
                        entity.SetPropValue(PrimaryKeyField.MemberName, id);
                    }
                    else
                    {
                        var id = cnn.Connection.InsertWithInt64Identity(entity);
                        entity.SetPropValue(PrimaryKeyField.MemberName, id);
                    }

                }
                else
                {
                    cnn.Connection.Insert(entity);
                }

                return true;
            }
        }

        public async Task<bool> InsertAsync(TEntity entity, UnitOfWork uow = null)
        {
            ProcessUpdateValue(entity);
            using (var cnn = GetUnitDataSource(uow))
            {
                if (HasIdentity)
                {
                    var propId = entity.GetProperty(PrimaryKeyField.MemberName);
                    if (propId.GetPropertyType() == typeof(int))
                    {
                        var id = cnn.Connection.InsertWithInt32Identity(entity);
                        entity.SetPropValue(PrimaryKeyField.MemberName, id);
                    }
                    else
                    {
                        var id = cnn.Connection.InsertWithInt64Identity(entity);
                        entity.SetPropValue(PrimaryKeyField.MemberName, id);
                    }

                }
                else
                {
                    await cnn.Connection.InsertAsync(entity);
                }

                return true;
            }
        }

        public bool BulkInsert(List<TEntity> list, UnitOfWork uow = null)
        {
            ProcessUpdateValues(list);
            using (var cnn = GetUnitDataSource(uow))
            {
                cnn.Connection.BulkCopy(list);
                return true;
            }
        }

        public async Task<bool> BulkInsertAsync(List<TEntity> list, UnitOfWork uow = null)
        {
            ProcessUpdateValues(list);
            using (var cnn = GetUnitDataSource(uow))
            {
                await cnn.Connection.BulkCopyAsync(list);
                return true;
            }
        }

        public bool Update(TEntity entity, UnitOfWork uow = null)
        {
            ProcessUpdateValue(entity);
            using (var cnn = GetUnitDataSource(uow))
            {
                cnn.Connection.Update(entity);
                return true;
            }
        }

        public async Task<bool> UpdateAsync(TEntity entity, UnitOfWork uow = null)
        {
            ProcessUpdateValue(entity);
            using (var cnn = GetUnitDataSource(uow))
            {
                await cnn.Connection.UpdateAsync(entity);
                return true;
            }
        }

        public bool BulkUpdate(List<TEntity> list, UnitOfWork uow = null)
        {
            ProcessUpdateValues(list);
            using (var cnn = GetUnitDataSource(uow))
            {
                foreach (var entity in list)
                {
                    cnn.Connection.Update(entity);
                }
                return true;
            }
        }

        public async Task<bool> BulkUpdateAsync(List<TEntity> list, UnitOfWork uow = null)
        {
            ProcessUpdateValues(list);
            using (var cnn = GetUnitDataSource(uow))
            {
                foreach (var entity in list)
                {
                    await cnn.Connection.UpdateAsync(entity);
                }
                return true;
            }
        }

        public bool UpdateWhere(Expression<Func<TEntity, bool>> where, object parameter, UnitOfWork uow = null)
        {
            ProcessUpdateValue(parameter);
            GetTable(uow: uow).Update(where, entity => entity);
            return true;
        }

        public async Task<bool> UpdateWhereAsync(Expression<Func<TEntity, bool>> where, object parameter, UnitOfWork uow = null)
        {
            ProcessUpdateValue(parameter);
            await GetTable(uow: uow).UpdateAsync(where, entity => entity);
            return true;
        }

        public bool UpdateColumn(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> setter, UnitOfWork uow = null)
        {
            GetTable(uow: uow).Update(where, setter);
            return true;
        }

        public async Task<bool> UpdateColumnAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> setter, UnitOfWork uow = null)
        {
            await GetTable(uow: uow).UpdateAsync(where, setter);
            return true;
        }

        public bool Delete(TEntity entity, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                cnn.Connection.Delete(entity);
                return true;
            }
        }

        public async Task<bool> DeleteAsync(TEntity entity, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                await cnn.Connection.DeleteAsync(entity);
                return true;
            }
        }

        public bool BulkDelete(List<TEntity> list, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                foreach (var entity in list)
                {
                    cnn.Connection.Delete(entity);
                }
                return true;
            }
        }

        public async Task<bool> BulkDeleteAsync(List<TEntity> list, UnitOfWork uow = null)
        {
            using (var cnn = GetUnitDataSource(uow))
            {
                foreach (var entity in list)
                {
                    await cnn.Connection.DeleteAsync(entity);
                }
                return true;
            }
        }

        public bool DeleteWhere(Expression<Func<TEntity, bool>> where, UnitOfWork uow = null)
        {
            GetTable(uow: uow).Delete(where);
            return true;
        }

        public async Task<bool> DeleteWhereAsync(Expression<Func<TEntity, bool>> where, UnitOfWork uow = null)
        {
            await GetTable(uow: uow).DeleteAsync(where);
            return true;
        }

    }
}
