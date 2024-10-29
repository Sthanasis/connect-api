using connect.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace connect.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductService(
        IOptions<ProductDatabaseSettings> productDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            productDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            productDatabaseSettings.Value.DatabaseName);

        _productCollection = mongoDatabase.GetCollection<Product>(
            productDatabaseSettings.Value.ProductCollectionName);
    }

    public async Task<List<Product>> GetAsync() =>
        await _productCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetAsync(string id) =>
        await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product newProduct) =>
        await _productCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _productCollection.DeleteOneAsync(x => x.Id == id);
}