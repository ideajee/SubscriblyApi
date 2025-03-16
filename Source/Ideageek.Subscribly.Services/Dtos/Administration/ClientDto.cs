namespace Ideageek.Subscribly.Services.Dtos.Administration
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreateClientDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
