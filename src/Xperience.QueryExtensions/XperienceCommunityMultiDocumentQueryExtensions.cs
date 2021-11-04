using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using Microsoft.Extensions.Logging;

namespace Xperience.QueryExtensions
{
    public static class XperienceCommunityMultiDocumentQueryExtensions
    {
        /// <summary>
        /// Returns the <see cref="MultiDocumentQuery"/> filtered to a single Node with a <see cref="TreeNode.NodeGUID"/> matching the provided value
        /// </summary>
        /// <param name="query">The current MultiDocumentQuery</param>
        /// <param name="nodeGuid">Value of the <see cref="TreeNode.NodeGUID" /> to filter by</param>
        /// <returns></returns>
        public static MultiDocumentQuery WhereNodeGUIDEquals(this MultiDocumentQuery query, Guid nodeGuid) =>
            query.WhereEquals(nameof(TreeNode.NodeGUID), nodeGuid);

        /// <summary>
        /// Returns the <see cref="MultiDocumentQuery"/> ordered by <see cref="TreeNode.NodeOrder"/>
        /// </summary>
        /// <param name="query">The current MultiDocumentQuery</param>
        /// <returns></returns>
        public static MultiDocumentQuery OrderByNodeOrder(this MultiDocumentQuery query) =>
            query.OrderBy(nameof(TreeNode.NodeOrder));

        /// <summary>
        /// Prints the provided query's full materialized query text using <see cref="Debug.WriteLine(object?)"/>
        /// </summary>
        /// <param name="query">The current MultiDocumentQuery</param>
        /// <param name="queryName">Optional Name for the query that will denote in the output where this specific query starts and ends.
        /// If no value is supplied, the filename containing the calling method will be used. If null or an empty string is supplied, "MultiDocumentQuery" will be used.
        /// </param>
        /// <example>
        /// DocumentHelper
        ///     .GetDocuments()
        ///     .TopN(1)
        ///     .DebugQuery("First Document");
        /// 
        /// 
        /// --- QUERY [First Document] START ---
        /// 
        /// 
        /// DECLARE @DocumentCulture nvarchar(max) = N'en-US';
        ///
        /// SELECT TOP 1 *
        /// FROM View_CMS_Tree_Joined AS V WITH (NOLOCK, NOEXPAND) LEFT OUTER JOIN COM_SKU AS S WITH (NOLOCK) ON [V].[NodeSKUID] = [S].[SKUID]
        /// WHERE [DocumentCulture] = @DocumentCulture
        ///
        /// 
        /// --- QUERY [Dirst Document] END ---
        ///
        /// </example>
        /// <returns></returns>
        public static MultiDocumentQuery DebugQuery(this MultiDocumentQuery query, [CallerFilePath] string queryName = "")
        {
            queryName = string.IsNullOrWhiteSpace(queryName)
                ? nameof(MultiDocumentQuery)
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
        /// <param name="query">The current MultiDocumentQuery</param>
        /// <param name="logger">The logger used to output the query</param>
        /// <param name="queryName">Optional Name for the query that will denote in the output where this specific query starts and ends.
        /// If no value is supplied, the filename containing the calling method will be used. If null or an empty string is supplied, "MultiDocumentQuery" will be used.
        /// </param>
        /// <returns></returns>
        public static MultiDocumentQuery LogQuery(this MultiDocumentQuery query, ILogger logger, [CallerFilePath] string queryName = "")
        {
            queryName = string.IsNullOrWhiteSpace(queryName)
                ? nameof(MultiDocumentQuery)
                : queryName;

            logger.LogDebug("{queryName} {queryText}", queryName, query.GetFullQueryText());

            return query;
        }

        /// <summary>
        /// Converts the <paramref name="query"/> to a <see cref="List{T}"/> of <see cref="TreeNode"/>
        /// </summary>
        /// <param name="query">The current MultiDocumentQuery</param>
        /// <param name="token">Optional cancellation token</param>
        /// <returns></returns>
        public static async Task<IList<TreeNode>> ToListAsync(this MultiDocumentQuery query, CancellationToken token = default)
        {
            var result = await query.GetEnumerableTypedResultAsync(cancellationToken: token);

            return result.ToList();
        }

        /// <summary>
        /// Returns the first item of the <paramref name="query"/> as a <see cref="TreeNode"/> and null if no items were returned.
        /// </summary>
        /// <param name="query">The current MultiDocumentQuery</param>
        /// <param name="token">Optional cancellation token</param>
        /// <returns></returns>
        public static async Task<TreeNode?> FirstOrDefaultAsync(this MultiDocumentQuery query, CancellationToken token = default)
        {
            var result = await query.GetEnumerableTypedResultAsync(cancellationToken: token);

            return result?.FirstOrDefault();
        }
    }
}