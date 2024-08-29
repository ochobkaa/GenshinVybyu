namespace GenshinVybyu.Services.Interfaces
{
    public interface IRequestRepeater : IService
    {
        public Task<bool> Repeat(Func<CancellationToken, Task> action, int delay, int times, 
            CancellationToken cancellationToken);

        public Task<T?> Repeat<T>(Func<CancellationToken, Task<T>> action, int delay, int times,
            CancellationToken cancellationToken);
    }
}
