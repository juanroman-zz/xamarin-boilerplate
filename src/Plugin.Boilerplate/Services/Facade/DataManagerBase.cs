using Microsoft.WindowsAzure.MobileServices;
using Plugin.Boilerplate.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Boilerplate.Services.Facade
{
    public abstract class DataManagerBase<TModel> : IDataManager<TModel> where TModel : ModelBase
    {
        private readonly IMobileServiceTable<TModel> _mobileServiceTable;

        protected DataManagerBase(IMobileServiceTable<TModel> mobileServiceTable)
        {
            _mobileServiceTable = mobileServiceTable;
        }

        public abstract string Identifier { get; }

        public async Task<IList<TModel>> GetItemsAsync(bool forceRefresh = true) => await _mobileServiceTable.ToListAsync();

        public Task<TModel> GetItemAsync(string id, bool forceRefresh = true) => _mobileServiceTable.LookupAsync(id);

        public Task<bool> SaveAsync(TModel item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (string.IsNullOrWhiteSpace(item.Id))
            {
                return InsertAsync(item);
            }

            return UpdateAsync(item);
        }

        public async Task<bool> InsertAsync(TModel item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await _mobileServiceTable.InsertAsync(item);

            var updated = await GetItemAsync(item.Id, true);
            item.Id = updated.Id;
            item.CreatedAt = updated.CreatedAt;
            item.UpdatedAt = updated.UpdatedAt;
            item.Version = updated.Version;

            return true;
        }

        public async Task<bool> UpdateAsync(TModel item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await _mobileServiceTable.UpdateAsync(item);

            var updated = await GetItemAsync(item.Id, true);
            item.UpdatedAt = updated.UpdatedAt;
            item.Version = updated.Version;

            return true;
        }

        public Task DeleteAsync(TModel item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return _mobileServiceTable.DeleteAsync(item);
        }
    }
}
