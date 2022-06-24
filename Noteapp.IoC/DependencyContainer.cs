using Noteapp.Application.Interfaces;
using Noteapp.Application.Services;
using Noteapp.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noteapp.Data.Context;
using Noteapp.Domain.Repositories;

using DBContext = Noteapp.Data.Context.DBContext;

namespace Noteapp.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IDBContext, DBContext>();
            services.AddScoped(typeof(IRepository<>), typeof(NoteRepository<>));
            services.AddScoped<IUserService, UserService>();

            //Register services
            services.AddScoped<INoteService, NoteService>();
        }
    }
}
