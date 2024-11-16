namespace STOCKS.Models;

public class ArticleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublicationDate { get; set; }
    public string PublicationDateText { get; set; }
    public string Author { get; set; }
    public string Url { get; set; }
    public string SourceName { get; set; }
    public string StockSymbol { get; set; }
}