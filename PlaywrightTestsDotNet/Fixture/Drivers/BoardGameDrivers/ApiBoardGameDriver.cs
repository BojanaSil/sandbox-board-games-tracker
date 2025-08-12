using System.Text;
using Newtonsoft.Json;
using PlaywrightTestsDotNet.Fixture.Utils;

namespace PlaywrightTestsDotNet.Fixture.Drivers.BoardGameDrivers;

public class ApiBoardGameDriver : IBoardGameDriver
{
    private HttpClient _httpClient;
    private HttpRequestMessage _request;
    private List<BoardGameModel> _boardGameModels;
    private string _url;
    private string _createRequestJson;

    public Task InitializeAsync()
    {
        _httpClient = new HttpClient();
        
        return Task.CompletedTask;
    }

    public Task VisitPage()
    {
        // TODO: Configure this in variable group in pipeline
        _url = "http://localhost:5001/api/BoardGame";
        
        return Task.CompletedTask;
    }

    public Task CreateBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers,
        int minimumAge, decimal difficulty, int averagePlayingTime, string categories)
    {
        var request = new BoardGameModel
        {
            Name = name,
            ReleaseDate = releaseDate,
            MinNumberOfPlayers = minNumberOfPlayers,
            MaxNumberOfPlayers = maxNumberOfPlayers,
            MinimumAge = minimumAge,
            Difficulty = difficulty,
            AveragePlayingTimeInMinutes = averagePlayingTime,
            Categories = categories
        };
        _createRequestJson = JsonConvert.SerializeObject(request);

        _request = new HttpRequestMessage
        {
            RequestUri = new Uri(_url),
            Method = HttpMethod.Post,
            Content = new StringContent(_createRequestJson, Encoding.UTF8, "application/json")
        };
        
        return Task.CompletedTask;
    }

    public Task UpdateBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers, int minimumAge,
        decimal difficulty, int averagePlayingTime, string categories)
    {
        var id = _boardGameModels.Find(e => e.Name == name).Id;
        
        var request = new BoardGameModel
        {
            Id = id,
            Name = name,
            ReleaseDate = releaseDate,
            MinNumberOfPlayers = minNumberOfPlayers,
            MaxNumberOfPlayers = maxNumberOfPlayers,
            MinimumAge = minimumAge,
            Difficulty = difficulty,
            AveragePlayingTimeInMinutes = averagePlayingTime,
            Categories = categories
        };
        _createRequestJson = JsonConvert.SerializeObject(request);

        _request = new HttpRequestMessage
        {
            RequestUri = new Uri(_url + $"/{id}"),
            Method = HttpMethod.Put,
            Content = new StringContent(_createRequestJson, Encoding.UTF8, "application/json"),
            
        };
        
        return Task.CompletedTask;
    }

    public async Task SaveBoardGame()
    {
        var a = await _httpClient.SendAsync(_request);
    }

    public async Task RefreshBoardGames()
    {
        var responseMessage = await _httpClient.SendAsync(new HttpRequestMessage
        {
            RequestUri = new Uri(_url),
            Method = HttpMethod.Get,
        });
        
        var response = await responseMessage.Content.ReadAsStringAsync();
        
        _boardGameModels = JsonConvert.DeserializeObject<List<BoardGameModel>>(response);
    }

    public Task AssertBoardGameUpdated(string name, int releaseDate)
    {
        Assert.Contains(_boardGameModels, e => e.Name == name && e.ReleaseDate == releaseDate);
        return Task.CompletedTask;
    }

    public Task AssertBoardGameExists(string text)
    {
        Assert.Contains(_boardGameModels, e => e.Name == text);
        return Task.CompletedTask;
    }

    public Task AssertBoardGameDoesNotExist(string text)
    {
        Assert.DoesNotContain(_boardGameModels, e => e.Name == text);
        return Task.CompletedTask;
    }

    public async Task DeleteBoardGame(string name)
    {
        var id = _boardGameModels.Find(e => e.Name == name).Id;
        
        await _httpClient.SendAsync(new HttpRequestMessage
        {
            RequestUri = new Uri(_url + $"/{id}"),
            Method = HttpMethod.Delete
        });
    }
}