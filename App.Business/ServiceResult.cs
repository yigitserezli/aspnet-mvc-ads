using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public object? Data { get; set; }

        public static ServiceResult Success(object? data = null, int statusCode = StatusCodes.Status200OK)
        {
            return new ServiceResult
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Data = data
            };
        }

        public static ServiceResult<T> Success<T>(T? data = default, int statusCode = StatusCodes.Status200OK)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Data = data
            };
        }

        public static ServiceResult Fail(string? errorMessage = null, int statusCode = StatusCodes.Status400BadRequest)
        {
            return new ServiceResult
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Data = null,
                ErrorMessage = errorMessage
            };
        }

        public static ServiceResult<T> Fail<T>(string? errorMessage = null, int statusCode = StatusCodes.Status400BadRequest)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Data = default,
                ErrorMessage = errorMessage
            };
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public new T? Data
        {
            get
            {
                return (T?)base.Data;
            }
            set
            {
                base.Data = value;
            }
        }
    }
}
