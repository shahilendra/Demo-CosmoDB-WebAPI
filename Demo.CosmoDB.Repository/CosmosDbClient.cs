using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Demo.CosmoDB.Repository
{
    public class CosmosDbClient<T>
    {
        private Container _container;
        private readonly ILogger _logger;
        public CosmosDbClient(CosmosClient dbClient, string databaseName, ILogger logger)
        {
            this._container = dbClient.GetContainer(databaseName, typeof(T).Name);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task AddAsync(T item)
        {
            try
            {
                _logger.LogInformation($"{nameof(CosmosDbClient<T>)}.{nameof(AddAsync)} started!");
                Type myType = item.GetType();
                PropertyInfo propertyInfo = myType.GetProperty("id");
                await this._container.CreateItemAsync<T>(item, new PartitionKey(propertyInfo.GetValue(item).ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CosmosDbClient<T>)}.{nameof(AddAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(CosmosDbClient<T>)}.{nameof(DeleteAsync)} started!");
                await this._container.DeleteItemAsync<T>(id, new PartitionKey(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CosmosDbClient<T>)}.{nameof(DeleteAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public async Task<T> GetAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(CosmosDbClient<T>)}.{nameof(GetAsync)} started!");
                ItemResponse<T> response = await this._container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogError(ex, $"{nameof(CosmosDbClient<T>)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                return default(T);
            }
        }

        public async Task UpdateAsync(string id, T item)
        {
            try
            {
                _logger.LogInformation($"{nameof(CosmosDbClient<T>)}.{nameof(GetAsync)} started!");
                await this._container.UpsertItemAsync<T>(item, new PartitionKey(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CosmosDbClient<T>)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
        public async Task<List<T>> GetAsync()
        {
            List<T> entities = new List<T>();
            try
            {
                _logger.LogInformation($"{nameof(CosmosDbClient<T>)}.{nameof(GetAsync)} started!");
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<T> queryResultSetIterator = this._container.GetItemQueryIterator<T>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (T entity in currentResultSet)
                    {
                        entities.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CosmosDbClient<T>)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
            return entities;
        }
    }
}
