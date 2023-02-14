namespace FastDeliveryAPI.Repositories.Interfaces;

public interface IUnitOfWorks
{
    Task SaveChangeAsync(CancellationToken cancellationToken = default);
}
