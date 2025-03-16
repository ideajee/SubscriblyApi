using Ideageek.Subscribly.Services.Dtos.Administration;
using Ideageek.Subscribly.Core.Entities.Administration;
using Ideageek.Subscribly.Core.Repositories;
using Microsoft.Extensions.Configuration;

namespace Ideageek.Subscribly.Services.Administration
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAll();
        Task<ClientDto> GetById(Guid id);
        Task<Client> Add(CreateClientDto client);
        Task<int> Update(UpdateClientDto client);
        Task<int> Delete(Guid id);
        Task<IEnumerable<Client>> ExecuteCustomQuery(string query, object parameters = null);
    }
    public class ClientService : IClientService
    {
        private readonly BaseRepository<Client> _repository;
        private readonly IConfiguration _configuration;
        public ClientService(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = configuration.GetConnectionString("MunshiJeeConnection");
            _repository = new BaseRepository<Client>(connectionString);
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<ClientDto> GetById(Guid id)
        {
            var result = await _repository.GetById(id);
            if (result == null)
                throw new Exception();
            
            return new ClientDto()
            {
                Id = result.Id,
                Name = result.Name,
                IsActive = result.IsActive
            };
        }

        public async Task<Client> Add(CreateClientDto client)
        {
            var entity = new Client()
            {
                Name = client.Name,
                IsActive = true,
                CreatedBy = Guid.NewGuid()
            };
            await _repository.Add(entity);
            return await _repository.GetById(entity.Id);
        }

        public async Task<int> Update(UpdateClientDto client)
        {
            var entity = await _repository.GetById(client.Id);
            if (entity == null)
                throw new Exception();

            
            entity.Name = client.Name;
            entity.IsActive = client.IsActive;
            return await _repository.Update(entity);
        }

        public async Task<int> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<Client>> ExecuteCustomQuery(string query, object parameters = null)
        {
            return await _repository.ExecuteCustomQuery(query, parameters);
        }
    }
}
