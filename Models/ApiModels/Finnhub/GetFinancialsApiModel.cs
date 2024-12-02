using Newtonsoft.Json;
using System.Collections.Generic;

public class GetFinancialsApiModel
{
    [JsonProperty("cik")]
    public string Cik { get; set; }

    [JsonProperty("data")]
    public List<DataEntry> Data { get; set; }
}

public class DataEntry
{
    [JsonProperty("accessNumber")]
    public string AccessNumber { get; set; }

    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

    [JsonProperty("cik")]
    public string Cik { get; set; }

    [JsonProperty("year")]
    public int Year { get; set; }

    [JsonProperty("quarter")]
    public int Quarter { get; set; }

    [JsonProperty("form")]
    public string Form { get; set; }

    [JsonProperty("startDate")]
    public string StartDate { get; set; }

    [JsonProperty("endDate")]
    public string EndDate { get; set; }

    [JsonProperty("filedDate")]
    public string FiledDate { get; set; }

    [JsonProperty("acceptedDate")]
    public string AcceptedDate { get; set; }

    [JsonProperty("report")]
    public Report Report { get; set; }
}

public class Report
{
    [JsonProperty("bs")]
    public List<ReportEntry> BalanceSheet { get; set; }

    [JsonProperty("ic")]
    public List<ReportEntry> IncomeStatement { get; set; }

    [JsonProperty("cf")]
    public List<ReportEntry> CashFlow { get; set; }
}

public class ReportEntry
{
    [JsonProperty("concept")]
    public string Concept { get; set; }

    [JsonProperty("unit")]
    public string Unit { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("value")]
    public decimal Value { get; set; }
}