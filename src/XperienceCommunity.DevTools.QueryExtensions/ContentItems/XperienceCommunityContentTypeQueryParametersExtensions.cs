using System;
using CMS.ContentEngine;
using CMS.ContentEngine.Internal;

namespace XperienceCommunity.QueryExtensions.ContentItems
{
    public static class XperienceCommunityContentTypeQueryParametersExtensions
    {
        /// <summary>
        /// Returns the <see cref="ContentTypeQueryParameters"/> filtered to a single ContentItem with a <see cref="ContentItemInfo.ContentItemGUID"/> matching the provided value
        /// </summary>
        /// <param name="query">The current ContentTypeQueryParameters</param>
        /// <param name="contentItemGuid">Value of the <see cref="ContentItemInfo.ContentItemGUID" /> to filter by</param>
        /// <returns></returns>
        public static ContentTypeQueryParameters WhereContentItemGUIDEquals(this ContentTypeQueryParameters query, Guid contentItemGuid) =>
            query.Where(where => where.WhereEquals(nameof(ContentItemInfo.ContentItemGUID), contentItemGuid));

        /// <summary>
        /// Returns the <see cref="ContentTypeQueryParameters"/> filtered to a single ContentItem with a <see cref="ContentItemInfo.ContentItemID"/> matching the provided value
        /// </summary>
        /// <param name="query">The current ContentTypeQueryParameters</param>
        /// <param name="contentItemId">Value of the <see cref="ContentItemInfo.ContentItemID" /> to filter by</param>
        /// <returns></returns>
        public static ContentTypeQueryParameters WhereContentItemIDEquals(this ContentTypeQueryParameters query, int contentItemId) =>
            query.Where(where => where.WhereEquals(nameof(ContentItemInfo.ContentItemID), contentItemId));

        /// <summary>
        /// Returns the <see cref="ContentTypeQueryParameters"/> filtered to a single ContentItem with a <see cref="ContentItemInfo.ContentItemID"/> matching the provided value
        /// </summary>
        /// <param name="query">The current ContentTypeQueryParameters</param>
        /// <param name="contentItemLanguageMetadata">Value of theContentItemLanguageMetadataID to filter by</param>
        /// <returns></returns>
        public static ContentTypeQueryParameters WhereContentItemLanguageMetadataIDEquals(this ContentTypeQueryParameters query, int contentItemLanguageMetadata) =>
            query.Where(where => where.WhereEquals("ContentItemLanguageMetadataID", contentItemLanguageMetadata));

        /// <summary>
        /// Returns the <see cref="ContentTypeQueryParameters"/> ordered by the WebPageItemOrder (should only be used for WebPageItem types)
        /// </summary>
        /// <param name="query">The current ContentTypeQueryParameters</param>
        /// <returns></returns>
        public static ContentTypeQueryParameters OrderByWebpageItemOrder(this ContentTypeQueryParameters query) =>
            query.OrderBy("WebPageItemOrder");

        /// <summary>
        /// Allows the caller to specify an action that has access to the query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ContentItemQueryBuilder Tap(this ContentItemQueryBuilder query, Action<ContentItemQueryBuilder> action)
        {
            action(query);

            return query;
        }


        /// <summary>
        /// Executes the <paramref name="ifTrueAction" /> if the <paramref name="condition" /> is true, otherwise executes
        /// the <paramref name="elseAction" /> if it is provided.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="ifTrueAction"></param>
        /// <param name="elseAction"></param>
        /// <returns></returns>
        public static ContentItemQueryBuilder If(
            this ContentItemQueryBuilder query, bool condition,
            Action<ContentItemQueryBuilder> ifTrueAction,
            Action<ContentItemQueryBuilder>? elseAction = null)
        {
            if (condition)
            {
                ifTrueAction(query);
            }
            else if (elseAction is not null)
            {
                elseAction(query);
            }

            return query;
        }

        /// <summary>
        /// Executes the <paramref name="ifTrueAction" /> if the <paramref name="condition" /> is true, otherwise executes
        /// the <paramref name="elseAction" /> if it is provided.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="ifTrueAction"></param>
        /// <param name="elseAction"></param>
        /// <returns></returns>
        public static ContentTypeQueryParameters If(
            this ContentTypeQueryParameters query, bool condition,
            Action<ContentTypeQueryParameters> ifTrueAction,
            Action<ContentTypeQueryParameters>? elseAction = null)
        {
            if (condition)
            {
                ifTrueAction(query);
            }
            else if (elseAction is not null)
            {
                elseAction(query);
            }

            return query;
        }

        /// <summary>
        /// Executes the <paramref name="ifTrueAction" /> if the <paramref name="condition" /> is true, otherwise executes
        /// the <paramref name="elseAction" /> if it is provided.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="condition"></param>
        /// <param name="ifTrueAction"></param>
        /// <param name="elseAction"></param>
        /// <returns></returns>
        public static WhereParameters If(
            this WhereParameters where, bool condition,
            Action<WhereParameters> ifTrueAction,
            Action<WhereParameters>? elseAction = null)
        {
            if (condition)
            {
                ifTrueAction(where);
            }
            else if (elseAction is not null)
            {
                elseAction(where);
            }

            return where;
        }

    }
}
