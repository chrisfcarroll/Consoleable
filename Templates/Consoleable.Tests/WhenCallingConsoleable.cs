using System.Collections.Generic;
using System.Linq;
using Extensions.Logging.ListOfString;
using NUnit.Framework;
using TestBase;

namespace Consoleable.Tests
{
    public class WhenCallingConsoleable
    {
        Consoleable unitUnderTest;
        List<string> log;

        [SetUp]
        public void Setup()
        {
            unitUnderTest = new Consoleable(
                new StringListLogger(log = new List<string>()), 
                new Settings());
        }

        [Test]
        public void ShouldLog()
        {
            //Act
            unitUnderTest.Do();
            
            //Assert
            log
                .ShouldNotBeEmpty()
                .First().ShouldNotBeEmpty();
        }
    }
}