//#define FORCE_GC

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
        public List<TestResult> ExecuteTestsWithRandomData(int contacts, int repeatTest)
        {
            var dataList = DataSource.GenerateRandomDataList(contacts);
            var tests = GetAllTests();
            var output = new List<TestResult>(tests.Count);

            int baselineBytes = 1;
            foreach (var test in tests)
            {
                if (!test.IsEnabled)
                {
                    continue;
                }

                float timeTotal = 0;
                var result = test.Execute(dataList); // warm up
                //byte[] result = new byte[0];
                for (int i = 0; i < repeatTest; i++)
                {
#if FORCE_GC
                    // force cleanup
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
#endif
                    var watch = Stopwatch.StartNew();

                    result = test.Execute(dataList);

                    watch.Stop();
                    timeTotal = timeTotal + watch.ElapsedMilliseconds;
                }

                var elapsedMs = timeTotal / repeatTest;

                float gain;
                if (test.IsBaseline)
                {
                    baselineBytes = result.Length;
                    gain = 0;
                }
                else
                {
                    gain = CalcGain(baselineBytes, result.Length);
                }

                output.Add(new TestResult(test.TestName, ByteSize.FromBytes(result.Length), gain, elapsedMs));
            }

            output.Sort((a, b) => a.GainPerc.CompareTo(b.GainPerc));

            return output;
        }

        private List<BaseTest> GetAllTests()
        {
            return typeof(BaseTest)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseTest)) && !t.IsAbstract)
                .Select(t => (BaseTest)Activator.CreateInstance(t))
                .OrderByDescending(t => t.IsBaseline)
                .ToList();
        }

        private static float CalcGain(int baselineBytes, int testBytes) => 1 - testBytes / (float)baselineBytes;
    }
}
