using Architecture;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seed
{
	public static class SeedInitializer
	{
		public static void Seed(IApplicationBuilder applicationBuilder)
		{
			var dbContext = applicationBuilder.ApplicationServices.GetRequiredService<ApplicationDataContext>();
			dbContext.Database.EnsureCreatedAsync();
			
			dbContext.SaveChanges();
		}
	}
}
