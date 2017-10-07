using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoProject.Web.Controllers.Api;
using ToDoProject.Web.Models;
using ToDoProject.Web.Repository;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Tests.ControllersTests
{
    [TestClass]
    public class TasksApiControllerTests
    {
        [TestMethod]
        public async Task Controller_GetAllUsersTasks_Test()
        {
            //arrange
            var collection = new List<TaskCollectionViewModel>
            {
                new TaskCollectionViewModel()
                {
                    Name = "A test",
                    DueDate = new DateTime(2017, 5, 21),
                    IsOverdue = true
                },
                new TaskCollectionViewModel()
                {
                    Name = "b test"
                },
                new TaskCollectionViewModel()
                {
                    Name = "c test",
                    IsOverdue = true
                },
            };
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(x => x.GetAllUserTasks(It.IsAny<string>()))
                .Returns(Task.FromResult(collection.AsEnumerable()));
            var controller = new TasksApiController(repositoryMock.Object);

            //act
            var result = await controller.GetUserTasks();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(3, ((result as OkObjectResult).Value as IEnumerable<TaskCollectionViewModel>).Count());
            repositoryMock.Verify(x => x.GetAllUserTasks(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Controller_GetTaskById_Test()
        {
            //arrange
            var task = new TaskAddEditViewModel()
            {
                Name = "Concrete task",
                Comment = "Task for test",
                PriorityId = Priority.High,
                DueDate = DateTime.Now.AddDays(1)
            };

            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(x => x.GetTask(It.IsAny<int>()))
                .Returns(Task.FromResult(task));
            var controller = new TasksApiController(repositoryMock.Object);

            //act
            var result = await controller.GetTaskById(It.IsAny<int>());

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(task.Name, ((result as OkObjectResult).Value as TaskAddEditViewModel).Name);
            repositoryMock.Verify(x => x.GetTask(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task Controller_CreateTask_Test()
        {
            //arrange
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(x => x.AddEditTask(It.IsAny<TaskAddEditViewModel>()))
                .Returns(Task.FromResult(true));
            var controller = new TasksApiController(repositoryMock.Object);

            //act
            var result = await controller.AddEditTask(It.IsAny<TaskAddEditViewModel>());

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            repositoryMock.Verify(x => x.AddEditTask(It.IsAny<TaskAddEditViewModel>()), Times.Once);
            repositoryMock.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task Controller_CompleteTask_Test()
        {
            //arrange
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(x => x.CompleteTask(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            var controller = new TasksApiController(repositoryMock.Object);

            //act
            var result = await controller.CompleteTask(It.IsAny<int>());

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            repositoryMock.Verify(x => x.CompleteTask(It.IsAny<int>()), Times.Once);
            repositoryMock.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
        }
        
        [TestMethod]
        public async Task Controller_DeleteTask_Test()
        {
            //arrange
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(x => x.DeleteTask(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            var controller = new TasksApiController(repositoryMock.Object);

            //act
            var result = await controller.DeleteTask(It.IsAny<int>());

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            repositoryMock.Verify(x => x.DeleteTask(It.IsAny<int>()), Times.Once);
            repositoryMock.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
        }
    }
}
