using Plugin.Boilerplate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Boilerplate.Services.Facade
{
    public interface IDataManager<TModel> where TModel : ModelBase
    {
        string Identifier { get; }

        Task<IList<TModel>> GetItemsAsync(bool forceRefresh = true);

        Task<TModel> GetItemAsync(string id, bool forceRefresh = true);

        Task<bool> SaveAsync(TModel item);

        Task<bool> InsertAsync(TModel item);

        Task<bool> UpdateAsync(TModel item);

        Task DeleteAsync(TModel item);
    }
}
