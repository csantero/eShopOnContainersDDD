﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Catalog.Product
{
    public class Handler :
        IHandleQueries<Queries.List>,
        IHandleQueries<Queries.Catalog>,
        IHandleMessages<Events.Added>,
        IHandleMessages<Events.DescriptionUpdated>,
        IHandleMessages<Events.PictureSet>,
        IHandleMessages<Events.PriceUpdated>,
        IHandleMessages<Events.Removed>
    {
        public async Task Handle(Queries.List query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.ProductIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
        public async Task Handle(Queries.Catalog query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            if (query.BrandId.HasValue)
                builder.Add("CatalogBrandId", query.BrandId.ToString(), Operation.EQUAL);
            if (query.TypeId.HasValue)
                builder.Add("CatalogTypeId", query.TypeId.ToString(), Operation.EQUAL);

            if (!string.IsNullOrEmpty(query.Search))
                builder.Grouped(Group.ANY)
                    .Add("Name", query.Search, Operation.CONTAINS)
                    .Add("Description", query.Search, Operation.CONTAINS);

            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.ProductIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public async Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var brand = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<CategoryBrand.Models.CategoryBrand>(e.CategoryBrandId).ConfigureAwait(false);
            var type = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<CategoryType.Models.CatalogType>(e.CategoryTypeId).ConfigureAwait(false);

            var model = new Models.ProductIndex
            {
                Id = e.ProductId,
                Name = e.Name,
                Price = e.Price,
                CatalogBrand = brand.Brand,
                CatalogBrandId = brand.Id,
                CatalogType = type.Type,
                CatalogTypeId = type.Id
            };

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.ProductId, model).ConfigureAwait(false);
        }
        public async Task Handle(Events.DescriptionUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.ProductIndex>(e.ProductId).ConfigureAwait(false);

            product.Description = e.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.PictureSet e, IMessageHandlerContext ctx)
        {
            var product = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.ProductIndex>(e.ProductId).ConfigureAwait(false);

            product.PictureContents = e.Content;
            product.PictureContentType = e.ContentType;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.PriceUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.ProductIndex>(e.ProductId).ConfigureAwait(false);

            product.Price = e.Price;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.ProductIndex>(e.ProductId);
        }
    }
}
