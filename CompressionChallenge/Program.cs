using Alba.CsConsoleFormat;
using System.Collections.Generic;
using System.Linq;
using static System.ConsoleColor;
using System;
using Core;

namespace CompressionChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            var contacts = 12000;
            var scheduler = new TestScheduler();
            var result = scheduler.ExecuteTasksWithRandomData(contacts);
            ConsoleRenderer.RenderDocument(CreateGridv2(contacts, true, result));
        }

        private static Document CreateGridv2(int contacts, bool randomData, List<TestResult> resultList)
        {
            var headerThickness = new LineThickness(LineWidth.Double, LineWidth.Single);

            ConsoleColor GetGainColor(double gainPerc) => gainPerc switch
            {
                > 0 => Green,
                0 => Gray,
                < 0 => Red
            };

            string GainToString(double gainPerc) => gainPerc switch
            {
                0 => "-",
                _ => gainPerc.ToString("0.0%")
            };

            return new Document(
                new Span("Contact List Size: ") { Color = Yellow }, contacts, "\n",
                new Span("Random Data? ") { Color = Yellow }, randomData, "\n",
                new Grid
                {
                    Color = Gray,
                    Columns = { GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto },
                    Children = {
                        new Cell("Method") { Stroke = headerThickness },
                        new Cell("Bytes") { Stroke = headerThickness },
                        new Cell("KB") { Stroke = headerThickness },
                        new Cell("Gain %") { Stroke = headerThickness },
                        new Cell("Time (ms)") { Stroke = headerThickness },
                        resultList.Select(item => new[] {
                            new Cell(item.Method) { Color = Yellow },
                            new Cell(item.Size.Bytes),
                            new Cell(item.Size.KiloBytes.ToString("0.0")) { Align = Align.Right },
                            new Cell(GainToString(item.GainPerc)) { Color = GetGainColor(item.GainPerc) },
                            new Cell(item.ExecutionTime.TotalMilliseconds),

                        })
                    }
                });
        }
    }
}
