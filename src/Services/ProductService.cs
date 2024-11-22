using connect.Models;
using connect_utilities.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace connect.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductService(
        IOptions<AppDatabaseSettings> appDatabaseSettings)
    {
        var settings = MongoClientSettings.FromConnectionString(appDatabaseSettings.Value.ConnectionString);
        // Request timeout
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
        var mongoClient = new MongoClient(settings);

        var mongoDatabase = mongoClient.GetDatabase(
            appDatabaseSettings.Value.DatabaseName);

        _productCollection = mongoDatabase.GetCollection<Product>(
            appDatabaseSettings.Value.ProductCollectionName);
    }

    public async Task<List<Product>> GetAllAsync() => await _productCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetAsync(string id) =>
        await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product newProduct) =>
        await _productCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _productCollection.DeleteOneAsync(x => x.Id == id);

    public async Task UpdateProductRating(ReviewMessage review)
    {
        var product = await _productCollection.Find(x => x.Id == review.ProductId).FirstAsync();
        product.ReviewsCount += 1;
        product.TotalScore += review.ReviewScore;
        await _productCollection.ReplaceOneAsync(x => x.Id == product.Id, product);
    }
}