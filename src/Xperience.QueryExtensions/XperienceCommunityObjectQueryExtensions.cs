using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CMS.DataEngine
{
    public static class XperienceCommunityObjectQueryExtensions
    {
        /// <summary>
        /// Converts the <paramref name="query"/> to a <see cref="List{TObject}"/> of the generic Object type
        /// </summary>
        /// <param name="query">The current ObjectQuery</param>
        /// <param name="token">Optional cancellation token</param>
        /// <returns></returns>
        public static async Task<IList<TObject>> ToListAsync<TObject>(this ObjectQuery<TObject> query, CancellationToken token = default)
            where TObject : BaseInfo
        {
            var result = await query.GetEnumerableTypedResultAsync(cancellationToken: token);

            return result.ToList();
        }

        /// <summary>
        /// Converts the <paramref name="query"/> to a <see cref="List{BaseInfo}"/> of <see cref="BaseInfo" />
        /// </summary>
        /// <param name="query">The current ObjectQuery</param>
        /// <param name="token">Optional cancellation token</param>
        /// <returns></returns>
        public static async Task<IList<BaseInfo>> ToListAsync(this ObjectQuery query, CancellationToken token = default)
        {
            var result = await query.GetEnumerableTypedResultAsync(cancellationToken: token);

            return result.ToList();
        }

        /// <summary>
        /// Returns the first item of the <paramref name="query"/> as the generic Object type and null if no items were returned.
        /// /// </summary>
        /// <param name="query">The current ObjectQuery</param>
        /// <param name="token">Optional cancellation token</param>
        /// <returns></returns>
        public static async Task<TObject?> FirstOrDefaultAsync<TObject>(this ObjectQuery<TObject> query, CancellationToken token = default)
            where TObject : BaseInfo
        {
            var result = await query.GetEnumerableTypedResultAsync(cancellationToken: token);

            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Returns the first item of the <paramref name="query"/> as a <see cref="BaseInfo" /> and null if no items were returned.
        /// </summary>
        /// <param name="query">The current ObjectQuery</param>
        /// <param name="token">Optional cancellation token</param>
        /// <returns></returns>
        public static async Task<BaseInfo?> FirstOrDefaultAsync(this ObjectQuery query, CancellationToken token = default)
        {
            var result = await query.GetEnumerableTypedResultAsync(cancellationToken: token);

            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Prints the provided query's full materialized query text using <see cref="Debug.WriteLine(object?)"/>
        /// </summary>
        /// <param name="query">The current ObjectQuery</param>
        /// <param name="queryName">Optional Name for the query that will denote in the output where this specific query starts and ends.
        /// If no value is supplied, the filename containing the calling method will be used. If null or an empty string is supplied, name of the generic will be used.
        /// </param>
        /// <example>
        /// UserInfo.Provider.Get()
        ///     .WhereEquals("Email", "admin@localhost.local")
        ///     .Print("User");
        /// 
        /// --- BEGIN [User] QUERY ---
        ///
        /// DECLARE @Email nvarchar(max) = N'admin@localhost.local';
        ///
        /// SELECT *
        /// FROM CMS_User
        /// WHERE [Email] = @Email
        ///
        /// --- END [User] QUERY ---
        /// </example>
        /// <returns></returns>
        public static ObjectQuery<TObject> DebugQuery<TObject>(this ObjectQuery<TObject> query, [CallerFilePath] string queryName = "")
            where TObject : BaseInfo, new()
        {
            queryName = string.IsNullOrWhiteSpace(queryName)
                ? typeof(TObject).Name
                : queryName;

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine($"--- BEGIN [{queryName}] QUERY ---");
            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine(query.GetFullQueryText());
            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine($"--- END [{queryName}] QUERY ---");
            Debug.WriteLine(Environment.NewLine);

            return query;
        }

        /// <summary>
        /// Prints the provided query's full materialized query text using <see cref="Debug.WriteLine(object?)"/>
        /// </summary>
        /// <param name="query">The current ObjectQuery</param>
        /// <param name="queryName">Optional Name for the query that will denote in the output where this specific query starts and ends.
        /// If no value is supplied, the filename containing the calling method will be used. If null or an empty string is supplied, "BaseInfo" will be used.
        /// </param>
        /// <returns></returns>
        public static ObjectQuery DebugQuery(this ObjectQuery query, [CallerFilePath] string queryName = "")
        {
            queryName = string.IsNullOrWhiteSpace(queryName)
                ? nameof(BaseInfo)
                : queryName;

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine($"--- BEGIN [{queryName}] QUERY ---");
            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine(query.GetFullQueryText());
            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine($"--- END [{queryName}] QUERY ---");
            Debug.WriteLine(Environment.NewLine);

            return query;
        }

        /// <summary>
        /// Prints the provided query's full materialized query text using <see cref="LoggerExtensions.LogDebug(ILogger, string, object[])"/>
        /// </summary>
        /// <param name="query">The current ObjectQuery</param>
        /// <param name="logger">The logger used to output the query</param>
        /// <param name="queryName">Optional Name for the query that will denote in the output where this specific query starts and ends.
        /// If no value is supplied, the filename containing the calling method will be used. If null or an empty string is supplied, name of the generic will be used.
        /// </param>
        /// <returns></returns>
        public static ObjectQuery<TObject> LogQuery<TObject>(this ObjectQuery<TObject> query, ILogger logger, [CallerFilePath] string queryName = "")
            where TObject : BaseInfo, new()
        {
            queryName = string.IsNullOrWhiteSpace(queryName)
                ? typeof(TObject).Name
                : queryName;

            logger.LogDebug("Object Query: {queryName} {queryText}", queryName, query.GetFullQueryText());

            return query;
        }

        /// <summary>
        /// Prints the provided query's full materialized query text using <see cref="LoggerExtensions.LogDebug(ILogger, string, object[])"/>
        /// </summary>
        /// <param name="query">The current ObjectQuery</param>
        /// <param name="logger">The logger used to output the query</param>
        /// <param name="queryName">Optional Name for the query that will denote in the output where this specific query starts and ends.
        /// If no value is supplied, the filename containing the calling method will be used. If null or an empty string is supplied, "BaseInfo" will be used.
        /// </param>
        /// <returns></returns>
        public static ObjectQuery LogQuery(this ObjectQuery query, ILogger logger, [CallerFilePath] string queryName = "")
        {
            queryName = string.IsNullOrWhiteSpace(queryName)
                ? nameof(BaseInfo)
                : queryName;

            logger.LogDebug("Object Query: {queryName} {queryText}", queryName, query.GetFullQueryText());

            return query;
        }
    }
}
