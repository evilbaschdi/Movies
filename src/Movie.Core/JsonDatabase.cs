using System.Text.Json;
using System.Text.Json.Serialization;
using Movie.Core.Models;

namespace Movie.Core;

/// <inheritdoc />
public class JsonDatabase : IJsonDatabase
{
    private readonly ISettings _settings;
    private List<MovieRecord> _movieRecords;
    
    private class JsonRoot
    {
        [JsonPropertyName("movies")]
        public List<MovieRecord> Movies { get; set; }
    }

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="settings"></param>
    public JsonDatabase(ISettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        Load();
    }

    private void Load()
    {
        var options = new JsonSerializerOptions
                      {
                          PropertyNameCaseInsensitive = true
                      };
        var json = File.ReadAllText(_settings.FilePath);
        var jsonRoot = JsonSerializer.Deserialize<JsonRoot>(json, options);
        _movieRecords = jsonRoot?.Movies ?? new List<MovieRecord>();
    }

    /// <inheritdoc />
    public void Create(IMovieRecord movieRecord)
    {
        var newMovie = new MovieRecord
                       {
                           Id = Guid.NewGuid().ToString(),
                           Name = movieRecord.Name,
                           Year = movieRecord.Year,
                           Format = movieRecord.Format,
                           Distributed = movieRecord.Distributed,
                           DistributedTo = movieRecord.DistributedTo,
                           Watched = movieRecord.Watched
                       };
        
        _movieRecords.Add(newMovie);
        Save();
    }

    /// <inheritdoc />
    public void Update(IMovieRecord movieRecord)
    {
        var existingRecord = _movieRecords.FirstOrDefault(m => m.Id == movieRecord.Id);
        if (existingRecord != null)
        {
            existingRecord.Name = movieRecord.Name;
            existingRecord.Year = movieRecord.Year;
            existingRecord.Format = movieRecord.Format;
            existingRecord.Distributed = movieRecord.Distributed;
            existingRecord.DistributedTo = movieRecord.DistributedTo;
            existingRecord.Watched = movieRecord.Watched;
            Save();
        }
    }
    
    /// <inheritdoc />
    public void Delete(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var movieToRemove = _movieRecords.FirstOrDefault(m => m.Id == id);
        if (movieToRemove != null)
        {
            _movieRecords.Remove(movieToRemove);
            Save();
        }
    }
    
    /// <inheritdoc />
    public IMovieRecord ValueForId(string id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return _movieRecords.FirstOrDefault(m => m.Id == id);
    }
    
    /// <inheritdoc />
    public IMovieRecord ValueForName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return _movieRecords.FirstOrDefault(m => m.Name == name);
    }

    /// <inheritdoc />
    public List<IMovieRecord> GetValue()
    {
        return _movieRecords.Cast<IMovieRecord>().ToList();
    }
    
    private void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonRoot = new JsonRoot { Movies = _movieRecords };
        var json = JsonSerializer.Serialize(jsonRoot, options);
        File.WriteAllText(_settings.FilePath, json);
    }
}