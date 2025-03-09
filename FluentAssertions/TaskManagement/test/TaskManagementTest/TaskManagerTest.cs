using FluentAssertions;
using TaskManagement;
using Task = TaskManagement.Task;

namespace TaskManagementTest
{
    public class TaskManagerTest
    {
        private readonly TaskManager _taskManager;

        public TaskManagerTest()
        {
            _taskManager = new TaskManager();
        }

        [Fact]
        public void GetTasksByStatus_Test()
        {
            _taskManager.AddTask("Make PPT", "Presentation for showcasing a product", DateTime.Now.AddDays(1));

            _taskManager.GetTasksByStatus(isCompleted: false)
                                .Should()
                                .HaveCount(1);
        }

        [Fact]
        public void GetOverdueTasks_Test_NoException()
        {
            _taskManager.AddTask("Make PPT", "Presentation for showcasing a product", DateTime.Now.AddDays(1));

            Func<List<Task>> func = () => _taskManager.GetOverdueTasks(DateTime.Now.AddDays(1));

            func.Should().NotThrow<InvalidOperationException>();

            _taskManager.GetOverdueTasks(DateTime.Now.AddDays(1))
                            .Should().HaveCount(1);
        }


        [Fact]
        public void GetOverrideTasks_Test_Exception()
        {
            var action = () => _taskManager.GetOverdueTasks(DateTime.Now);

            action.Should()
                    .Throw<InvalidOperationException>()
                    .WithMessage("No overdue tasks found.");
        }

        [Fact]
        public void MarkTaskAsComplete_Test()
        {
            Task originalTask = _taskManager.AddTask("Update System", "Install Scheduled Update", DateTime.Now.AddDays(1));

            _taskManager.GetTaskById(originalTask.Id).Should().NotBeSameAs(originalTask);
            _taskManager.GetTaskById(originalTask.Id).Should().BeEquivalentTo(originalTask);

            _taskManager.MarkTaskAsComplete(originalTask.Id);

            _taskManager.GetTaskById(originalTask.Id).IsCompleted.Should().BeTrue();
            _taskManager.GetTaskById(originalTask.Id).Should().NotBeEquivalentTo(originalTask);
        }




    }
}