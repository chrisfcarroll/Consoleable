using System.Collections.Generic;

namespace TestBase
{
    /// <summary>Fluent assertions taken from https://nuget.org/packages/TestBase</summary>
    public static class Shoulds
    {
        /// <summary>
        /// Chainably assert that <paramref name="is enumerable"/> is not empty.
        /// Install package https://nuget.org/packages/TestBase for extensive set of
        /// fluent <c>Should...()</c>s
        /// </summary>
        public static IEnumerable<T> ShouldNotBeEmpty<T>(this IEnumerable<T> enumerable)
        {
            NUnit.Framework.Assert.IsNotEmpty(enumerable);
            return enumerable;
        }
        
        /// <summary>
        /// Chainably assert that <paramref name="is enumerable"/> is not empty.
        /// Install package https://nuget.org/packages/TestBase for an extensive set of
        /// fluent <c>Should...()</c>s
        /// </summary>
        public static IEnumerable<T> 
            ShouldNotBeEmpty<T>(
                this IEnumerable<T> enumerable, string message, params object[] args)
        {
            NUnit.Framework.Assert.IsNotEmpty(enumerable, message, args);
            return enumerable;
        }
    }
}