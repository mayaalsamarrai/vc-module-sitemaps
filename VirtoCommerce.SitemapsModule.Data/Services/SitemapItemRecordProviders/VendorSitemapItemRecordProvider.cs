﻿using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.SitemapsModule.Core.Models;
using VirtoCommerce.SitemapsModule.Core.Services;
using VirtoCommerce.Tools;

namespace VirtoCommerce.SitemapsModule.Data.Services.SitemapItemRecordProviders
{
    public class VendorSitemapItemRecordProvider : SitemapItemRecordProviderBase, ISitemapItemRecordProvider
    {
        public VendorSitemapItemRecordProvider(
            IUrlBuilder urlBuilder,
            ISettingsManager settingsManager,
            IMemberService memberService)
            : base(settingsManager, urlBuilder)
        {
            MemberService = memberService;
        }

        protected IMemberService MemberService { get; private set; }

        public virtual void LoadSitemapItemRecords(Store store, Sitemap sitemap, string baseUrl, Action<ExportImportProgressInfo> progressCallback = null)
        {
            var progressInfo = new ExportImportProgressInfo();

            var sitemapItemRecords = new List<SitemapItemRecord>();
            var vendorOptions = new SitemapItemOptions();

            var vendorSitemapItems = sitemap.Items.Where(x => x.ObjectType.EqualsInvariant(SitemapItemTypes.Vendor));
            var vendorIds = vendorSitemapItems.Select(x => x.ObjectId).ToArray();
            var members = MemberService.GetByIds(vendorIds);
            var i = 0;
            foreach (var sitemapItem in vendorSitemapItems)
            {
                var vendor = members.FirstOrDefault(x => x.Id == sitemapItem.ObjectId) as Vendor;
                if (vendor != null)
                {
                    sitemapItem.ItemsRecords = GetSitemapItemRecords(store, vendorOptions, sitemap.UrlTemplate, baseUrl, vendor);

                    progressInfo.Description = string.Format("Generating sitemap items for vendors: {0}...", i);
                    if (progressCallback != null)
                    {
                        progressCallback(progressInfo);
                    }

                    i += sitemapItem.ItemsRecords.Count;
                }
            }
        }
    }
}