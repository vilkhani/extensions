﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Extensions.Http.Mvc
{
    public sealed class ApiResult : ActionResult
    {
        public ApiResult(HttpStatusCode statusCode, object data, PaginationInfo paginationInfo = null)
            : this(statusCode, data)
        {
            PaginationInfo = paginationInfo;
        }

        public ApiResult(HttpStatusCode statusCode, params ApiErrorEntry[] errors)
        {
            Guard.NotNull(errors, nameof(errors));

            Code = statusCode;
            Errors = errors;
        }

        public ApiResult(HttpStatusCode code, object data)
        {
            Code = code;
            Data = data;
        }

        public ApiResult(HttpStatusCode code, string errorMessage)
        {
            Guard.NotNullOrEmpty(errorMessage, nameof(errorMessage));

            Code = code;
            Data = errorMessage;
        }

        public HttpStatusCode Code { get; }
        public IEnumerable<ApiErrorEntry> Errors { get; }
        public PaginationInfo PaginationInfo { get; }
        public object Data { get; }

        public override void ExecuteResult(ActionContext context)
        {
            SetStatusCode(context);

            CreateResult().ExecuteResult(context);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            SetStatusCode(context);

            return CreateResult().ExecuteResultAsync(context);
        }

        private ObjectResult CreateResult()
        {
            if (IsSuccess(Code))
            {
                return new ObjectResult(Envelop<object>.Success(Code, Data, PaginationInfo));
            }
            else
            {
                return new ObjectResult(Envelop.HandledError(Code, Errors, Data as string));
            }

            static bool IsSuccess(HttpStatusCode statusCode) => ((int)statusCode >= 200) && ((int)statusCode <= 299);
        }

        private void SetStatusCode(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)Code;
        }
    }
}
