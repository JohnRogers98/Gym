using FluentAssertions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features._Common.States;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Scanners;
using Gym.WebDto.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gym.WebApplication.Tests;

public class RequestHandlersScannerTests
{
    [Fact]
    public void ScanAssembly_Registers_ConcreteHandlers()
    {
        var services = this.ScanTestFixtures();
        var provider = services.BuildServiceProvider();

        var handler = provider.GetService<IRequestHandler<TestRequest, TestResponse>>();

        handler.Should().NotBeNull();
    }

    [Fact]
    public void ScanAssembly_Skips_AbstractHandler()
    {
        var services = this.ScanTestFixtures();

        services.Should().NotContain(dependency => dependency.ImplementationType == typeof(AbstractHandler));
    }

    [Fact]
    public void ScanAssembly_Skips_GenericTypeDefinition()
    {
        var services = this.ScanTestFixtures();
        
        services
            .Should()
            .NotContain(dependency => dependency.ImplementationType != null && dependency.ImplementationType.IsGenericTypeDefinition);
    }

    [Fact]
    public void ScanAssembly_Skips_DecoratorMarker()
    {
        var services = this.ScanTestFixtures();
        services
            .Should()
            .NotContain(dependency => dependency.ImplementationType == typeof(DecoratorMarkerHandler));
    }

    [Fact]
    public void ScanAssembly_Skips_NonHandlerTypes()
    {
        var services = this.ScanTestFixtures();
        services
            .Should()
            .NotContain(dependency => dependency.ImplementationType == typeof(NotAHandler));
    }

    private IServiceCollection ScanTestFixtures()
    {
        var services = new ServiceCollection();
        this.PrepopulateServiceCollection(services);
        RequestHandlersScanner.ScanAssembly(typeof(RequestHandlersScannerTests).Assembly, services);
        return services;
    }
    private void PrepopulateServiceCollection(IServiceCollection services)
    {
        services.AddScoped<IAppSnackbarNotifier, AppSnackbarNotifier>();
    }
}

#region First handler for testing

public record TestRequest;
public record TestResponse;

public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
{
    public Task<AsyncOperation<TestResponse>> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
#endregion

#region Another handler for testing to test if scanner can find multiple handlers
public record AnotherRequest;
public record AnotherResponse;

public class AnotherRequestHandler : IRequestHandler<AnotherRequest, AnotherResponse>
{
    public Task<AsyncOperation<AnotherResponse>> HandleAsync(AnotherRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
#endregion

#region Handlers that should be skipped by scanner
public abstract class AbstractHandler : IRequestHandler<TestRequest, TestResponse>
{
    public Task<AsyncOperation<TestResponse>> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public class GenericHandler<T> : IRequestHandler<TestRequest, TestResponse>
{
    public Task<AsyncOperation<TestResponse>> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public class DecoratorMarkerHandler : IRequestHandler<TestRequest, TestResponse>, IRequestHandlerDecoratorMarker
{
    public Task<AsyncOperation<TestResponse>> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public class NotAHandler;
#endregion
