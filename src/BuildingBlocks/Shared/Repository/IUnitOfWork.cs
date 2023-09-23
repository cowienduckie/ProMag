using System.Data;

namespace Shared.Repository;

public interface IUnitOfWork
{
    IDbConnection Connection { get; }
    void BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted);
    void CommitTransaction();
    void RollbackTransaction();
}