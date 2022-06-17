namespace AutoCloseWithTimer
{
    using System;

    public class WorkTimeRecord
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public TimeSpan? FromTime { get; set; }

        public TimeSpan? ToTime { get; set; }

        public bool HasWorkingTime =>
            FromTime.HasValue && ToTime.HasValue;

        public static WorkTimeRecord Create(
            long id,
            TimeSpan? from,
            TimeSpan? to) =>
            new WorkTimeRecord()
            {
                Id = id,
                Name = $"Work Time {id}",
                FromTime = from,
                ToTime = to,
            };
    }
}
