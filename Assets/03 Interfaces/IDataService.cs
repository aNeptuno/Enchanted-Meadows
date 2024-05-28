public interface IDataService
{

    /// <summary>
    /// Return true if it saved data succesfuly
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="RelativePath">Path to save data</param>
    /// <returns></returns>
    bool SaveData<T>(string RelativePath, T Data, bool Encrypted);

    T LoadData<T>(string RelativePath, bool Encrypted);
}
