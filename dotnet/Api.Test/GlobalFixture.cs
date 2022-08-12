using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Test
{
    [SetUpFixture]
    public class GlobalFixture
    {
        [OneTimeSetUp]
        public void Setup()
        {

        }

        [OneTimeTearDown]
        public void TearDown()
        {

        }
    }
}
