﻿using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using System.Security;

namespace CoreModule.Application.Authorization;

public class AuthorizationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : BaseQueryRequest<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> decoratedHandler;
    private readonly ICurrentUserService currentUser;

    public AuthorizationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decoratedHandler, ICurrentUserService currentUser)
    {
        this.decoratedHandler = decoratedHandler;
        this.currentUser = currentUser;
    }

    public Task<TResult> Handle(TQuery query)
    {
        this.Authorize(query);

        return this.decoratedHandler.Handle(query);
    }

    private void Authorize(TQuery query)
    {
        if (!this.currentUser.IsInRole(query.GetPermissionName()))
        {
            throw new SecurityException();
        }
    }
}
