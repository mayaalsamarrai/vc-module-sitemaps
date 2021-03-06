﻿using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.SitemapsModule.Core.Models;

namespace VirtoCommerce.SitemapsModule.Core.Services
{
    public interface ISitemapItemService
    {
        GenericSearchResult<SitemapItem> Search(SitemapItemSearchCriteria criteria);

        void SaveChanges(SitemapItem[] items);

        void Remove(string[] itemIds);
    }
}