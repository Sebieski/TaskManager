using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BusinessLogic
{
    public class Task
    {
        public int Id { get; set; }
        private static int _idCount;  
        public string Description { get; set; }
        public DateTime CreationDate { get; } = DateTime.Now;
        public DateTime? DueDate { get; set;}
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public TaskStatus Status { get; private set; } = TaskStatus.ToDo;

        public TimeSpan? Duration
        {
            get {
                if (Status == TaskStatus.Done)
                {
                    if (StartDate.HasValue && DoneDate.HasValue) //nullpropagation - sprawdzić
                    {
                        return DoneDate - StartDate;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (StartDate.HasValue)
                {
                    return DateTime.Now - StartDate;
                }
                else { return null; }
            }
        }

        public Task(string description, DateTime? dueDate)
        {
            _idCount++;
            Id = _idCount;
            Description = description;
            DueDate = dueDate;
        }

        public bool Start()
        {
            if (Status == TaskStatus.ToDo)
            {
                Status = TaskStatus.InProgress;
                StartDate = DateTime.Now;
                DoneDate = null;
                return true;
            }
            return false;
        }

        public bool Open()
        {
            if (Status == TaskStatus.Done)
            {
                Status = TaskStatus.ToDo;
                StartDate = null;
                DoneDate = null;
                return true;
            }
            return false;
        }

        public bool Done()
        {
            if (Status == TaskStatus.InProgress)
            {
                Status = TaskStatus.Done;
                DoneDate = DateTime.Now;
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return $"{Id} - {Description} ({Status}), data do kiedy zadanie powinno być wykonane: {(DueDate == null ? "nie podano" : DueDate)}";
        }
    }
}
