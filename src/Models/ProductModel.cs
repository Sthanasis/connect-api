using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace connect.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [BsonElement("Name")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    [JsonPropertyName("category")]
    public string Category { get; set; } = null!;
    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = [];
    [JsonPropertyName("rating")]
    public float Rating { get; set; } = 0;
    [JsonPropertyName("reviewsCount")]
    public int ReviewsCount { get; set; } = 0;
}