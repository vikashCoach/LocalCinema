using LocalCinema.Data.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LocalCinema.Data.Repository
{
    public class LocalCinemaUnitOfWork : IUnitOfWork
    {
        private readonly ILogger _logger;
        private readonly IDbConnectionContext _dbConnectionContext;

        public LocalCinemaUnitOfWork(ILogger<LocalCinemaUnitOfWork> logger, IDbConnectionContext dbConnectionContext)
        {
            _logger = logger;
            _dbConnectionContext = dbConnectionContext;
        }

        public async Task ExecuteWithTransactionAsync(Func<IDbConnection, Action, Action,Task> unitOfWork,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var conn = _dbConnectionContext.GetOpenConnection();
            var trans = conn.BeginTransaction(isolationLevel);
            Action Commit = () => trans.Commit();
            Action Rollback = () => trans.Rollback();

            try
            {
                await unitOfWork(conn, Commit, Rollback);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError(ex, "Error executing unit of work");
                throw;
            }
        }

        public async Task<T> ExecuteWithTransactionAsync<T>(Func<IDbConnection, Action, Action, Task<T>> unitOfWork,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var conn = _dbConnectionContext.GetOpenConnection();
            var trans = conn.BeginTransaction(isolationLevel);
            Action Commit = () => trans.Commit();
            Action Rollback = () => trans.Rollback();

            try
            {
                var result = await unitOfWork(conn, Commit, Rollback);
                return result;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError(ex, "Error executing unit of work");
                throw;
            }
            finally
            {
                trans.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _dbConnectionContext != null)
                _dbConnectionContext.Dispose();
        }
    }
}
