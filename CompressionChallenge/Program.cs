﻿using Alba.CsConsoleFormat;
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
            var repeatTests = 100;
            var now = DateTime.Now;
            var scheduler = new TestScheduler();
            var result = scheduler.ExecuteTasksWithRandomData(contacts, repeatTests);

            var totalProcessingTime = DateTime.Now - now;
            ConsoleRenderer.RenderDocument(CreateGridv2(contacts, true, repeatTests, totalProcessingTime, result));
        }

        private static Document CreateGridv2(int contacts, bool randomData, int repeatedTests, TimeSpan totalProcessingTime, List<TestResult> resultList)
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
                new Span("Contact List Size: ") { Color = Magenta }, contacts, "\n",
                new Span("Tests repeated ") { Color = Magenta }, repeatedTests, new Span(" Times\n") { Color = Magenta },
                new Span("Total processing time (s): ") { Color = Magenta }, totalProcessingTime.TotalSeconds.ToString("0.0"), "\n",

                new Grid
                {
                    Color = Gray,
                    Columns = { GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto },
                    Children = {
                        new Cell("Test Method") { Stroke = headerThickness, Align = Align.Center },
                        new Cell(" Bytes ") { Stroke = headerThickness, Align = Align.Center },
                        new Cell(" KB ") { Stroke = headerThickness, Align = Align.Center },
                        new Cell(" Gain % ") { Stroke = headerThickness, Align = Align.Center },
                        new Cell(" Time Avg (ms) ") { Stroke = headerThickness, Align = Align.Center },
                        resultList.Select(item => new[] {
                            new Cell(item.Method) { Color = Yellow },
                            new Cell(item.Size.Bytes),
                            new Cell(item.Size.KiloBytes.ToString("0.0")) { Align = Align.Right },
                            new Cell(GainToString(item.GainPerc)) { Color = GetGainColor(item.GainPerc) },
                            new Cell(item.ExecutionTimeInMs),
                        })
                    }
                });
        }
    }
}
