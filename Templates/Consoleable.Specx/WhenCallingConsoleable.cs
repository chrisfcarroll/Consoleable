using System.Collections.Generic;
using System.Linq;
using Extensions.Logging.ListOfString;
using Xunit;

namespace Consoleable.Specx
{
    public class WhenCallingConsoleable
    {
        [Fact]
        public void ShouldLog()
        {
            //Act
            unitUnderTest.Do();
            
            //Assert
            log
                .ShouldNotBeEmpty()
                .First().ShouldNotBeEmpty();
        }

        public WhenCallingConsoleable()
        {
            unitUnderTest = new Consoleable(
                new StringListLogger(log = new List<string>()), 
                new Settings());
        }

        readonly Consoleable unitUnderTest;
        readonly List<string> log;
    }
}