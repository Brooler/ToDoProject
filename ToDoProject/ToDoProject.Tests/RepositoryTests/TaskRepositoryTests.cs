using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoProject.Web.Context;
using ToDoProject.Web.Helpers;
using ToDoProject.Web.Models;
using ToDoProject.Web.Repository;
using ToDoProject.Web.Services;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Tests.RepositoryTests
{
    [TestClass]
    public class TaskRepositoryTests
    {
        [TestMethod]
        public async Task Repository_AddTask_Test()
        {
            //arrange
            MapperHelper.InitializeMapper();
            var vm = new TaskAddEditViewModel()
            {
                Name="test task",
                Comment = "test comment",
                PriorityId = Priority.High
            };
            var user = new ProjectUser
            {
                Id = "qwer"
            };
            var contextMock = new Mock<IProjectContext>();
            var dbsetMock = CreateDbSetMock(new List<TaskModel>());
            contextMock.Setup(x => x.Tasks).Returns(dbsetMock.Object);
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<string>()))
                .Returns(Task.FromResult(user));
            var repository = new TaskRepository(contextMock.Object, userServiceMock.Object);

            //act
            var result = await repository.AddEditTask(vm);

            //assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, contextMock.Object.Tasks.Count(), "Object wasn't added to DbSet");
            Assert.IsNotNull(contextMock.Object.Tasks.SingleOrDefault(), "Object was added to DbSet more than one time");
        }

        [TestMethod]
        public async Task Repository_CompleteTask_Test()
        {
            //arrange
            var task = new TaskModel
            {
                Name = "Task to be completed",
                Comment = "Some comment",
                IsDeleted = false,
                PriorityId = Priority.Lowest,
                TaskId = 12,
                IsCompleted = false
            };
            var sourceList = new List<TaskModel>() { task };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Setup(x => x.Tasks).Returns(CreateDbSetMock(sourceList).Object);
            var userServiceMock = new Mock<IUserService>();
            var repository = new TaskRepository(contextMock.Object, userServiceMock.Object);

            //act
            var result = await repository.CompleteTask(12);

            //assert
            Assert.IsTrue(result);
            Assert.IsTrue(contextMock.Object.Tasks.SingleOrDefault(x => x.TaskId == task.TaskId).IsCompleted);
            Assert.IsNotNull(contextMock.Object.Tasks.SingleOrDefault(x => x.TaskId == task.TaskId).CompletedDate);
        }

        [TestMethod]
        public async Task Repository_EditTask_Test()
        {
            //arrange
            MapperHelper.InitializeMapper();
            var vm = new TaskAddEditViewModel
            {
                TaskId = 3,
                Name = "Edited name",
                Comment = "edited comment",
                DueDate = DateTime.UtcNow.AddDays(2),
                PriorityId = Priority.Medium
            };
            var task = new TaskModel
            {
                TaskId = 3,
                Name = "not edited name",
                Comment = "not edited comment",
                PriorityId = Priority.High,
                DueDate = DateTime.UtcNow
            };
            var user = new ProjectUser
            {
                Id = "qwer"
            };
            var sourceList = new List<TaskModel>() { task };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Setup(x => x.Tasks).Returns(CreateDbSetMock(sourceList).Object);
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<string>()))
                .Returns(Task.FromResult(user));
            var repository = new TaskRepository(contextMock.Object, userServiceMock.Object);

            //act
            var result = await repository.AddEditTask(vm);

            //assert
            Assert.IsTrue(result);
            var editedObj = contextMock.Object.Tasks.FirstOrDefault(x => x.TaskId == vm.TaskId);
            Assert.AreEqual(vm.Name, editedObj.Name);
            Assert.AreEqual(vm.Comment, editedObj.Comment);
            Assert.AreEqual(vm.DueDate, editedObj.DueDate);
            Assert.AreEqual(vm.PriorityId, editedObj.PriorityId);
        }

        [TestMethod]
        public async Task Repository_GetAllUserTasks_Test()
        {
            //arrange
            MapperHelper.InitializeMapper();
            var collection = new List<TaskModel>
            {
                new TaskModel
                {
                    UserId = "firstuser",
                    Name = "first"
                },
                new TaskModel
                {
                    UserId = "firstuser",
                    Name = "second"
                },
                new TaskModel
                {
                    UserId = "seconduser",
                    Name = "third"
                },
            };
            var user = new ProjectUser
            {
                Id = "qwer"
            };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Setup(x => x.Tasks).Returns(CreateDbSetMock(collection).Object);
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<string>()))
                .Returns(Task.FromResult(user));
            var repository = new TaskRepository(contextMock.Object, userServiceMock.Object);

            //act
            var firstuserResults = await repository.GetAllUserTasks("firstuser");
            var seconduserResults = await repository.GetAllUserTasks("seconduser");
            var thirduserResults = await repository.GetAllUserTasks("thirduser");

            //assert
            Assert.AreEqual(2, firstuserResults.Count());
            Assert.AreEqual(1, seconduserResults.Count());
            Assert.IsNotNull(thirduserResults);
            Assert.AreEqual(0, thirduserResults.Count());
        }

        [TestMethod]
        public async Task Repository_GetAllUserTasks_IsOverdueSettedCorrectly()
        {
            //arrange
            MapperHelper.InitializeMapper();
            var collection = new List<TaskModel>
            {
                new TaskModel
                {
                    UserId = "firstuser",
                    Name = "first",
                    DueDate = DateTime.UtcNow.AddDays(-2)
                },
                new TaskModel
                {
                    UserId = "firstuser",
                    Name = "second",
                    DueDate = DateTime.UtcNow.AddDays(-1)
                },
                new TaskModel
                {
                    UserId = "seconduser",
                    Name = "third",
                    DueDate = DateTime.UtcNow.AddDays(2)
                },
            };
            var user = new ProjectUser
            {
                Id = "qwer"
            };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Setup(x => x.Tasks).Returns(CreateDbSetMock(collection).Object);
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<string>()))
                .Returns(Task.FromResult(user));
            var repository = new TaskRepository(contextMock.Object, userServiceMock.Object);

            //act
            var firstuserResults = await repository.GetAllUserTasks("firstuser");
            var seconduserResults = await repository.GetAllUserTasks("seconduser");
            var thirduserResults = await repository.GetAllUserTasks("thirduser");

            //assert
            Assert.IsTrue(firstuserResults.First(x => x.Name == "first").IsOverdue);
            Assert.IsTrue(firstuserResults.First(x => x.Name == "second").IsOverdue);
            Assert.IsFalse(seconduserResults.First(x => x.Name == "third").IsOverdue);
        }

        [TestMethod]
        public async Task Repository_GetAllUserTasks_ContainsNoDelatedTasks()
        {
            //arrange
            MapperHelper.InitializeMapper();
            var collection = new List<TaskModel>
            {
                new TaskModel
                {
                    UserId = "firstuser",
                    Name = "first",
                    IsDeleted = true
                },
                new TaskModel
                {
                    UserId = "firstuser",
                    Name = "second"
                },
                new TaskModel
                {
                    UserId = "seconduser",
                    Name = "third",
                    IsDeleted = true
                },
            };
            var user = new ProjectUser
            {
                Id = "qwer"
            };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Setup(x => x.Tasks).Returns(CreateDbSetMock(collection).Object);
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<string>()))
                .Returns(Task.FromResult(user));
            var repository = new TaskRepository(contextMock.Object, userServiceMock.Object);

            //act
            var firstuserResults = await repository.GetAllUserTasks("firstuser");
            var seconduserResults = await repository.GetAllUserTasks("seconduser");
            var thirduserResults = await repository.GetAllUserTasks("thirduser");

            //assert
            Assert.AreEqual(1, firstuserResults.Count());
            Assert.AreEqual(0, seconduserResults.Count());
            Assert.AreEqual(0, thirduserResults.Count());
        }

        [TestMethod]
        public async Task Repository_GetTask_Test()
        {
            //arrange
            MapperHelper.InitializeMapper();
            var collection = new List<TaskModel>
            {
                new TaskModel
                {
                    TaskId = 1,
                    Name = "First Task"
                },
                new TaskModel
                {
                    TaskId = 2,
                    Name = "Second Task"
                },
                new TaskModel
                {
                    TaskId = 3,
                    Name = "Third Task"
                }
            };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Setup(x => x.Tasks).Returns(CreateDbSetMock(collection).Object);
            var userServiceMock = new Mock<IUserService>();
            var repository = new TaskRepository(contextMock.Object, userServiceMock.Object);

            //act
            var firstResult = await repository.GetTask(1);
            var secondResult = await repository.GetTask(2);
            var thirdResult = await repository.GetTask(3);

            //assert
            Assert.IsNotNull(firstResult);
            Assert.AreEqual("First Task", firstResult.Name);
            Assert.AreEqual(1, firstResult.TaskId);
            Assert.AreEqual("Second Task", secondResult.Name);
            Assert.AreEqual(2, secondResult.TaskId);
            Assert.AreEqual("Third Task", thirdResult.Name);
        }

        [TestMethod]
        public async Task Repository_DeleteTask_Test()
        {
            //arrange
            var collection = new List<TaskModel>
            {
                new TaskModel
                {
                    TaskId = 1,
                    Name = "First"
                },
                new TaskModel
                {
                    TaskId = 2,
                    Name = "Second"
                },
                new TaskModel
                {
                    TaskId = 3,
                    Name = "Third"
                }
            };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Setup(x => x.Tasks).Returns(CreateDbSetMock(collection).Object);
            var userServiceMock = new Mock<IUserService>();
            var repository = new TaskRepository(contextMock.Object, userServiceMock.Object);

            //act
            var firstResult = await repository.DeleteTask(1);
            var thirdResult = await repository.DeleteTask(3);
            var fourthResult = await repository.DeleteTask(4);

            //assert
            Assert.IsTrue(firstResult);
            Assert.IsTrue(thirdResult);
            Assert.IsFalse(fourthResult);
            Assert.IsTrue(contextMock.Object.Tasks.Select(t => t.TaskId).Contains(2));
            Assert.IsTrue(contextMock.Object.Tasks.Select(t => t.TaskId).Contains(1));
            Assert.IsTrue(contextMock.Object.Tasks.Select(t => t.TaskId).Contains(3));
            Assert.IsTrue(contextMock.Object.Tasks.SingleOrDefault(t => t.TaskId == 1).IsDeleted);
            Assert.IsFalse(contextMock.Object.Tasks.SingleOrDefault(t => t.TaskId == 2).IsDeleted);
            Assert.IsTrue(contextMock.Object.Tasks.SingleOrDefault(t => t.TaskId == 3).IsDeleted);
        }

        public static Mock<DbSet<TaskModel>> CreateDbSetMock(List<TaskModel> tasks)
        {
            //var sourceList = new List<TaskModel>() { task };
            var queryable = tasks.AsQueryable();

            var dbSet = new Mock<DbSet<TaskModel>>();
            dbSet.As<IQueryable<TaskModel>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<TaskModel>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<TaskModel>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<TaskModel>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<TaskModel>())).Callback<TaskModel>((s) => tasks.Add(s));

            return dbSet;
        }
    }
}
