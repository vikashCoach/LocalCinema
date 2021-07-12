using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace LocalCinema.Data.Repository.Interfaces
{
    public interface IDbConnectionContext : IDisposable
    {
        
        IDbConnection GetOpenConnection(string connectionString = null);
    }
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Executes a unit of work wrapped in a transaction.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to execute</param>
        /// <param name="isolationLevel">Specify transaction isolation level</param>
        /// <returns></returns>
        Task<T> ExecuteWithTransactionAsync<T>(Func<IDbConnection, Action, Action, Task<T>> unitOfWork,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Executes a unit of work wrapped in a transaction.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        Task ExecuteWithTransactionAsync(Func<IDbConnection, Action, Action, Task> unitOfWork,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
