﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Controllers;
using TestProject.Models;
using Xunit;

namespace TestProject.Tests
{
    public class RolesControllerTests
    {
        [Fact]
        public void GetRoles_Should_Use_Only_Admin()
        {
            //Arrage
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "RolesControllerDB_1")
                .Options;

            using var dbContext = new ApplicationContext(options);

            var controller = new RolesController(dbContext);

            //Act
            var actualAttribute = controller.GetType()?.GetMethod("GetRoles")?.GetCustomAttributes(typeof(AuthorizeAttribute), true);

            //Assert
            var single = Assert.Single(actualAttribute);
            var authorizeAttribute = Assert.IsType<AuthorizeAttribute>(single);
            Assert.Equal("Admin", authorizeAttribute.Roles);
        }

        [Fact]
        public async void GetRoles_Should_Get_Roles()
        {
            //Arrage
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "RolesControllerDB_2")
                .Options;

            using var dbContext = new ApplicationContext(options);

            var controller = new RolesController(dbContext);

            //Act
            var response = await controller.GetRoles();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<Role>>>(response);
        }
    }
}
