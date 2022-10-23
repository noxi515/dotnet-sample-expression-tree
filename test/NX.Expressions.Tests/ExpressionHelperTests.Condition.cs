using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NX.Expressions
{
    public partial class ExpressionHelperTests
    {
        public class ConditionTests
        {
            [Fact]
            public void And()
            {
                var expression = ExpressionHelper.AndAlso<Data>(x => x.Name == "Hoge", x => x.Age == 10);
                CreateData().Where(expression.Compile()).Should().BeEquivalentTo(new[] { new Data("Hoge", 10) });
            }

            [Fact]
            public void Or()
            {
                var expression = ExpressionHelper.OrElse<Data>(x => x.Name == "Hoge", x => x.Age == 10);
                CreateData().Where(expression.Compile()).Should().BeEquivalentTo(new[]
                {
                    new Data("Hoge", 10), new Data("Piyo", 10), new Data("Hoge", 20)
                });
            }

            private static IEnumerable<Data> CreateData()
            {
                yield return new Data("Hoge", 10);
                yield return new Data("Fuga", 20);
                yield return new Data("Piyo", 10);
                yield return new Data("Hoge", 20);
            }

            private class Data
            {
                public string Name { get; }
                public int Age { get; }

                public Data(string name, int age)
                {
                    Name = name;
                    Age = age;
                }
            }
        }
    }
}
