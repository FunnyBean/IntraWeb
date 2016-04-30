using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IntraWeb.Filters;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using NSubstitute;
using Xunit;

namespace IntraWeb.UnitTests.Filters
{
    public class CheckArgumentsForNullAttributeTest
    {
        [Fact]
        public void AllArgumentsHaveNullValues()
        {
            // Arrange
            var executingContext = ActionFilterCheckerExtensions.CreateActionExecutingContext((args) =>
            {
                args.Add("RoomId", null);
                args.Add("Room", null);
            });

            var filter = new CheckArgumentsForNullAttribute();

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            Assert.IsType<BadRequestObjectResult>(executingContext.Result);
        }

        [Fact]
        public void OneArgumentsHaveNullValues()
        {
            // Arrange
            var executingContext = ActionFilterCheckerExtensions.CreateActionExecutingContext((args) =>
            {
                args.Add("RoomId", 1);
                args.Add("Room", null);
            });

            var filter = new CheckArgumentsForNullAttribute();

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            Assert.IsType<BadRequestObjectResult>(executingContext.Result);
        }

        [Fact]
        public void AnyArgumentsHaveNotNullValues()
        {
            // Arrange
            var executingContext = ActionFilterCheckerExtensions.CreateActionExecutingContext((args) =>
            {
                args.Add("RoomId", 1);
                args.Add("Room", new object());
            });

            var filter = new CheckArgumentsForNullAttribute();

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            Assert.Null(executingContext.Result);
        }

        [Fact]
        public void NoArguments()
        {
            // Arrange
            var executingContext = ActionFilterCheckerExtensions.CreateActionExecutingContext((args) => { });

            var filter = new CheckArgumentsForNullAttribute();

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            Assert.Null(executingContext.Result);
        }
    }
}
