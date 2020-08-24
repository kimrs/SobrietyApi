
namespace SobrietyApi.Models
{
    public class SoberDatabaseSettings : ISoberDatabaseSettings
    {
        public string RecordsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISoberDatabaseSettings 
    {
        string RecordsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}