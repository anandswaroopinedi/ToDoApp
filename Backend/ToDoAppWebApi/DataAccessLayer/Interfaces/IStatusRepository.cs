namespace DataAccessLayer.Interfaces
{
    public interface IStatusRepository
    {
        public Task<int> getIdByName(string name);
    }
}
