using LinqToDB;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using VSW.Core.Crypto;

namespace VSW.Core.Services
{
    public static class SqlRepositoryExtensions
    {
        public static PagingResult<T> ToPaging<T>(this IQueryable<T> query, int pagingIndex, int pagingSize)
        {
            return query.ToPaging(new PagingInfo(pagingSize, pagingIndex));
        }

        public static PagingResult<T> ToPaging<T>(this IQueryable<T> query, PagingInfo page)
        {
            var count = query.Count();
            var lst = query.Skip(page.GetSkip()).Take(page.GetTake()).ToList();
            return new PagingResult<T>(lst, count, page.Index, page.Size);
        }


        public static async Task<PagingResult<T>> ToPagingAsync<T>(this IQueryable<T> query, int pagingIndex, int pagingSize)
        {
            return await query.ToPagingAsync(new PagingInfo(pagingSize, pagingIndex));
        }

        public static async Task<PagingResult<T>> ToPagingAsync<T>(this IQueryable<T> query, PagingInfo page)
        {
            var count = query.Count();
            var lst = await query.Skip(page.GetSkip()).Take(page.GetTake()).ToListAsync();
            return new PagingResult<T>(lst, count, page.Index, page.Size);
        }

        public static SqlCommandExecutor WithSqlText(this SqlRepository repository, string sqlText)
        {
            var rs = new SqlCommandExecutor(sqlText, SqlCommandType.SqlText, repository);
            return rs;
        }

        public static SqlCommandExecutor WithStoredProc(this SqlRepository repository, string procName)
        {
            var rs = new SqlCommandExecutor(procName, SqlCommandType.SqlStoredProc, repository);
            return rs;
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
            {
                return query.Where(predicate);
            }
            return query;
        }

        public static string ToDebugString<T>(this IQueryable<T> query)
        {
            return EncryptionHelper.ComputeMD5(query.ToString());
        }

        public static List<T> ToListWithCache<T>(this IQueryable<T> query)
        {
            var _cache = CoreService.LocalCache;
            string key = ToDebugString(query);
            if (_cache.HasKey(key))
            {
                return _cache.Get<List<T>>(key);
            }
            else
            {
                var lst = query.ToList();
                _cache.Set(key, lst);
                return lst;
            }
        }

        public static T FirstOrDefaultWithCache<T>(this IQueryable<T> query)
        {
            var _cache = CoreService.LocalCache;
            string key = ToDebugString(query);
            if (_cache.HasKey(key))
            {
                return _cache.Get<T>(key);
            }
            else
            {
                var item = query.FirstOrDefault();
                _cache.Set(key, item);
                return item;
            }
        }

        public static async Task<List<T>> ToListWithCacheAsync<T>(this IQueryable<T> query)
        {
            var _cache = CoreService.LocalCache;
            string key = ToDebugString(query);
            if (_cache.HasKey(key))
            {
                return _cache.Get<List<T>>(key);
            }
            else
            {
                var lst = await query.ToListAsync();
                _cache.Set(key, lst);
                return lst;
            }
        }

        public static async Task<T> FirstOrDefaultWithCacheAsync<T>(this IQueryable<T> query)
        {
            var _cache = CoreService.LocalCache;
            string key = ToDebugString(query);
            if (_cache.HasKey(key))
            {
                return _cache.Get<T>(key);
            }
            else
            {
                var item = await query.FirstOrDefaultAsync();
                _cache.Set(key, item);
                return item;
            }
        }

    }
}
