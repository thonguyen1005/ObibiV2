using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace VSW.Core.Services
{
    public enum UnitOfWorkStatus
    {
        None,

        BeginWork,

        Committed,

        Rollbacked
    }

    public class UnitOfWork: IDisposable
    {
        public UnitDataSourceCollection Sources { get; set; }

        public UnitOfWorkStatus Status { get; protected set; }

        public bool AutoProcess { get; set; }

        public UnitOfWork(UnitDatasource[] items) : this(items, true)
        {
        }

        public UnitOfWork(UnitDatasource[] items, bool autoProcess)
        {
            Sources = new UnitDataSourceCollection();
            if (items.IsNotEmpty())
            {
                foreach (var item in items)
                {
                    AddSource(item);
                }
            }

            AutoProcess = autoProcess;
            AutoBeginWork();
        }

        public UnitOfWork(params SqlRepository[] repos) : this(repos, true)
        {
        }

        public UnitOfWork(SqlRepository[] repos, bool autoProcess)
        {
            Sources = new UnitDataSourceCollection();
            if (repos.IsNotEmpty())
            {
                foreach (var item in repos)
                {
                    AddSource(item.GetUnitDataSource(null));
                }
            }

            AutoProcess = autoProcess;
            AutoBeginWork();
        }

        private void AutoBeginWork()
        {
            if (AutoProcess)
            {
                BeginWork();
            }
        }

        public void AddSource(UnitDatasource source)
        {
            if (!Sources.Contains(source.Name))
            {
                source.UnitOfWork = this;
                Sources.AddIfNotExist(source);
                if (Status == UnitOfWorkStatus.BeginWork)
                {
                    source.BeginWork();
                }
            }
        }

        public void AddSource(SqlRepository repo)
        {
            AddSource(repo.GetUnitDataSource());
        }

        public void BeginWork()
        {
            if (Status == UnitOfWorkStatus.BeginWork)
            {
                return;
            }

            if (Status != UnitOfWorkStatus.None)
            {
                throw new Exception("[DEBUG] UnitOfWork is already begin");
            }

            foreach (var item in Sources)
            {
                item.BeginWork();
            }

            Status = UnitOfWorkStatus.BeginWork;
        }

        public void Commit()
        {
            if (Status == UnitOfWorkStatus.None)
            {
                throw new Exception("[DEBUG] UnitOfWork isn't begin");
            }

            if (Status == UnitOfWorkStatus.Committed || Status == UnitOfWorkStatus.Rollbacked)
            {
                throw new Exception("[DEBUG] UnitOfWork was committed or rollbacked");
            }

            foreach (var item in Sources)
            {
                item.Commit();
            }         

            Status = UnitOfWorkStatus.Committed;
        }

        private void AutoRollback()
        {
            if (AutoProcess)
            {
                if (Status == UnitOfWorkStatus.BeginWork)
                {
                    Rollback();
                }
            }
        }

        public void Dispose()
        {
            AutoRollback();

            foreach (var item in Sources)
            {
                item.UnitOfWork = null;
                item.Dispose();
            }
        }

        public void Rollback()
        {
            if (Status == UnitOfWorkStatus.None || Status == UnitOfWorkStatus.Rollbacked)
            {
                Status = UnitOfWorkStatus.Rollbacked;
                return;
            }

            if (Status == UnitOfWorkStatus.Committed)
            {
                throw new Exception("[DEBUG] UnitOfWork was committed");
            }

            foreach (var item in Sources)
            {
                item.Rollback();
            }

            Status = UnitOfWorkStatus.Rollbacked;
        }
    }


    public class UnitDataSourceCollection : KeyValueList<string, UnitDatasource>
    {
        public UnitDataSourceCollection() : base(x => x.Name)
        {

        }
    }

    public class UnitDatasource: IDisposable
    {
        public string Name { get; set; }

        public DataConnection Connection { get; internal set; }

        public UnitOfWork UnitOfWork { get; set; }

        public void BeginWork()
        {
            Connection.Connection.EnsureOpen();
            Connection.BeginTransaction();
        }

        public void Commit()
        {
            Connection.CommitTransaction();
        }

        public void Rollback()
        {
            Connection.RollbackTransaction();
        }

        public void Dispose()
        {
            if (UnitOfWork == null)
            {
                if (Connection != null)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
            }
        }
    }
}
