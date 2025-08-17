using Domain;
using System;

namespace myTasker.Helpers
{
    public static class EnumTaskStatusHelper
    {
        public static Array TaskStatusValues => Enum.GetValues(typeof(TaskStatus));
    }
}
