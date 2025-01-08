﻿using Codxia.Core.Interfaces;
using Codxia.EF.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codxia.Core;
using Codixa.Core.Models.UserModels;

namespace Codxia.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _Context;
        private readonly UserManager<AppUser> _userManager;
        public IUserRepository UsersManger { get; private set; }
        private readonly IWebHostEnvironment _environment;
  
        public UnitOfWork(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _Context = context;
            _environment = environment;
            _userManager = userManager;
            UsersManger = new UserRepository(_userManager);


        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure, params object[] parameters) where T : class
        {
           
            return await _Context.Set<T>().FromSqlRaw(storedProcedure, parameters).ToListAsync();
        }

        public async Task<int> Complete()
        {
            return await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }
}
