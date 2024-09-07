using Newtonsoft.Json;

namespace STOCKS.Models;

public class NewsApiModel
{ 
    [JsonProperty("status")]
    public string Status { get; set; }
        
    [JsonProperty("totalResults")]
    public int TotalResults { get; set; }
        
    [JsonProperty("articles")]
    public List<ArticleApiModel> Articles { get; set; }
}

public class ArticleApiModel
{
    [JsonProperty("author")]
    public string Author { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("url")]
    public string Url { get; set; }
    
    [JsonProperty("publishedAt")]
    public DateTime PublishDate { get; set; }
    
    [JsonProperty("content")]
    public string Content { get; set; }
    
    [JsonProperty("source")]
    public Source Source { get; set; }
            
}

public class Source
{
    [JsonProperty("name")]
    public string Name { get; set; }
}