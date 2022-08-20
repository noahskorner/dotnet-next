using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Extensions
{
    public static class ResultExtensions
    {
        public static void ShouldHaveValidationErrorsFor<T>(this Result<T> result, string field)
        {
            Assert.That(result.Errors.Any(x => x.Field == field), Is.True);
        }

        public static void ShouldHaveErrorsFor<T>(this Result<T> result, string key)
        {
            Assert.That(result.Errors.Any(x => x.Key == key), Is.True);
        }
    }
}
