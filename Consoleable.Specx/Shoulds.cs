using System.Collections.Generic;
using Xunit;

namespace Consoleable.Specx
{
    /// <summary>
    /// An abbreviation of 
    /// </summary>
    public static class Shoulds
    {
        /// <summary>
        /// Chainably assert that <paramref name="is enumerable"/> is not empty.
        /// Install package https://nuget.org/packages/TestBase for an extensive set of <c>Should...()</c>s
        /// </summary>
        public static IEnumerable<T> ShouldNotBeEmpty<T>(this IEnumerable<T> enumerable)
        {
            Assert.NotEmpty(enumerable);
            return enumerable;
        }
    }
}