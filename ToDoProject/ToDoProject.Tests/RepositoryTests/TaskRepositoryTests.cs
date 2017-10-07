using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoProject.Web.Context;
using ToDoProject.Web.Models;
using ToDoProject.Web.Repository;
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
            var vm = new TaskAddEditViewModel()
            {
                Name="test task",
                Comment = "test comment",
                PriorityId = Priority.High
            };
            var contextMock = new Mock<IProjectContext>();
            contextMock.SetupSet(x => x.Tasks = It.IsAny<DbSet<TaskModel>>()).Verifiable();
            var repository = new TaskRepository(contextMock.Object);

            //act
            await repository.AddEditTask(vm);

            //assert
            contextMock.VerifySet(x => x.Tasks = It.IsAny<DbSet<TaskModel>>());
            Assert.AreEqual(1, await contextMock.Object.Tasks.CountAsync());
            Assert.IsNotNull(await contextMock.Object.Tasks.SingleOrDefaultAsync());
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
            contextMock.Object.Tasks = CreateDbSetMock(sourceList).Object;
            contextMock.Setup(x => x.SaveChangesAsync());
            var repository = new TaskRepository(contextMock.Object);

            //act
            var result = await repository.CompleteTask(12);

            //assert
            Assert.IsTrue(result);
            Assert.IsTrue((await contextMock.Object.Tasks.SingleOrDefaultAsync(x => x.TaskId == task.TaskId)).IsCompleted);
            Assert.IsNotNull((await contextMock.Object.Tasks.SingleOrDefaultAsync(x => x.TaskId == task.TaskId)).CompletedDate);
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Repository_EditTask_Test()
        {
            //arrange
            var vm = new TaskAddEditViewModel
            {
                TaskId = 3,
                Name = "Edited name",
                Comment = "edited comment",
                DueDate = DateTime.UtcNow.AddDays(2),
                PriorityId = Priority.Middle
            };
            var task = new TaskModel
            {
                TaskId = 3,
                Name = "not edited name",
                Comment = "not edited comment",
                PriorityId = Priority.High,
                DueDate = DateTime.UtcNow
            };
            var sourceList = new List<TaskModel>() { task };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Object.Tasks = CreateDbSetMock(sourceList).Object;
            var repository = new TaskRepository(contextMock.Object);

            //act
            var result = await repository.AddEditTask(vm);

            //assert
            Assert.IsTrue(result);
            var editedObj = await contextMock.Object.Tasks.FirstOrDefaultAsync(x => x.TaskId == vm.TaskId);
            Assert.AreEqual(vm.Name, editedObj.Name);
            Assert.AreEqual(vm.Comment, editedObj.Comment);
            Assert.AreEqual(vm.DueDate, editedObj.DueDate);
            Assert.AreEqual(vm.PriorityId, editedObj.PriorityId);
        }

        [TestMethod]
        public async Task Repository_GetAllUserTasks_Test()
        {
            //arrange
            var collection = new List<TaskModel>
            {
                new TaskModel
                {
                    User = new ProjectUser()
                    {
                        Id = "firstuser"
                    },
                    Name = "first"
                },
                new TaskModel
                {
                    User = new ProjectUser()
                    {
                        Id = "firstuser"
                    },
                    Name = "second"
                },
                new TaskModel
                {
                    User = new ProjectUser()
                    {
                        Id = "seconduser"
                    },
                    Name = "third"
                },
            };
            var contextMock = new Mock<IProjectContext>();
            contextMock.Object.Tasks = CreateDbSetMock(collection).Object;
            var repository = new TaskRepository(contextMock.Object);

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
        public async Task Repository_GetTask_Test()
        {
            //arrange
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
            contextMock.Object.Tasks = CreateDbSetMock(collection).Object;
            var repository = new TaskRepository(contextMock.Object);

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
            contextMock.Object.Tasks = CreateDbSetMock(collection).Object;
            var repository = new TaskRepository(contextMock.Object);

            //act
            var firstResult = await repository.DeleteTask(1);
            var thirdResult = await repository.DeleteTask(3);
            var fourthResult = await repository.DeleteTask(4);

            //assert
            Assert.IsTrue(firstResult);
            Assert.IsTrue(thirdResult);
            Assert.IsFalse(fourthResult);
            Assert.IsTrue(contextMock.Object.Tasks.Select(t => t.TaskId).Contains(2));
            Assert.IsFalse(contextMock.Object.Tasks.Select(t => t.TaskId).Contains(1));
            Assert.IsFalse(contextMock.Object.Tasks.Select(t => t.TaskId).Contains(3));
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
