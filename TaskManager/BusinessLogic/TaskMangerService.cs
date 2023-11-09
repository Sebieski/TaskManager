using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BusinessLogic
{
    public class TaskMangerService
    {
        private List<Task> _tasks = new List<Task>();

        public Task Add(string description, DateTime? dueDate)
        {
            var task = new Task(description, dueDate);
            _tasks.Add(task);
            return task;
        }

        public bool Remove(int taskId)
        {
            var taskToRemove = Get(taskId);
            if (taskToRemove != null)
            {
                return _tasks.Remove(taskToRemove);
            }
            return false;
        }

        public Task Get(int taskId)
        {
            return _tasks.Find(t => t.Id == taskId);
        }

        public Task[] GetAll()
        {
            return _tasks.ToArray();
        }

        public Task[] GetAll(TaskStatus status)
        {
            return _tasks.FindAll(t => t.Status == status).ToArray();
        }
        public Task[] GetAll(string description)
        {
            return _tasks.FindAll(t =>
                t.Description.Contains(description, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }

        public bool ChangeStatus(int taskId, TaskStatus status)
        {
            var task = Get(taskId);
            if (task != null && task.Status != status)
            {
                switch (status)
                {
                    case TaskStatus.ToDo:
                        return task.Open();
                    case TaskStatus.InProgress:
                        return task.Start(); ;
                    case TaskStatus.Done:
                        return task.Done();
                    default:
                        return false;
                }
            }
            return false;
            
        }

        public int Count()
        {
            return _tasks.Count;
        }

    }
}
