using Azure.Core;
using Dapper;
using Ideageek.Subscribly.Core.Entities;
using Ideageek.Subscribly.Core.Entities.Administration;
using Ideageek.Subscribly.Core.Entities.UserManagement;
using Ideageek.Subscribly.Core.Helpers;
using Ideageek.Subscribly.Core.Repositories;
using System.Data;

namespace Ideageek.Subscribly.Core.Services.UserManagement
{
    public interface ISubscriberService
    {
        Task<Subscriber> GetSubscriberById(Guid adminId, Guid subscriberId);
        Task<IEnumerable<Subscriber>> GetAllSubscribersByAdminAndSubscriptionId(Guid adminId, Guid subscriptionId);
        Task<Subscriber> Add(Guid subscriberId, Guid adminId);
        Task<bool> RemoveSubscriberFromSubscription(Guid subscriberId, Guid subscriptionId);
    }
    public class SubscriberService : ISubscriberService
    {
        private readonly BaseRepository<Subscriber> _repository;
        private readonly IAppHelper _appHelper;
        public SubscriberService(IAppHelper appHelper)
        {
            _appHelper = appHelper;
            _repository = new BaseRepository<Subscriber>(_appHelper.GetConnectionString());
        }
        public async Task<Subscriber> GetSubscriberById(Guid adminId, Guid subscriberId)
        {
            var conditions = new { AdminId = adminId, SubscriberId = subscriberId };
            return await _repository.GetByConditionsAsync(conditions);
        }
        public async Task<IEnumerable<Subscriber>> GetAllSubscribersByAdminAndSubscriptionId(Guid adminId, Guid subscriptionId)
        {
            var conditions = new { AdminId = adminId, SubscriptionId = subscriptionId };
            return await _repository.GetAllByConditionsAsync(conditions);
        }
        public async Task<Subscriber> Add(Guid subscriberId, Guid adminId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SubscriptionId", subscriberId, DbType.Guid);
            parameters.Add("@CreatedBy", adminId, DbType.Guid);

            return await _repository.ExecuteSingleStoredProcedure("AddSubscriber", parameters);
        }
        public async Task<bool> RemoveSubscriberFromSubscription(Guid subscriberId, Guid subscriptionId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SubscriberId", subscriberId, DbType.Guid);
            parameters.Add("@SubscriptionId", subscriptionId, DbType.Guid);

            var response = await _repository.ExecuteSingleStoredProcedure("RemoveSubscriberFromSubscription", parameters);
            if (response != null)
                return true;
            else
                return false;
        }
    }
}
