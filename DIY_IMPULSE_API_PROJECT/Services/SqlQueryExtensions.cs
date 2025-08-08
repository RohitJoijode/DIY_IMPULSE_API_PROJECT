using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Reflection.Emit;

namespace DIY_IMPULSE_API_PROJECT.Services
{
    public static class SqlQueryExtensions
    {
        

        public static IList<T> SqlQuery<T>(this DbContext db, string sql, params object[] parameters) where T : class
        {
           
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));

            // Get the connection and validate it
            var connection = db.Database.GetDbConnection();

            if (connection == null)
                throw new InvalidOperationException("No database connection available");

            // Ensure we have a valid connection string
            if (string.IsNullOrWhiteSpace(connection.ConnectionString))
                throw new InvalidOperationException("Connection string has not been initialized");

            using (var db2 = new ContextForQueryType<T>(connection))
            {
                // share the current database transaction, if one exists
                var transaction = db.Database.CurrentTransaction;
                if (transaction != null)
                    db2.Database.UseTransaction(transaction.GetDbTransaction());
                return db2.Set<T>().FromSqlRaw(sql, parameters).ToList();
            }
        }

        private class ContextForQueryType<T> : DbContext where T : class
        {
            private readonly DbConnection connection;

            public ContextForQueryType(DbConnection connection)
            {
                this.connection = connection;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());

                base.OnConfiguring(optionsBuilder);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>().HasNoKey();
                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
