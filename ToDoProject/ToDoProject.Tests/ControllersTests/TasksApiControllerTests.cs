using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var resultCorrectId = await controller.GetTaskById(12);
            var resultIncorrectId = await controller.GetTaskById(0);

            //assert
            Assert.IsNotNull(resultCorrectId);
            Assert.IsNotNull(resultIncorrectId);
            Assert.IsInstanceOfType(resultCorrectId, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultIncorrectId, typeof(BadRequestResult));
            Assert.AreEqual(task.Name, ((resultCorrectId as OkObjectResult).Value as TaskAddEditViewModel).Name);
            repositoryMock.Verify(x => x.GetTask(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task Controller_AddEditTest_Test()
        {
            //arrange
            TaskAddEditViewModel correctTask = new TaskAddEditViewModel
            {
                TaskId = 14,
                Name = "testName",
                DueDate = DateTime.Now,
                Comment = "comment",
                PriorityId = Priority.Low
            };
            TaskAddEditViewModel errorTask = new TaskAddEditViewModel
            {
                TaskId = -1,
                Name = "error",
                DueDate = DateTime.Now,
                Comment = "comment",
                PriorityId = Priority.Low
            };
            TaskAddEditViewModel incorrectTask = null;
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(x => x.AddEditTask(correctTask))
                .Returns(Task.FromResult(true));
            repositoryMock.Setup(x => x.AddEditTask(errorTask))
                .Returns(Task.FromResult(false));
            repositoryMock.Setup(x => x.SaveChanges())
                .Returns(Task.FromResult(true));
            var controller = new TasksApiController(repositoryMock.Object);

            //act
            var resultCorrectModel = await controller.AddEditTask(correctTask);
            var resultModelError = await controller.AddEditTask(incorrectTask);
            var resultDataSavingError = await controller.AddEditTask(errorTask);

            //assert
            Assert.IsNotNull(resultModelError);
            Assert.IsNotNull(resultCorrectModel);
            Assert.IsNotNull(resultDataSavingError);
            Assert.IsInstanceOfType(resultCorrectModel, typeof(OkResult));
            Assert.IsInstanceOfType(resultModelError, typeof(BadRequestResult));
            Assert.IsInstanceOfType(resultDataSavingError, typeof(BadRequestResult));
            repositoryMock.Verify(x => x.AddEditTask(It.IsAny<TaskAddEditViewModel>()), Times.Exactly(2));
            repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public async Task Controller_CompleteTask_Test()
        {
            //arrange
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(x => x.CompleteTask(13))
                .Returns(Task.FromResult(true));
            repositoryMock.Setup(x => x.CompleteTask(11))
                .Returns(Task.FromResult(false));
            repositoryMock.Setup(x => x.SaveChanges())
                .Returns(Task.FromResult(true));
            var controller = new TasksApiController(repositoryMock.Object);

            //act
            var resultIncorrect = await controller.CompleteTask(It.IsAny<int>());
            var resultCorrect = await controller.CompleteTask(13);
            var resultDataSavingError = await controller.CompleteTask(11);

            //assert
            Assert.IsNotNull(resultIncorrect);
            Assert.IsNotNull(resultCorrect);
            Assert.IsNotNull(resultDataSavingError);
            Assert.IsInstanceOfType(resultCorrect, typeof(OkResult));
            Assert.IsInstanceOfType(resultDataSavingError, typeof(BadRequestResult));
            Assert.IsInstanceOfType(resultIncorrect, typeof(BadRequestResult));
            repositoryMock.Verify(x => x.CompleteTask(It.IsAny<int>()), Times.Exactly(2));
            repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }
        
        [TestMethod]
        public async Task Controller_DeleteTask_Test()
        {
            //arrange
            var repositoryMock = new Mock<ITaskRepository>();
            repositoryMock.Setup(x => x.DeleteTask(14))
                .Returns(Task.FromResult(true));
            repositoryMock.Setup(x => x.DeleteTask(1))
                .Returns(Task.FromResult(false));
            repositoryMock.Setup(x => x.SaveChanges())
                .Returns(Task.FromResult(true));
            var controller = new TasksApiController(repositoryMock.Object);

            //act
            var resultIncorrect = await controller.DeleteTask(It.IsAny<int>());
            var resultCorrect = await controller.DeleteTask(14);
            var resultDataModifyError = await controller.DeleteTask(1);

            //assert
            Assert.IsNotNull(resultIncorrect);
            Assert.IsNotNull(resultCorrect);
            Assert.IsNotNull(resultDataModifyError);
            Assert.IsInstanceOfType(resultIncorrect, typeof(BadRequestResult));
            Assert.IsInstanceOfType(resultCorrect, typeof(OkResult));
            Assert.IsInstanceOfType(resultDataModifyError, typeof(BadRequestResult));
            repositoryMock.Verify(x => x.DeleteTask(It.IsAny<int>()), Times.Exactly(2));
            repositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
