using System.Linq.Expressions;
using homework1.Web.Models;

namespace homework1.Web.Service;

public interface IPoetryStorage {
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task<Poetry> GetPoetryAsync(int id);

    Task<IEnumerable<Poetry>> GetPoetriesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take);

    // Connection.Table<Poetry>.Where(p => p.Name.Contains("H")).ToListAsync()
}