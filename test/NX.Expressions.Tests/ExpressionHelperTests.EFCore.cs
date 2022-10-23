using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace NX.Expressions
{
    public partial class ExpressionHelperTests
    {
        public class EFCoreTests : IDisposable
        {
            private SqliteConnection _connection;
            private DatabaseContext _context;

            public EFCoreTests()
            {
                InitializeContext();
            }

            [Fact]
            public async Task And()
            {
                var data = await _context.Data
                    .Where(ExpressionHelper.AndAlso<Data>(x => x.Name == "Hoge", x => x.Age == 10))
                    .ToListAsync();
                data.Should().BeEquivalentTo(new[] { new Data { Id = 1, Name = "Hoge", Age = 10 } });
            }

            [Fact]
            public async Task Or()
            {
                var data = await _context.Data
                    .Where(ExpressionHelper.OrElse<Data>(x => x.Name == "Hoge", x => x.Age == 10))
                    .ToListAsync();
                data.Should().BeEquivalentTo(new[]
                {
                    new Data { Id = 1, Name = "Hoge", Age = 10 }, new Data { Id = 3, Name = "Piyo", Age = 10 },
                    new Data { Id = 4, Name = "Hoge", Age = 20 },
                });
            }


            public void Dispose()
            {
                _context?.Dispose();
                _connection?.Dispose();
            }

            private void InitializeContext()
            {
                _connection = new SqliteConnection("Filename=:memory:");
                _connection.Open();

                var options = new DbContextOptionsBuilder()
                    .UseSqlite(_connection)
                    .Options;
                _context = new DatabaseContext(options);

                if (_context.Database.EnsureCreated())
                {
                    _context.Database.ExecuteSqlRaw(@"
INSERT INTO Data (Id, Name, Age)
VALUES
(1, 'Hoge', 10),
(2, 'Fuga', 20),
(3, 'Piyo', 10),
(4, 'Hoge', 20);
");
                }
            }

            private class DatabaseContext : DbContext
            {
                public virtual DbSet<Data> Data { get; set; }

                public DatabaseContext(DbContextOptions options)
                    : base(options)
                {
                }
            }

            private class Data
            {
                [Key]
                public int Id { get; set; }
                public string Name { get; set; }
                public int Age { get; set; }
            }

        }
    }
}
