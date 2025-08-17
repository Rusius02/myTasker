using System;

namespace myTasker.Views
{
    public static class TaskStatusValues
    {
        public static Array All => Enum.GetValues(typeof(Domain.TaskStatus));
    }
}
