using Microsoft.VisualBasic;
using Task = TaskManager.BusinessLogic.Task;
using TaskStatus = TaskManager.BusinessLogic.TaskStatus;

namespace TaskManager.Tests
{
    public class TaskTests
    {
        [Fact]
        public void IdTest_Should_CreateTask_WithAutoIncrementedId()
        {
            //Arrange & Act
            var task1 = new Task("Opis testowy1", null);
            var task2 = new Task("Opis testowy2", null);

            //Assert
            Assert.True(task1.Id > 0);
            Assert.Equal(task1.Id+1, task2.Id);
        }

        [Fact]
        public void CreationDateTest_Should_SetCreationDate_WhenCreatingTask()
        {
            //Arrange & Act
            var task = new Task("Opis testowy1", null);

            //Assert
            Assert.Equal(DateTime.Now.Year, task.CreationDate.Year);
        }

        [Fact]
        public void DueDateTest_Should_SetDueDate_WhenProvided()
        {
            //Arrange & Act
            var task = new Task("Opis testowy1", new DateTime(2023,12,31));
            var date = new DateTime(2023, 12, 31);

            //Assert
            Assert.NotNull(task.DueDate);
            Assert.Equal(date, task.DueDate);
        }

        [Fact]
        public void StatusTest_Should_SetStatusToTodo_WhenTaskIsCreated()
        {
            //Arrange & Act
            var task = new Task("Opis testowy1", new DateTime(2023, 12, 31));
            
            //Assert
            Assert.Equal(TaskStatus.ToDo, task.Status);
        }

        [Fact]
        public void StartTest_Should_ChangeStatus_ToInProgress_WhenStartIsCalled()
        {
            //Arrange
            var task = new Task("Opis testowy1", new DateTime(2023, 12, 31));

            //Act
            task.Start();

            //Assert
            Assert.Equal(TaskManager.BusinessLogic.TaskStatus.InProgress, task.Status);
        }

        [Fact]
        public void StartTest_Should_SetStartDate_WhenStartIsCalled()
        {
            //Arrange
            var task = new Task("Opis testowy1", new DateTime(2023, 12, 31));

            //Act
            task.Start();

            //Assert
            Assert.NotNull(task.StartDate);
        }

        [Fact]
        public void StartTest_Should_NotChangeStatus_ToInProgress_IfAlreadyInProgress()
        {
            //Arrange
            var task = new Task("Opis testowy1", new DateTime(2023, 12, 31));
            task.Start();

            //Act
            bool isStarted = task.Start();

            //Assert
            Assert.False(isStarted);
            Assert.Equal(TaskStatus.InProgress,task.Status);
        }

        [Fact]
        public void DoneTest_Should_ChangeStatus_ToDone_WhenDoneIsCalledAndStatusIsInProgress()
        {
            //Arrange
            var task = new Task("Opis testowy", null);
            task.Start();

            //Act
            task.Done();

            //Assert
            Assert.Equal(TaskStatus.Done, task.Status);
            Assert.NotNull(task.DoneDate);
        }

        [Fact]
        public void DoneTest_Should_SetDoneDate_WhenDoneIsCalled()
        {
            //Arrange
            var task = new Task("Opis testowy", null);
            task.Start();

            //Act
            task.Done();
            
            //Assert
            Assert.Equal(DateTime.Now.Year, task.DoneDate.Value.Year);
            Assert.Equal(DateTime.Now.Month, task.DoneDate.Value.Month);
            Assert.Equal(DateTime.Now.Day, task.DoneDate.Value.Day);
        }

        [Fact]
        public void DoneTest_Should_NotChangeStatus_ToDone_IfStatusIsNotInProgress()
        {
            //Arrange
            var task = new Task("Opis testowy", null);

            //Act
            bool isDone = task.Done();

            //Assert
            Assert.False(isDone);
            Assert.Equal(TaskStatus.ToDo, task.Status);
        }

        [Fact]
        public void DurationTest_Should_CalculateDuration_WhenStatusIsInProgress()
        {
            //Arrange
            var task = new Task("Opis testowy", null);

            //Act
            task.Start();
            Thread.Sleep(5000);
            var duration = task.Duration;

            //Assert
            Assert.NotNull(duration);
            Assert.True(duration > TimeSpan.Zero);
        }

        [Fact]
        public void DurationTest_Should_ReturnNullDuration_WhenStatusIsTodo()
        {
            //Arrange
            var task = new Task("Opis testowy", null);

            //Act
            var duration = task.Duration;

            //Assert
            Assert.Null(duration);
        }

        [Fact]
        public void OpenTest_ReturnsFalseWhenTaskStatusToDo()
        {
            //Arrange
            var task = new Task("Opis testowy", null);

            //Act
            bool isOpened = task.Open();

            //Assert
            Assert.False(isOpened);
        }

        [Fact]
        public void OpenTest_ReturnsTrueWhenTaskHadStatusDone()
        {
            //Arrange
            var task = new Task("Opis testowy", null);
            task.Start();
            task.Done();

            //Act
            bool isOpened = task.Open();

            //Assert
            Assert.True(isOpened);
        }
    }
}