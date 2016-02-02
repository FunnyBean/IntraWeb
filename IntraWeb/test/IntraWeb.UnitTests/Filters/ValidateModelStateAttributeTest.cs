using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWeb.Filters;
using Microsoft.AspNet.Mvc;
using Xunit;

namespace IntraWeb.UnitTests.Filters
{
    public class ValidateModelStateAttributeTest
    {
        [Fact]
        public void ValidModel()
        {
            // Arrange
            var executingContext = ActionFilterCheckerExtensions.CreateActionExecutingContext(null);
            var filter = new ValidateModelStateAttribute();

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            Assert.Null(executingContext.Result);
        }

        [Fact]
        public void OneErrorInModelState()
        {
            // Arrange
            var executingContext = ActionFilterCheckerExtensions.CreateActionExecutingContext(null);
            executingContext.ModelState.AddModelError("Id", "Required");
            var filter = new ValidateModelStateAttribute();

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            Assert.IsType<BadRequestObjectResult>(executingContext.Result);
        }

        [Fact]
        public void MoreErrorsInModelState()
        {
            // Arrange
            var executingContext = ActionFilterCheckerExtensions.CreateActionExecutingContext(null);
            executingContext.ModelState.AddModelError("Id", "Required");
            executingContext.ModelState.AddModelError("Name", "Max lenght 20");
            var filter = new ValidateModelStateAttribute();

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            Assert.IsType<BadRequestObjectResult>(executingContext.Result);
        }
    }
}
