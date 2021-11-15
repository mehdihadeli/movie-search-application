using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace BuildingBlocks.Domain
{
    public interface ICommand<out T> : IRequest<T> where T : notnull
    {
    }

    public interface ICommand : IRequest
    {
    }

    public interface IQuery<out T> : IRequest<T>
        where T : notnull
    {
    }

    public interface ICreateCommand<out TResponse> : ICommand<TResponse>, ITxRequest
        where TResponse : notnull
    {
    }

    public interface ICreateCommand : ICommand, ITxRequest
    {
    }

    public interface IUpdateCommand : ICommand, ITxRequest
    {
    }

    public interface IUpdateCommand<out TResponse> : ICommand<TResponse>, ITxRequest
        where TResponse : notnull
    {
    }

    public interface IDeleteCommand<TId, out TResponse> : ICommand<TResponse>
        where TId : struct
        where TResponse : notnull
    {
        public TId Id { get; init; }
    }

    public interface IDeleteCommand<TId> : ICommand where TId : struct
    {
        public TId Id { get; init; }
    }

    public interface IPageList
    {
        public List<string> Includes { get; init; }
        public List<FilterModel> Filters { get; init; }
        public List<string> Sorts { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public interface IListQuery<out TResponse> : IQuery<TResponse>, IPageList
        where TResponse : notnull
    {
    }

    public interface IItemQuery<TId, out TResponse> : IQuery<TResponse>
        where TId : struct
        where TResponse : notnull
    {
        public List<string> Includes { get; }
        public TId Id { get; }
    }

    public record FilterModel(string FieldName, string Comparision, string FieldValue);

    public record ListResultModel<T>(List<T> Items, long TotalItems, int Page, int PageSize) where T : notnull
    {
        public static ListResultModel<T> Create(List<T> items, long totalItems = 0, int page = 1, int pageSize = 20)
        {
            return new(items, totalItems, page, pageSize);
        }

        public ListResultModel<U> Map<U>(Func<T, U> map) => ListResultModel<U>.Create(
            this.Items.Select<T, U>(map).ToList(),
            this.TotalItems, this.Page, this.PageSize);

        public static ListResultModel<T> Empty => new(Enumerable.Empty<T>().ToList(), 0, 0, 0);
    }
}