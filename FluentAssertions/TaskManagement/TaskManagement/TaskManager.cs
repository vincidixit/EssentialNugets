using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement
{
    public class TaskManager
    {
        private readonly List<Task> _tasks = new List<Task>();
        private readonly List<Task> _archivedTasks = new List<Task>();

        public Task AddTask(string title, string description, DateTime? dueDate = null)
        {
            if (_tasks.Exists(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateTaskTitleException(title);
            }

            var task = new Task
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                DueDate = dueDate,
                IsCompleted = false
            };
            _tasks.Add(task);

            return (Task) task.Clone();
        }

        public Task GetTaskById(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                throw new TaskNotFoundException(id);
            }
            return task;
        }

        public bool UpdateTask(Guid id, string title, string description, DateTime? dueDate, bool isCompleted)
        {
            var task = GetTaskById(id);

            if (_tasks.Exists(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && t.Id != id))
            {
                throw new DuplicateTaskTitleException(title);
            }

            task.Title = title;
            task.Description = description;
            task.DueDate = dueDate;
            task.IsCompleted = isCompleted;
            return true;
        }

        public bool DeleteTask(Guid id)
        {
            var task = GetTaskById(id);
            if (task == null)
            {
                throw new TaskNotFoundException(id);
            }
            return _tasks.Remove(task);
        }

        public bool MarkTaskAsComplete(Guid id)
        {
            var task = GetTaskById(id);
            if (task.IsCompleted)
            {
                throw new InvalidOperationException($"Task with ID {id} is already completed.");
            }

            task.IsCompleted = true;
            return true;
        }

        public List<Task> SearchTasks(string keyword)
        {
            return _tasks.Where(t =>
                t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Task> GetTasksByStatus(bool isCompleted)
        {
            return _tasks.Where(t => t.IsCompleted == isCompleted).ToList();
        }

        public List<Task> GetOverdueTasks(DateTime currentDate)
        {
            var overdueTasks = _tasks.Where(t => t.DueDate.HasValue && t.DueDate < currentDate && !t.IsCompleted).ToList();

            if (!overdueTasks.Any())
            {
                throw new InvalidOperationException("No overdue tasks found.");
            }

            return overdueTasks;
        }

        public void BulkMarkAsCompleted(List<Guid> taskIds)
        {
            foreach (var id in taskIds)
            {
                var task = GetTaskById(id);
                if (task != null)
                {
                    task.IsCompleted = true;
                }
            }
        }

        public List<Task> SortTasksByTitle()
        {
            return _tasks.OrderBy(t => t.Title).ToList();
        }

        public List<Task> SortTasksByDueDate()
        {
            return _tasks.OrderBy(t => t.DueDate).ToList();
        }

        public List<Task> GetPaginatedTasks(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Page number and page size must be greater than zero.");
            }

            var paginatedTasks = _tasks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            if (!paginatedTasks.Any())
            {
                throw new InvalidOperationException("No tasks found for the specified page.");
            }

            return paginatedTasks;
        }

        public bool ArchiveCompletedTasks()
        {
            var completedTasks = _tasks.Where(t => t.IsCompleted).ToList();
            if (!completedTasks.Any()) return false;

            _archivedTasks.AddRange(completedTasks);
            _tasks.RemoveAll(t => t.IsCompleted);
            return true;
        }
    }

}
