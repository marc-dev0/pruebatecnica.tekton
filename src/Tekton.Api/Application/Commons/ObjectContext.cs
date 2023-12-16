namespace Tekton.Api.Application.Commons;

public class ObjectContext<T> : IObjectContext<T>
{
    private readonly Dictionary<string, T> _modifiedObjects = new Dictionary<string, T>();

    public void AddModifiedObject(string key, T modifiedObject)
    {
        _modifiedObjects[key] = modifiedObject;
    }

    public T GetModifiedObject(string key)
    {
        _modifiedObjects.TryGetValue(key, out var modifiedObject);
        return modifiedObject;
    }
}

public interface IObjectContext<T>
{
    void AddModifiedObject(string key, T modifiedObject);
    T GetModifiedObject(string key);
}