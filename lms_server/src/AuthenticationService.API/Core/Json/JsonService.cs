using Newtonsoft.Json;

namespace AuthenticationService.API.Core.Json;

public interface IJsonService
{
    string Serialize<T>(T obj);
    T Deserialize<T>(string json);
}

public class JsonService : IJsonService
{
    public T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json) ?? throw new InvalidOperationException("Deserialization failed.");
    }

    public string Serialize<T>(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj), "Object to serialize cannot be null.");

        try
        {
            return JsonConvert.SerializeObject(obj);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Serialization failed.", ex);
        }
    }
}
