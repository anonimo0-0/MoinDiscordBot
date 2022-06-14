using System.Text.Json.Serialization;
using System.Text.Json;

namespace MoinDiscordBot;

public class TenorClient
{
    private readonly static HttpClient httpClient = new HttpClient();
    private string tenorToken = Environment.GetEnvironmentVariable("TENOR_TOKEN") ?? throw new MissingFieldException("Tenor token missing");
    private TenorResponse tenorResponse;
    public IList<string> GifUrls
    {
        get
        {
            IList<string> gifUrls = new List<string>(50);

            foreach (var result in tenorResponse.Results)
            {
                gifUrls.Add(result.MediaFormat.Gif.Url);
            }

            return gifUrls;
        }
    }

    public TenorClient()
    {
        var responseTask = httpClient.GetStreamAsync(
            $"https://tenor.googleapis.com/v2/search?q=mowing&key={tenorToken}&limit=50"
        );
        responseTask.Wait();

        var response = responseTask.Result;
        var jsonTask = JsonSerializer.DeserializeAsync<TenorResponse>(response);
        jsonTask.AsTask().Wait();

        var tenorResponse = jsonTask.Result;

        if (tenorResponse is null)
        {
            throw new JsonException("Empty response from tenor");
        }

        this.tenorResponse = tenorResponse;
    }

    class Gif
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
    class MediaFormat
    {
        [JsonPropertyName("gif")]
        public Gif Gif { get; set; }
    }
    class Result
    {
        [JsonPropertyName("media_formats")]
        public MediaFormat MediaFormat { get; set; }
    }
    class TenorResponse
    {
        [JsonPropertyName("results")]
        public IEnumerable<Result> Results { get; set; }
    }
}
