namespace GenshinVybyu.Services.Interfaces
{
    public interface INgrokTunnel : IService
    {
        public Task<string?> CreateTunnel(string uri, CancellationToken cancellationToken);
        public Task CloseTunnel(CancellationToken cancellationToken);
    }
}
