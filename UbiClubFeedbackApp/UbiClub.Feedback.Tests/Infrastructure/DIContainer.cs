using System;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UbiClub.Feedback.Api.Infrastructure;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Api.ModelFactory;
using UbiClub.Feedback.Api.RequestHandlers;
using UbiClub.Feedback.Api.Validators;
using UbiClub.Feedback.Core.Models;
using UbiClub.Feedback.Data;
using UbiClub.Feedback.Data.Interfaces;
using UbiClub.Feedback.Data.Services;
using UbiClub.Feedback.Entities;

namespace UbiClub.Feedback.Tests.Infrastructure
{
    internal class DiContainer : IDisposable
    {
        internal DiContainer(string testingDbConnectionString)
        {
            Configure(testingDbConnectionString);
        }
        private ServiceProvider ServiceProvider { get; set; }

        internal IServiceScope CreateScope() => ServiceProvider.CreateScope();

        private void Configure(string testingDbConnectionString)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(AppStartup));
            ConfigureDb(services, testingDbConnectionString);
            ConfigureServices(services);
            ConfigureValidators(services);
            ConfigureModelFactories(services);
            ConfigureRequestHandlers(services);
            ServiceProvider = services.BuildServiceProvider(true);
        }

        private void ConfigureDb(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FeedbackContext>(
                options => options.UseSqlServer(connectionString));
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IGenericRepository, GenericRepository>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IGameSessionService, GameSessionService>();
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<FeedbackCreateModel>, FeedbackCreateModelValidator>();
            services.AddTransient<IValidator<FeedbackGetModel>, FeedbackGetModelValidator>();
        }

        private void ConfigureModelFactories(IServiceCollection services)
        {
            services.AddTransient<IFeedbackCreateModelFactory, FeedbackCreateModelFactory>();
            services.AddTransient<IFeedbackGetModelFactory, FeedbackGetModelFactory>();
        }

        private void ConfigureRequestHandlers(IServiceCollection services)
        {
            services.AddTransient<IFeedbackCreateRequestHandler, FeedbackCreateRequestHandler>();
            services.AddTransient<IFeedbackGetRequestHandler, FeedbackGetRequestHandler>();
        }

        public void Dispose() => ServiceProvider?.Dispose();
    }
}