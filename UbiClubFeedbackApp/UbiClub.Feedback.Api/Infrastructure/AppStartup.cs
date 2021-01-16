using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using FluentValidation;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Api.ModelFactory;
using UbiClub.Feedback.Api.RequestHandlers;
using UbiClub.Feedback.Api.Validators;
using UbiClub.Feedback.Core.Models;
using UbiClub.Feedback.Data;
using UbiClub.Feedback.Data.Interfaces;
using UbiClub.Feedback.Data.Services;
using UbiClub.Feedback.Entities;

[assembly: FunctionsStartup(typeof(UbiClub.Feedback.Api.Infrastructure.AppStartup))]

namespace UbiClub.Feedback.Api.Infrastructure
{
    public class AppStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(AppStartup));
            ConfigureDb(builder.Services);
            ConfigureServices(builder.Services);
            ConfigureValidators(builder.Services);
            ConfigureModelFactories(builder.Services);
            ConfigureRequestHandlers(builder.Services);
        }

        private void ConfigureDb(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
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
        }

        private void ConfigureModelFactories(IServiceCollection services)
        {
            services.AddTransient<IFeedbackCreateModelFactory, FeedbackCreateModelFactory>();
        }

        private void ConfigureRequestHandlers(IServiceCollection services)
        {
            services.AddTransient<IFeedbackCreateRequestHandler, FeedbackCreateRequestHandler>();
        }
    }
}