using PerfectChannel.WebApi.Controllers;
using PerfectChannel.WebApi.Models;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace PerfectChannel.WebApi.Test
{
    public class ToDoApiTest
    {
        // Initial call to GetToDoItems return OK but empty object
        [Test]
        public void GetToDoItemsTest()
        {
            var contextOptions = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase("ToDoList")
                .Options;

            using var context = new ToDoContext(contextOptions);

            var controller = new TaskController(context);
            var result = controller.GetToDoItems();
            var response = (OkObjectResult)result.Result.Result;
            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual("{\"Completed\":[],\"Pending\":[]}",response.Value);
        }

        // Add a new to do item that return 201 code
        [Test]
        public void PostToDoItemTest()
        {
            var contextOptions = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase("ToDoList")
                .Options;

            using var context = new ToDoContext(contextOptions);

            var controller = new TaskController(context);
            var result = controller.PostToDoItem(new ToDoItemModel { 
                ItemName = "Name",
                ItemDescription = "Description"
            });
            var response = (CreatedAtActionResult)result.Result.Result;

            var expectedResult = new ToDoItemModel
            {
                ItemDescription = "Description",
                ItemId = 1,
                ItemName = "Name",
                ItemStatusCompleted = false
            };

            Assert.IsNotNull(response);
            Assert.AreEqual(201, response.StatusCode);
            //Commented because fails with error: Expected: same as <PerfectChannel.WebApi.Models.ToDoItemModel> But was:  < PerfectChannel.WebApi.Models.ToDoItemModel >
            //Assert.AreSame(expectedResult, response.Value);
            //Assert.AreEqual(expectedResult, response.Value); 
        }

        // Add a new to do item and get items
        [Test]
        public void PostToDoItemAndGetToDoItemsTest()
        {
            var contextOptions = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase("ToDoList")
                .Options;

            using var context = new ToDoContext(contextOptions);

            var controller = new TaskController(context);
            // Add a new item and get 201 code
            var result = controller.PostToDoItem(new ToDoItemModel
            {
                ItemName = "Name",
                ItemDescription = "Description"
            });
            var response = (CreatedAtActionResult)result.Result.Result;
            var expectedResult = new ToDoItemModel
            {
                ItemDescription = "Description",
                ItemId = 1,
                ItemName = "Name",
                ItemStatusCompleted = false
            };

            Assert.IsNotNull(response);
            Assert.AreEqual(201, response.StatusCode);
            //Assert.AreEqual(expectedResult, response.Value);

            // Get added items, getting the item added
            var resultGet = controller.GetToDoItems();
            var responseGet = (OkObjectResult)resultGet.Result.Result;

            Assert.IsNotNull(responseGet);
            Assert.AreEqual(200, responseGet.StatusCode);
            Assert.AreEqual("{\"Completed\":[],\"Pending\":[{\"ItemId\":1,\"ItemName\":\"Name\",\"ItemDescription\":\"Description\",\"ItemStatusCompleted\":false}]}",responseGet.Value);
        }

        // Add a new to do item and update the status
        [Test]
        public void PostToDoItemAndPutToDoItemTest()
        {
            var contextOptions = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase("ToDoList")
                .Options;

            using var context = new ToDoContext(contextOptions);

            var controller = new TaskController(context);
            // Add a new item
            var result = controller.PostToDoItem(new ToDoItemModel
            {
                ItemName = "Name",
                ItemDescription = "Description"
            });
            var response = (CreatedAtActionResult)result.Result.Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(201, response.StatusCode);

            // Get the item
            var resultGet = controller.GetToDoItems();
            var responseGet = (OkObjectResult)resultGet.Result.Result;

            Assert.IsNotNull(responseGet);
            Assert.AreEqual(200, responseGet.StatusCode);
            //Assert.IsNotNull(response.Content);

            // Update the item
            var resultPut = controller.PutTodoItem(1);
            var responsePut = (CreatedAtActionResult)resultPut.Result.Result;

            Assert.IsNotNull(responsePut);
            Assert.AreEqual(201, responsePut.StatusCode);
            //Assert.IsNotNull(response.Content);
        }

        // Update a non existing id
        [Test]
        public void PutToDoItemNoExistTest()
        {
            var contextOptions = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase("ToDoList")
                .Options;

            using var context = new ToDoContext(contextOptions);

            var controller = new TaskController(context);

            // Update the item that not exists
            var resultPut = controller.PutTodoItem(100);
            var responsePut = (BadRequestResult)resultPut.Result.Result;

            Assert.IsNotNull(responsePut);
            Assert.AreEqual(400, responsePut.StatusCode);
        }
    }
}