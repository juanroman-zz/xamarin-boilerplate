using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Boilerplate.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Boilerplate.Services.Facade
{
    public abstract class OfflineDataManager<TModel> : IOfflineDataManager<TModel> where TModel : ModelBase
    {
        private readonly IMobileServiceSyncContext _mobileServiceSyncContext;
        private readonly IMobileServiceSyncTable<TModel> _mobileServiceSyncTable;

        protected OfflineDataManager(
            IMobileServiceSyncContext mobileServiceSyncContext,
            IMobileServiceSyncTable<TModel> mobileServiceSyncTable)
        {
            _mobileServiceSyncTable = mobileServiceSyncTable;
            _mobileServiceSyncContext = mobileServiceSyncContext;
        }

        public abstract string Identifier { get; }

        public async Task<IList<TModel>> GetItemsAsync(bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                await PullLatestAsync();
            }

            return await _mobileServiceSyncTable.ToListAsync();
        }

        public async Task<TModel> GetItemAsync(string id, bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                await PullLatestAsync();
            }

            return await _mobileServiceSyncTable.LookupAsync(id);
        }

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

            await _mobileServiceSyncTable.InsertAsync(item);

            var success = await SyncAsync();
            if (success)
            {
                var updated = await GetItemAsync(item.Id, false);
                item.Id = updated.Id;
                item.CreatedAt = updated.CreatedAt;
                item.UpdatedAt = updated.UpdatedAt;
                item.Version = updated.Version;
            }

            return success;
        }

        public async Task<bool> UpdateAsync(TModel item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await _mobileServiceSyncTable.UpdateAsync(item);

            var success = await SyncAsync();
            if (success)
            {
                var updated = await GetItemAsync(item.Id, false);
                item.UpdatedAt = updated.UpdatedAt;
                item.Version = updated.Version;
            }

            return false;
        }

        public Task DeleteAsync(TModel item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return _mobileServiceSyncTable.DeleteAsync(item);
        }

        public Task PullLatestAsync()
        {
            return _mobileServiceSyncTable.PullAsync($"all{Identifier}", _mobileServiceSyncTable.CreateQuery());
        }

        public Task PurgeAsync()
        {
            return _mobileServiceSyncTable.PurgeAsync(true);
        }

        public async Task<bool> SyncAsync()
        {
            try
            {
                await _mobileServiceSyncContext.PushAsync();
                await PullLatestAsync();

                return true;
            }
            catch (MobileServicePushFailedException exception)
            {
                if (null != exception.PushResult)
                {
                    foreach (var error in exception.PushResult.Errors)
                    {
                        if (MobileServiceTableOperationKind.Update == error.OperationKind && null != error.Result)
                        {
                            // Update failed, revert to server's copy
                            await error.CancelAndUpdateItemAsync(error.Result);
                        }
                        else
                        {
                            // Discard local change
                            await error.CancelAndDiscardItemAsync();
                        }
                    }
                }

                return false;
            }
        }
    }
}
