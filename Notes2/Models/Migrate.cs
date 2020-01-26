using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notes2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes2.Models
{
    public static class Migrate
    {
        public static void DoMigration(IApplicationBuilder app)
        {
            DoMigration(app.ApplicationServices.GetRequiredService<DbNoteContext>());
        }

        public static void DoMigration(DbNoteContext context)
        {
            context.Database.Migrate();
        }
    }
}
