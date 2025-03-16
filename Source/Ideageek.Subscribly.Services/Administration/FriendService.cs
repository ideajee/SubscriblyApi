using Dapper;
using Ideageek.Subscribly.Core.Entities.UserManagement;
using Ideageek.Subscribly.Core.Repositories;
using Ideageek.Subscribly.Services.Dtos.UserManagement;
using Ideageek.Subscribly.Services.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Ideageek.Subscribly.Services.Administration
{
    public interface IFriendService
    {
        Task<Friend> GetById(Guid id);
        Task<IEnumerable<Friend>> GetAll();
        Task<IEnumerable<Friend>> GetAllFriendsById(Guid id);
        Task<Friend> Add(AddFriendDto request, Guid userId);
        Task<Friend> Update(UpdateFriendDto request);
        Task<int> Delete(Guid id);
        Task<IEnumerable<Friend>> ExecuteCustomQuery(string query, object parameters = null);
        //Task<IEnumerable<Friend>> ExecuteStoredProcedure(string procedureName, object parameters = null);
    }
    public class FriendService : IFriendService
    {
        private readonly BaseRepository<Friend> _repository;
        private readonly IConfiguration _configuration;
        public FriendService(IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = new BaseRepository<Friend>(configuration.GetConnectionString("MunshiJeeConnection"));
        }
        public async Task<Friend> GetById(Guid id)
        {
            return await _repository.GetById(id);
        }

        public async Task<IEnumerable<Friend>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Friend>> GetAllFriendsById(Guid id)
        {
            var conditions = new { CreatedBy = id };
            return await _repository.GetByConditionsAsync(conditions);
        }

        public async Task<Friend> Add(AddFriendDto request, Guid userId)
        {
            //var entity = new Friend()
            //{
            //    FriendName = request.FriendName,
            //    PhoneNumber = request.PhoneNumber,
            //    Amount = 0,
            //    CreatedBy = userId
            //};
            //await _repository.Add(entity);
            //return await _repository.GetById(entity.Id);
            var parameters = new DynamicParameters();
            parameters.Add("@FriendName", request.FriendName, DbType.String);
            parameters.Add("@PhoneNumber", request.PhoneNumber, DbType.String);
            parameters.Add("@Amount", 0, DbType.Int64);
            parameters.Add("@CreatedBy", userId, DbType.Guid);

            return await _repository.ExecuteSingleStoredProcedure("AddFriend", parameters);
        }

        public async Task<Friend> Update(UpdateFriendDto request)
        {
            var entity = await _repository.GetById(request.Id);

            entity.FriendName = request.FriendName;
            entity.PhoneNumber = request.PhoneNumber;
            await _repository.Update(entity);
            
            return await _repository.GetById(entity.Id);
        }

        public async Task<int> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<Friend>> ExecuteCustomQuery(string query, object parameters = null)
        {
            return await _repository.ExecuteCustomQuery(query, parameters);
        }

        //public async Task<IEnumerable<Friend>> ExecuteStoredProcedure(string procedureName, object parameters = null)
        //{
        //    return await _repository.ExecuteStoredProcedure(procedureName, parameters);
        //}
    }
}