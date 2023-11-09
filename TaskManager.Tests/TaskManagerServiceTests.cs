using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic;
using Task = TaskManager.BusinessLogic.Task;
using TaskStatus = TaskManager.BusinessLogic.TaskStatus;

namespace TaskManager.Tests
{
    public class TaskManagerServiceTests
    {
        [Fact]
        public void AddTest_Should_AddTask_ToTaskList()
        {
            //Arrange
            var tasks = new TaskMangerService();

            //Act
            tasks.Add("Testowy opis", new DateTime(2023, 12, 31));

            //Assert
            Assert.NotNull(tasks);
            Assert.Equal(1, tasks.Count());
        }

        [Fact]
        public void RemoveTest_Should_RemoveTask_ByTaskId()
        {
            //Arrange
            var tasks = new TaskMangerService();
            tasks.Add("Opis testowy1", null);
            tasks.Add("Opis testowy2", new DateTime(2023,12,31));

            //Act 
            tasks.Remove(2);

            //Assert
            Assert.Equal(1, tasks.Count());
        }

        [Fact]
        public void RemoveTest_Should_NotRemoveTask_WhenTaskIdDoesNotExist()
        {
            //Arrange
            var tasks = new TaskMangerService();
            tasks.Add("Opis testowy1", null);

            //Act 
            bool isRemoved = tasks.Remove(2);

            //Assert
            Assert.False(isRemoved);
        }

        [Fact]
        public void GetTest_Should_GetTask_ByTaskId()
        {
            var tasks = new TaskMangerService();
            var task1 = tasks.Add("Opis testowy1", null);

            //Act 
            var taskToGet = tasks.Get(task1.Id);

            //Assert
            Assert.NotNull(taskToGet);
        }

        [Fact]
        public void GetAllTest_Should_GetAllTasks_WithNoFilter()
        {
            //Arrange
            var tasks = new TaskMangerService();
            tasks.Add("Opis testowy1", null);
            tasks.Add("Opis testowy2", new DateTime(2023, 12, 31));
            tasks.Add("Opis testowy3", DateTime.Now.AddDays(5));

            //Act 
            var taskTable = tasks.GetAll();

            //Assert
            Assert.Equal(3, taskTable.Length);
        }

        [Fact]
        public void GetAllTest_Should_GetTasks_ByStatus()
        {
            //Arrange
            var tasks = new TaskMangerService();
            var task1 = tasks.Add("Opis testowy1", null);
            task1.Start();
            tasks.Add("Opis testowy2", new DateTime(2023, 12, 31));
            tasks.Add("Opis testowy3", DateTime.Now.AddDays(5));

            //Act 
            var taskTable = tasks.GetAll(TaskStatus.ToDo);

            //Assert
            Assert.Equal(2, taskTable.Length);
        }

        [Fact]
        public void GetAllTest_Should_GetTasks_ByDescription()
        {
            //Arrange
            var tasks = new TaskMangerService();
            tasks.Add("Opis testowy1", null);
            tasks.Add("Opis testowy2", new DateTime(2023, 12, 31));
            tasks.Add("Brak słowa na O", DateTime.Now.AddDays(5));

            //Act
            var taskTable = tasks.GetAll("opis");

            //Assert
            Assert.Equal(2, taskTable.Length);
        }

        [Fact]
        public void ChangeStatusTest_Should_ChangeTaskStatus_WhenValid()
        {
            //Arrange
            var tasks = new TaskMangerService();
            var t1 = tasks.Add("Opis testowy1", null);
            var t2 = tasks.Add("Opis testowy1", null);

            //Act
            bool taskChanged = tasks.ChangeStatus(t1.Id, TaskStatus.InProgress);
            bool taskUnchanged = tasks.ChangeStatus(t2.Id, TaskStatus.ToDo);

            //Assert
            Assert.Equal(TaskStatus.InProgress, tasks.Get(t1.Id).Status);
            Assert.True(taskChanged);
            Assert.False(taskUnchanged);
        }

        [Fact]
        public void ChangeStatusTest_Should_NotChangeTaskStatus_WhenInvalidTransition()
        {
            //Arrange
            var tasks = new TaskMangerService();
            tasks.Add("Opis testowy1", null);
            
            //Act
            bool statusChange = tasks.ChangeStatus(1, TaskStatus.Done);
            
            //Assert
            Assert.False(statusChange);
        }

        [Fact]
        public void ChangeStatusTest_Should_NotChangeTaskStatus_WhenTaskIdDoesNotExist()
        {
            //Arrange
            var tasks = new TaskMangerService();
            
            //Act
            bool statusChange = tasks.ChangeStatus(1, TaskStatus.InProgress);

            //Assert
            Assert.False(statusChange);
        }
    }
}
