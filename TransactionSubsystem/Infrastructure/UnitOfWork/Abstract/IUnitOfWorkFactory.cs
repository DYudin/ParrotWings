
namespace TransactionSubsystem.Infrastructure.UnitOfWork.Abstract
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}