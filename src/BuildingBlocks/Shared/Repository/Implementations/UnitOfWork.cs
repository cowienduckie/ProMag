using System.Data;

namespace Shared.Repository.Implementations;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly Func<IDbConnection> _connFactory;
    private IDbConnection? _dbConnection;
    private IDbTransaction? _transaction;

    public UnitOfWork(Func<IDbConnection> connFactory)
    {
        _connFactory = connFactory;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        Connection.Close();
        Connection.Dispose();

        GC.SuppressFinalize(this);
    }

    public IDbConnection Connection => _dbConnection ??= _connFactory();

    public void BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        _transaction = Connection.BeginTransaction(level);
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
    }
}