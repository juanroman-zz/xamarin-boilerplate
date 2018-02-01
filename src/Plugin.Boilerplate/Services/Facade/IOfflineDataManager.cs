using Plugin.Boilerplate.Models;
using System.Threading.Tasks;

namespace Plugin.Boilerplate.Services.Facade
{
    public interface IOfflineDataManager<TModel> : IDataManager<TModel> where TModel : ModelBase
    {
        Task PullLatestAsync();

        Task PurgeAsync();

        Task<bool> SyncAsync();
    }
}
