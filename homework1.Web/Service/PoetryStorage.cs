using System.Linq.Expressions;
using homework1.Web.Models;
using SQLite;

namespace homework1.Web.Service;

public class PoetryStorage : IPoetryStorage {
    public const string DbName = "poetrydb.sqlite3";

    public static readonly string PoetryDbPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData), DbName);

    private SQLiteAsyncConnection _connection;

    private SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    private readonly IPreferenceStorage _preferenceStorage;

    public PoetryStorage(IPreferenceStorage preferenceStorage) {
        _preferenceStorage = preferenceStorage;
    }

    // TestIsInitialized
    public bool IsInitialized =>
        _preferenceStorage.Get(PoetryStorageConstant.DbVersionKey, 0) ==
        PoetryStorageConstant.Version;

    public async Task InitializeAsync() {
        await using var dbFileStream =
            new FileStream(PoetryDbPath, FileMode.OpenOrCreate);
        await using var dbAssetStream =
            typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName);
        await dbAssetStream.CopyToAsync(dbFileStream);

        _preferenceStorage.Set(PoetryStorageConstant.DbVersionKey,
            PoetryStorageConstant.Version);
    }

    public Task<Poetry> GetPoetryAsync(int id) =>
        Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Poetry>> GetPoetriesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take) =>
        await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take)
            .ToListAsync();

    public async Task CloseAsync() => await Connection.CloseAsync();
}

public static class PoetryStorageConstant {
    public const string DbVersionKey =
        nameof(PoetryStorageConstant) + "." + nameof(DbVersionKey);
    // nameof(PoetryStorageConstant) -> "PoetryStorageConstant"
    // "PoetryStorageConstant.DbVersionKey"

    public const int Version = 1;
}