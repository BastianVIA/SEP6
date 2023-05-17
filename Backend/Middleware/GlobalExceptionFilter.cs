﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using Backend.Database.TransactionManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Backend.Middleware;

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public GlobalExceptionFilter(IDatabaseTransactionFactory transactionFactory)
    {
        _transactionFactory = transactionFactory;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        LogManager.GetCurrentClassLogger().Error($"An Exception occurred of type: {context.Exception.GetType()}, with message: {context.Exception.Message}");

        var transaction = _transactionFactory.GetCurrentTransaction();
        if (transaction != null)
        {
            await transaction.RollbackTransactionAsync();
        }
     

        context.ExceptionHandled = true;
        context.Result = exceptionParser(context.Exception);
        await context.Result.ExecuteResultAsync(context);
    }


    private IActionResult exceptionParser(Exception exception)
    {
        var response = new ObjectResult("An error occurred.");

        if (exception is DbUpdateException)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Value = "An error occurred while contacting the database.";
        }
        else if (exception is KeyNotFoundException)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Value = "The requested resource was not found.";
        }
        else if (exception is UnauthorizedAccessException)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else if (exception is ValidationException)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else if (exception is NullReferenceException)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Value = "A null reference exception occurred.";
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        return response;
    }
}