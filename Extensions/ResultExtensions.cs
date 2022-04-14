using System.Collections.Generic;
using BookLibWebApi.Models;
using LibAPI.Models.ResponseResult;

namespace LibAPI.Extensions
{
    public static class ResultExtensions
    {
        public static ResponseResult<TableBaseModel<T>> ToTableResponseResult<T>(this (IEnumerable<T>, int) sender)
        {
            var (list, count) = sender;
            return new ResponseResult<TableBaseModel<T>>(true, string.Empty, new TableBaseModel<T>()
            {
                Count = count,
                Items = list
            });
        }
        
        public static TableBaseModel<T> ToTableModel<T>(this (IEnumerable<T>, int) sender)
        {
            var (list, count) = sender;
            return new TableBaseModel<T>
            {
                Count = count,
                Items = list
            };
        }

        public static PaginationModel<IEnumerable<T>> ToPaginationModel<T>(this (IEnumerable<T>, int) model)
        {
            var (list, count) = model;
            return new PaginationModel<IEnumerable<T>>
            {
                Total = count,
                Value = list
            };
        }
    }
}