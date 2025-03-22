using Dapper;
using Ideageek.Subscribly.Core.Entities;
using Ideageek.Subscribly.Core.Entities.Administration;
using Ideageek.Subscribly.Core.Helpers;
using Ideageek.Subscribly.Core.Repositories;
using System.Data;

namespace Ideageek.Subscribly.Core.Services.Administration
{
    public interface ISubscriptionService
    {
        Task<Subscription> GetById(Guid id);
        Task<IEnumerable<Subscription>> GetAll();
        Task<IEnumerable<Subscription>> GetAllSubscriptionsById(Guid id);
        Task<Subscription> Add(AddSubscription request, Guid userId);
        Task<Subscription> Update(UpdateSubscription request);
        Task<int> Delete(Guid id);
    }
    public class SubscriptionService : ISubscriptionService
    {
        private readonly BaseRepository<Subscription> _repository;
        private readonly IAppHelper _appHelper;
        public SubscriptionService(IAppHelper appHelper)
        {
            _appHelper = appHelper;
            _repository = new BaseRepository<Subscription>(_appHelper.GetConnectionString());
        }
        public async Task<Subscription> GetById(Guid id)
        {
            return await _repository.GetById(id);
        }

        public async Task<IEnumerable<Subscription>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsById(Guid id)
        {
            var conditions = new { CreatedBy = id };
            return await _repository.GetAllByConditionsAsync(conditions);
        }

        public async Task<Subscription> Add(AddSubscription request, Guid userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Name", request.Name, DbType.String);
            parameters.Add("@Icon", request.Icon, DbType.String);
            parameters.Add("@Status", SubscriptionStatus.Active, DbType.Boolean);
            parameters.Add("@CreatedBy", userId, DbType.Guid);

            return await _repository.ExecuteSingleStoredProcedure("AddSubscription", parameters);
        }

        public async Task<Subscription> Update(UpdateSubscription request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id, DbType.Guid);
            parameters.Add("@Name", request.Name, DbType.String);
            parameters.Add("@Icon", request.Icon, DbType.String);
            parameters.Add("@Status", request.Status, DbType.Boolean);

            return await _repository.ExecuteSingleStoredProcedure("UpdateSubscription", parameters);
        }

        public async Task<int> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }
    }
}