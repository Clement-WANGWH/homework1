namespace homework1.Web.Service; 

public interface IPreferenceStorage {
    void Set(string key, int value);

    int Get(string key, int defaultValue);

    void Set(string key, string value);

    string Get(string key, string defaultValue);
}