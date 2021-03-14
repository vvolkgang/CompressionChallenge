using ByteSizeLib;
using Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core
{
    public record TestResult(string Method, ByteSize Size, float GainPerc, float ExecutionTimeInMs);

    public class TestScheduler
    {
        public List<TestResult> ExecuteTasksWithRandomData(int contacts, int repeatTest)
        {
            var dataList = DataSource.GenerateRandomDataList(contacts);
            var tasks = GetAllTasks();
            var output = new List<TestResult>(tasks.Count);

            int baselineBytes = 1;
            foreach (var task in tasks)
            {
                float timeTotal = 0;
                byte[] result = new byte[0];
                for (int i = 0; i < repeatTest; i++)
                {
                    var watch = Stopwatch.StartNew();

                    result = task.Execute(dataList);

                    watch.Stop();
                    timeTotal = timeTotal + watch.ElapsedMilliseconds;
                }

                var elapsedMs = timeTotal / repeatTest;

                float gain;
                if (task.IsBaseline)
                {
                    baselineBytes = result.Length;
                    gain = 0;
                }
                else
                {
                    gain = CalcGain(baselineBytes, result.Length);
                }

                output.Add(new TestResult(task.TestName, ByteSize.FromBytes(result.Length), gain, elapsedMs));
            }

            output.Sort((a, b) => a.GainPerc.CompareTo(b.GainPerc));

            return output;
        }

        private List<BaseTest> GetAllTasks()
        {
            return typeof(BaseTest)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseTest)) && !t.IsAbstract)
                .Select(t => (BaseTest)Activator.CreateInstance(t))
                .OrderByDescending(t => t.IsBaseline)
                .ToList();
        }

        private static float CalcGain(int baselineBytes, int taskBytes) => 1 - taskBytes / (float)baselineBytes;

    }
}
