using System.Text;
using Newtonsoft.Json;
using PlaywrightTestsDotNet.Fixture.Utils;

namespace PlaywrightTestsDotNet.Fixture.Drivers.GameLogDrivers;

public class ApiGameLogDriver : IGameLogDriver
{
    private HttpClient _httpClient;
    private HttpRequestMessage _request;
    private List<GameLogModel> _gameLogModels;
    private string _url;
    private string _createRequestJson;

    public Task InitializeAsync()
    {
        _httpClient = new HttpClient();
        
        return Task.CompletedTask;
    }

    public Task SetupEntryPoint()
    {
        _url = TestConfiguration.GetApiUrl("GameLog");
        
        return Task.CompletedTask;
    }

    public Task CreateGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, 
        int averageDuration, string winner)
    {
        var request = new GameLogModel
        {
            BoardGameName = boardGameName,
            DateOfPlay = dateOfPlay,
            TimesPlayed = timesPlayed,
            NumberOfPlayers = numberOfPlayers,
            AverageDuration = averageDuration,
            Winner = winner
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

    public Task UpdateGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, 
        int averageDuration, string winner)
    {
        var id = _gameLogModels.Find(e => e.BoardGameName == boardGameName && e.DateOfPlay == dateOfPlay).Id;
        
        var request = new GameLogModel
        {
            Id = id,
            BoardGameName = boardGameName,
            DateOfPlay = dateOfPlay,
            TimesPlayed = timesPlayed,
            NumberOfPlayers = numberOfPlayers,
            AverageDuration = averageDuration,
            Winner = winner
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

    public async Task SaveGameLog()
    {
        var a = await _httpClient.SendAsync(_request);
    }

    public async Task RefreshGameLogs()
    {
        var responseMessage = await _httpClient.SendAsync(new HttpRequestMessage
        {
            RequestUri = new Uri(_url),
            Method = HttpMethod.Get,
        });
        
        var response = await responseMessage.Content.ReadAsStringAsync();
        
        _gameLogModels = JsonConvert.DeserializeObject<List<GameLogModel>>(response);
    }

    public Task AssertGameLogUpdated(string boardGameName, int timesPlayed)
    {
        Assert.Contains(_gameLogModels, e => e.BoardGameName == boardGameName && e.TimesPlayed == timesPlayed);
        return Task.CompletedTask;
    }

    public Task AssertGameLogExists(string text)
    {
        Assert.Contains(_gameLogModels, e => e.BoardGameName == text);
        return Task.CompletedTask;
    }

    public Task AssertGameLogDoesNotExist(string text)
    {
        Assert.DoesNotContain(_gameLogModels, e => e.BoardGameName == text);
        return Task.CompletedTask;
    }

    public async Task DeleteGameLog(string boardGameName, DateTime dateOfPlay)
    {
        var id = _gameLogModels.Find(e => e.BoardGameName == boardGameName && e.DateOfPlay == dateOfPlay).Id;

        await _httpClient.SendAsync(new HttpRequestMessage
        {
            RequestUri = new Uri(_url + $"/{id}"),
            Method = HttpMethod.Delete
        });
    }
}