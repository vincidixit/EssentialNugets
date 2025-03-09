using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(Guid id)
            : base($"Task with ID {id} was not found.")
        {
        }
    }

    public class DuplicateTaskTitleException : Exception
    {
        public DuplicateTaskTitleException(string title)
            : base($"A task with the title '{title}' already exists.")
        {
        }
    }
}
