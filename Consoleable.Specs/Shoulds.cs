using System.Collections.Generic;
using NUnit.Framework;

namespace Consoleable.Specs
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
            Assert.IsNotEmpty(enumerable);
            return enumerable;
        }
        
        /// <summary>
        /// Chainably assert that <paramref name="is enumerable"/> is not empty.
        /// Install package https://nuget.org/packages/TestBase for an extensive set of <c>Should...()</c>s
        /// </summary>
        public static IEnumerable<T> ShouldNotBeEmpty<T>(this IEnumerable<T> enumerable, string message, params object[] args)
        {
            Assert.IsNotEmpty(enumerable, message, args);
            return enumerable;
        }
    }
}