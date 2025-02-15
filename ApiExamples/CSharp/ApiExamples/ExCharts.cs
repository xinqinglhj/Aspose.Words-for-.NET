﻿using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Drawing.Charts;
using NUnit.Framework;

namespace ApiExamples
{
    [TestFixture]
    public class ExCharts : ApiExampleBase
    {
        [Test]
        public void ChartTitle()
        {
            //ExStart
            //ExFor:Charts.Chart
            //ExFor:Charts.Chart.Title
            //ExFor:Charts.ChartTitle
            //ExFor:Charts.ChartTitle.Overlay
            //ExFor:Charts.ChartTitle.Show
            //ExFor:Charts.ChartTitle.Text
            //ExSummary:Shows how to insert a chart and change its title.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Use a document builder to insert a bar chart
            Shape chartShape = builder.InsertChart(ChartType.Bar, 400, 300);

            Assert.AreEqual(ShapeType.NonPrimitive, chartShape.ShapeType);
            Assert.True(chartShape.HasChart);

            // Get the chart object from the containing shape
            Chart chart = chartShape.Chart;
            
            // Set the title text, which appears at the top center of the chart and modify its appearance
            ChartTitle title = chart.Title;
            title.Text = "MyChart";
            title.Overlay = true;
            title.Show = true;

            doc.Save(ArtifactsDir + "Charts.ChartTitle.docx");
            //ExEnd
        }

        [Test]
        public void NumberFormat()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Add chart with default data.
            Shape shape = builder.InsertChart(ChartType.Line, 432, 252);
            Chart chart = shape.Chart;
            chart.Title.Text = "Data Labels With Different Number Format";

            // Delete default generated series.
            chart.Series.Clear();

            // Add new series
            ChartSeries series0 =
                chart.Series.Add("AW Series 0", new[] { "AW0", "AW1", "AW2" }, new[] { 2.5, 1.5, 3.5 });

            // Add DataLabel to the first point of the first series.
            ChartDataLabel chartDataLabel0 = series0.DataLabels.Add(0);
            chartDataLabel0.ShowValue = true;

            // Set currency format code.
            chartDataLabel0.NumberFormat.FormatCode = "\"$\"#,##0.00";

            ChartDataLabel chartDataLabel1 = series0.DataLabels.Add(1);
            chartDataLabel1.ShowValue = true;

            // Set date format code.
            chartDataLabel1.NumberFormat.FormatCode = "d/mm/yyyy";

            ChartDataLabel chartDataLabel2 = series0.DataLabels.Add(2);
            chartDataLabel2.ShowValue = true;

            // Set percentage format code.
            chartDataLabel2.NumberFormat.FormatCode = "0.00%";

            // Or you can set format code to be linked to a source cell,
            // in this case NumberFormat will be reset to general and inherited from a source cell.
            chartDataLabel2.NumberFormat.IsLinkedToSource = true;

            doc.Save(ArtifactsDir + "Charts.NumberFormat.docx");

            Assert.IsTrue(DocumentHelper.CompareDocs(ArtifactsDir + "Charts.NumberFormat.docx", GoldsDir + "DocumentBuilder.NumberFormat Gold.docx"));
        }

        [Test]
        public void DataArraysWrongSize()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Add chart with default data.
            Shape shape = builder.InsertChart(ChartType.Line, 432, 252);
            Chart chart = shape.Chart;

            ChartSeriesCollection seriesColl = chart.Series;
            seriesColl.Clear();

            // Create category names array, second category will be null.
            string[] categories = { "Cat1", null, "Cat3", "Cat4", "Cat5", null };

            // Adding new series with empty (double.NaN) values.
            seriesColl.Add("AW Series 1", categories, new double[] { 1, 2, double.NaN, 4, 5, 6 });
            seriesColl.Add("AW Series 2", categories, new double[] { 2, 3, double.NaN, 5, 6, 7 });

            Assert.That(
                () => seriesColl.Add("AW Series 3", categories, new[] { double.NaN, 4, 5, double.NaN, double.NaN }),
                Throws.TypeOf<ArgumentException>());
            Assert.That(
                () => seriesColl.Add("AW Series 4", categories,
                    new[] { double.NaN, double.NaN, double.NaN, double.NaN, double.NaN }),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void EmptyValuesInChartData()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Add chart with default data.
            Shape shape = builder.InsertChart(ChartType.Line, 432, 252);
            Chart chart = shape.Chart;

            ChartSeriesCollection seriesColl = chart.Series;
            seriesColl.Clear();

            // Create category names array, second category will be null.
            string[] categories = { "Cat1", null, "Cat3", "Cat4", "Cat5", null };

            // Adding new series with empty (double.NaN) values.
            seriesColl.Add("AW Series 1", categories, new[] { 1, 2, double.NaN, 4, 5, 6 });
            seriesColl.Add("AW Series 2", categories, new[] { 2, 3, double.NaN, 5, 6, 7 });
            seriesColl.Add("AW Series 3", categories, new[] { double.NaN, 4, 5, double.NaN, 7, 8 });
            seriesColl.Add("AW Series 4", categories,
                new[] { double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, 9 });

            doc.Save(ArtifactsDir + "Charts.EmptyValuesInChartData.docx");
        }

        [Test]
        public void ChartDefaultValues()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert chart.
            builder.InsertChart(ChartType.Column3D, 432, 252);

            MemoryStream dstStream = new MemoryStream();
            doc.Save(dstStream, SaveFormat.Docx);

            Shape shapeNode = (Shape)doc.GetChild(NodeType.Shape, 0, true);
            Chart chart = shapeNode.Chart;

            // Assert X axis
            Assert.AreEqual(ChartAxisType.Category, chart.AxisX.Type);
            Assert.AreEqual(AxisCategoryType.Automatic, chart.AxisX.CategoryType);
            Assert.AreEqual(AxisCrosses.Automatic, chart.AxisX.Crosses);
            Assert.AreEqual(false, chart.AxisX.ReverseOrder);
            Assert.AreEqual(AxisTickMark.None, chart.AxisX.MajorTickMark);
            Assert.AreEqual(AxisTickMark.None, chart.AxisX.MinorTickMark);
            Assert.AreEqual(AxisTickLabelPosition.NextToAxis, chart.AxisX.TickLabelPosition);
            Assert.AreEqual(1, chart.AxisX.MajorUnit);
            Assert.AreEqual(true, chart.AxisX.MajorUnitIsAuto);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisX.MajorUnitScale);
            Assert.AreEqual(0.5, chart.AxisX.MinorUnit);
            Assert.AreEqual(true, chart.AxisX.MinorUnitIsAuto);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisX.MinorUnitScale);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisX.BaseTimeUnit);
            Assert.AreEqual("General", chart.AxisX.NumberFormat.FormatCode);
            Assert.AreEqual(100, chart.AxisX.TickLabelOffset);
            Assert.AreEqual(AxisBuiltInUnit.None, chart.AxisX.DisplayUnit.Unit);
            Assert.AreEqual(true, chart.AxisX.AxisBetweenCategories);
            Assert.AreEqual(AxisScaleType.Linear, chart.AxisX.Scaling.Type);
            Assert.AreEqual(1, chart.AxisX.TickLabelSpacing);
            Assert.AreEqual(true, chart.AxisX.TickLabelSpacingIsAuto);
            Assert.AreEqual(1, chart.AxisX.TickMarkSpacing);
            Assert.AreEqual(false, chart.AxisX.Hidden);

            // Assert Y axis
            Assert.AreEqual(ChartAxisType.Value, chart.AxisY.Type);
            Assert.AreEqual(AxisCategoryType.Category, chart.AxisY.CategoryType);
            Assert.AreEqual(AxisCrosses.Automatic, chart.AxisY.Crosses);
            Assert.AreEqual(false, chart.AxisY.ReverseOrder);
            Assert.AreEqual(AxisTickMark.None, chart.AxisY.MajorTickMark);
            Assert.AreEqual(AxisTickMark.None, chart.AxisY.MinorTickMark);
            Assert.AreEqual(AxisTickLabelPosition.NextToAxis, chart.AxisY.TickLabelPosition);
            Assert.AreEqual(1, chart.AxisY.MajorUnit);
            Assert.AreEqual(true, chart.AxisY.MajorUnitIsAuto);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisY.MajorUnitScale);
            Assert.AreEqual(0.5, chart.AxisY.MinorUnit);
            Assert.AreEqual(true, chart.AxisY.MinorUnitIsAuto);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisY.MinorUnitScale);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisY.BaseTimeUnit);
            Assert.AreEqual("General", chart.AxisY.NumberFormat.FormatCode);
            Assert.AreEqual(100, chart.AxisY.TickLabelOffset);
            Assert.AreEqual(AxisBuiltInUnit.None, chart.AxisY.DisplayUnit.Unit);
            Assert.AreEqual(true, chart.AxisY.AxisBetweenCategories);
            Assert.AreEqual(AxisScaleType.Linear, chart.AxisY.Scaling.Type);
            Assert.AreEqual(1, chart.AxisY.TickLabelSpacing);
            Assert.AreEqual(true, chart.AxisY.TickLabelSpacingIsAuto);
            Assert.AreEqual(1, chart.AxisY.TickMarkSpacing);
            Assert.AreEqual(false, chart.AxisY.Hidden);

            // Assert Z axis
            Assert.AreEqual(ChartAxisType.Series, chart.AxisZ.Type);
            Assert.AreEqual(AxisCategoryType.Category, chart.AxisZ.CategoryType);
            Assert.AreEqual(AxisCrosses.Automatic, chart.AxisZ.Crosses);
            Assert.AreEqual(false, chart.AxisZ.ReverseOrder);
            Assert.AreEqual(AxisTickMark.None, chart.AxisZ.MajorTickMark);
            Assert.AreEqual(AxisTickMark.None, chart.AxisZ.MinorTickMark);
            Assert.AreEqual(AxisTickLabelPosition.NextToAxis, chart.AxisZ.TickLabelPosition);
            Assert.AreEqual(1, chart.AxisZ.MajorUnit);
            Assert.AreEqual(true, chart.AxisZ.MajorUnitIsAuto);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisZ.MajorUnitScale);
            Assert.AreEqual(0.5, chart.AxisZ.MinorUnit);
            Assert.AreEqual(true, chart.AxisZ.MinorUnitIsAuto);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisZ.MinorUnitScale);
            Assert.AreEqual(AxisTimeUnit.Automatic, chart.AxisZ.BaseTimeUnit);
            Assert.AreEqual(string.Empty, chart.AxisZ.NumberFormat.FormatCode);
            Assert.AreEqual(100, chart.AxisZ.TickLabelOffset);
            Assert.AreEqual(AxisBuiltInUnit.None, chart.AxisZ.DisplayUnit.Unit);
            Assert.AreEqual(true, chart.AxisZ.AxisBetweenCategories);
            Assert.AreEqual(AxisScaleType.Linear, chart.AxisZ.Scaling.Type);
            Assert.AreEqual(1, chart.AxisZ.TickLabelSpacing);
            Assert.AreEqual(true, chart.AxisZ.TickLabelSpacingIsAuto);
            Assert.AreEqual(1, chart.AxisZ.TickMarkSpacing);
            Assert.AreEqual(false, chart.AxisZ.Hidden);
        }

        [Test]
        public void InsertChartUsingAxisProperties()
        {
            //ExStart
            //ExFor:ChartAxis
            //ExFor:ChartAxis.CategoryType
            //ExFor:ChartAxis.Crosses
            //ExFor:ChartAxis.ReverseOrder
            //ExFor:ChartAxis.MajorTickMark
            //ExFor:ChartAxis.MinorTickMark
            //ExFor:ChartAxis.MajorUnit
            //ExFor:ChartAxis.MinorUnit
            //ExFor:ChartAxis.TickLabelOffset
            //ExFor:ChartAxis.TickLabelPosition
            //ExFor:ChartAxis.TickLabelSpacingIsAuto
            //ExFor:ChartAxis.TickMarkSpacing
            //ExFor:Charts.AxisCategoryType
            //ExFor:Charts.AxisCrosses
            //ExFor:Charts.Chart.AxisX
            //ExFor:Charts.Chart.AxisY
            //ExFor:Charts.Chart.AxisZ
            //ExSummary:Shows how to insert chart using the axis options for detailed configuration.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert chart.
            Shape shape = builder.InsertChart(ChartType.Column, 432, 252);
            Chart chart = shape.Chart;

            // Clear demo data.
            chart.Series.Clear();
            chart.Series.Add("Aspose Test Series",
                new string[] { "Word", "PDF", "Excel", "GoogleDocs", "Note" },
                new double[] { 640, 320, 280, 120, 150 });

            // Get chart axes
            ChartAxis xAxis = chart.AxisX;
            ChartAxis yAxis = chart.AxisY;

            // For 2D charts like the one we made, the Z axis is null
            Assert.Null(chart.AxisZ);

            // Set X-axis options
            xAxis.CategoryType = AxisCategoryType.Category;
            xAxis.Crosses = AxisCrosses.Minimum;
            xAxis.ReverseOrder = false;
            xAxis.MajorTickMark = AxisTickMark.Inside;
            xAxis.MinorTickMark = AxisTickMark.Cross;
            xAxis.MajorUnit = 10;
            xAxis.MinorUnit = 15;
            xAxis.TickLabelOffset = 50;
            xAxis.TickLabelPosition = AxisTickLabelPosition.Low;
            xAxis.TickLabelSpacingIsAuto = false;
            xAxis.TickMarkSpacing = 1;

            // Set Y-axis options
            yAxis.CategoryType = AxisCategoryType.Automatic;
            yAxis.Crosses = AxisCrosses.Maximum;
            yAxis.ReverseOrder = true;
            yAxis.MajorTickMark = AxisTickMark.Inside;
            yAxis.MinorTickMark = AxisTickMark.Cross;
            yAxis.MajorUnit = 100;
            yAxis.MinorUnit = 20;
            yAxis.TickLabelPosition = AxisTickLabelPosition.NextToAxis;
            //ExEnd

            doc.Save(ArtifactsDir + "Charts.InsertChartUsingAxisProperties.docx");
            doc.Save(ArtifactsDir + "Charts.InsertChartUsingAxisProperties.pdf");
        }

        [Test]
        public void InsertChartWithDateTimeValues()
        {
            //ExStart
            //ExFor:AxisBound
            //ExFor:AxisBound.#ctor(Double)
            //ExFor:AxisBound.#ctor(DateTime)
            //ExFor:AxisScaling.Minimum
            //ExFor:AxisScaling.Maximum
            //ExFor:ChartAxis.Scaling
            //ExFor:Charts.AxisTickMark
            //ExFor:Charts.AxisTickLabelPosition
            //ExFor:Charts.AxisTimeUnit
            //ExFor:Charts.ChartAxis.BaseTimeUnit
            //ExSummary:Shows how to insert chart with date/time values
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert chart.
            Shape shape = builder.InsertChart(ChartType.Line, 432, 252);
            Chart chart = shape.Chart;
            
            // Clear demo data.
            chart.Series.Clear();

            // Fill data.
            chart.Series.Add("Aspose Test Series",
                new[]
                {
                    new DateTime(2017, 11, 06), new DateTime(2017, 11, 09), new DateTime(2017, 11, 15),
                    new DateTime(2017, 11, 21), new DateTime(2017, 11, 25), new DateTime(2017, 11, 29)
                },
                new[] { 1.2, 0.3, 2.1, 2.9, 4.2, 5.3 });

            ChartAxis xAxis = chart.AxisX;
            ChartAxis yAxis = chart.AxisY;

            // Set X axis bounds.
            xAxis.Scaling.Minimum = new AxisBound(new DateTime(2017, 11, 05).ToOADate());
            xAxis.Scaling.Maximum = new AxisBound(new DateTime(2017, 12, 03));

            // Set major units to a week and minor units to a day.
            xAxis.BaseTimeUnit = AxisTimeUnit.Days;
            xAxis.MajorUnit = 7;
            xAxis.MinorUnit = 1;
            xAxis.MajorTickMark = AxisTickMark.Cross;
            xAxis.MinorTickMark = AxisTickMark.Outside;

            // Define Y axis properties.
            yAxis.TickLabelPosition = AxisTickLabelPosition.High;
            yAxis.MajorUnit = 100;
            yAxis.MinorUnit = 50;
            yAxis.DisplayUnit.Unit = AxisBuiltInUnit.Hundreds;
            yAxis.Scaling.Minimum = new AxisBound(100);
            yAxis.Scaling.Maximum = new AxisBound(700);

            doc.Save(ArtifactsDir + "Charts.ChartAxisProperties.docx");
            //ExEnd
        }

        [Test]
        public void HideChartAxis()
        {
            //ExStart
            //ExFor:ChartAxis.Hidden
            //ExSummary:Shows how to hide chart axises.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert chart.
            Shape shape = builder.InsertChart(ChartType.Line, 432, 252);
            Chart chart = shape.Chart;
            chart.AxisX.Hidden = true;
            chart.AxisY.Hidden = true;

            // Clear demo data.
            chart.Series.Clear();
            chart.Series.Add("AW Series 1",
                new string[] { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" },
                new double[] { 1.2, 0.3, 2.1, 2.9, 4.2 });

            MemoryStream stream = new MemoryStream();
            doc.Save(stream, SaveFormat.Docx);

            shape = (Shape)doc.GetChild(NodeType.Shape, 0, true);
            chart = shape.Chart;

            Assert.AreEqual(true, chart.AxisX.Hidden);
            Assert.AreEqual(true, chart.AxisY.Hidden);
            //ExEnd
        }

        [Test]
        public void SetNumberFormatToChartAxis()
        {
            //ExStart
            //ExFor:ChartAxis.NumberFormat
            //ExFor:Charts.ChartNumberFormat
            //ExFor:ChartNumberFormat.FormatCode
            //ExFor:Charts.ChartNumberFormat.IsLinkedToSource
            //ExSummary:Shows how to set formatting for chart values.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert chart.
            Shape shape = builder.InsertChart(ChartType.Column, 432, 252);
            Chart chart = shape.Chart;

            // Clear demo data.
            chart.Series.Clear();

            chart.Series.Add("Aspose Test Series",
                new string[] { "Word", "PDF", "Excel", "GoogleDocs", "Note" },
                new double[] { 1900000, 850000, 2100000, 600000, 1500000 });

            // Set number format.
            chart.AxisY.NumberFormat.FormatCode = "#,##0";

            // Set this to override the above value and draw the number format from the source cell
            Assert.False(chart.AxisY.NumberFormat.IsLinkedToSource);
            //ExEnd

            doc.Save(ArtifactsDir + "Charts.SetNumberFormatToChartAxis.docx");
            doc.Save(ArtifactsDir + "Charts.SetNumberFormatToChartAxis.pdf");
        }

        // Note: Tests below used for verification conversion docx to pdf and the correct display.
        // For now, the results check manually.
        [Test]
        [TestCase(ChartType.Column)]
        [TestCase(ChartType.Line)]
        [TestCase(ChartType.Pie)]
        [TestCase(ChartType.Bar)]
        [TestCase(ChartType.Area)]
        public void TestDisplayChartsWithConversion(ChartType chartType)
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert chart.
            Shape shape = builder.InsertChart(chartType, 432, 252);
            Chart chart = shape.Chart;

            // Clear demo data.
            chart.Series.Clear();

            chart.Series.Add("Aspose Test Series",
                new string[] { "Word", "PDF", "Excel", "GoogleDocs", "Note" },
                new double[] { 1900000, 850000, 2100000, 600000, 1500000 });

            doc.Save(ArtifactsDir + "Charts.TestDisplayChartsWithConversion.docx");
            doc.Save(ArtifactsDir + "Charts.TestDisplayChartsWithConversion.pdf");
        }

        [Test]
        public void Surface3DChart()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert chart.
            Shape shape = builder.InsertChart(ChartType.Surface3D, 432, 252);
            Chart chart = shape.Chart;

            // Clear demo data.
            chart.Series.Clear();

            chart.Series.Add("Aspose Test Series 1",
                new string[] { "Word", "PDF", "Excel", "GoogleDocs", "Note" },
                new double[] { 1900000, 850000, 2100000, 600000, 1500000 });

            chart.Series.Add("Aspose Test Series 2",
                new string[] { "Word", "PDF", "Excel", "GoogleDocs", "Note" },
                new double[] { 900000, 50000, 1100000, 400000, 2500000 });

            chart.Series.Add("Aspose Test Series 3",
                new string[] { "Word", "PDF", "Excel", "GoogleDocs", "Note" },
                new double[] { 500000, 820000, 1500000, 400000, 100000 });

            doc.Save(ArtifactsDir + "Charts.SurfaceChart.docx");
            doc.Save(ArtifactsDir + "Charts.SurfaceChart.pdf");
        }

        [Test]
        public void BubbleChart()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert chart.
            Shape shape = builder.InsertChart(ChartType.Bubble, 432, 252);
            Chart chart = shape.Chart;

            // Clear demo data.
            chart.Series.Clear();

            chart.Series.Add("Aspose Test Series",
                new double[] { 2900000, 350000, 1100000, 400000, 400000 },
                new double[] { 1900000, 850000, 2100000, 600000, 1500000 },
                new double[] { 900000, 450000, 2500000, 800000, 500000 });

            doc.Save(ArtifactsDir + "Charts.BubbleChart.docx");
            doc.Save(ArtifactsDir + "Charts.BubbleChart.pdf");
        }

        //ExStart
        //ExFor:Charts.ChartSeries
        //ExFor:Charts.ChartSeries.DataLabels
        //ExFor:Charts.ChartSeries.DataPoints
        //ExFor:Charts.ChartSeries.Name
        //ExFor:Charts.ChartDataLabel
        //ExFor:Charts.ChartDataLabel.Index
        //ExFor:Charts.ChartDataLabel.IsVisible
        //ExFor:Charts.ChartDataLabel.NumberFormat
        //ExFor:Charts.ChartDataLabel.Separator
        //ExFor:Charts.ChartDataLabel.ShowCategoryName
        //ExFor:Charts.ChartDataLabel.ShowDataLabelsRange
        //ExFor:Charts.ChartDataLabel.ShowLeaderLines
        //ExFor:Charts.ChartDataLabel.ShowLegendKey
        //ExFor:Charts.ChartDataLabel.ShowPercentage
        //ExFor:Charts.ChartDataLabel.ShowSeriesName
        //ExFor:Charts.ChartDataLabel.ShowValue
        //ExFor:Charts.ChartDataLabelCollection
        //ExFor:Charts.ChartDataLabelCollection.Add(System.Int32)
        //ExFor:Charts.ChartDataLabelCollection.Clear
        //ExFor:Charts.ChartDataLabelCollection.Count
        //ExFor:Charts.ChartDataLabelCollection.GetEnumerator
        //ExFor:Charts.ChartDataLabelCollection.Item(System.Int32)
        //ExFor:Charts.ChartDataLabelCollection.RemoveAt(System.Int32)
        //ExSummary:Shows how to apply labels to data points in a chart.
        [Test] //ExSkip
        public void ChartDataLabels()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            
            // Use a document builder to insert a bar chart
            Shape chartShape = builder.InsertChart(ChartType.Line, 400, 300);

            // Get the chart object from the containing shape
            Chart chart = chartShape.Chart;

            // The chart already contains demo data comprised of 3 series each with 4 categories
            Assert.AreEqual(3, chart.Series.Count);
            Assert.AreEqual("Series 1", chart.Series[0].Name);

            // Apply data labels to every series in the graph
            foreach (ChartSeries series in chart.Series)
            {
                ApplyDataLabels(series, 4, "000.0", ", ");
                Assert.AreEqual(4, series.DataLabels.Count);
            }

            // Get the enumerator for a data label collection
            using (IEnumerator<ChartDataLabel> enumerator = chart.Series[0].DataLabels.GetEnumerator())
            {
                // And use it to go over all the data labels in one series and change their separator
                while (enumerator.MoveNext())
                {
                    Assert.AreEqual(", ", enumerator.Current.Separator);
                    enumerator.Current.Separator = " & ";
                }
            }

            // If the chart looks too busy, we can remove data labels one by one
            chart.Series[1].DataLabels.RemoveAt(2);

            // We can also clear an entire data label collection for one whole series
            chart.Series[2].DataLabels.Clear();

            doc.Save(ArtifactsDir + "Charts.ChartDataLabels.docx");
        }

        /// <summary>
        /// Apply uniform data labels with custom number format and separator to a number (determined by labelsCount) of data points in a series
        /// </summary>
        private void ApplyDataLabels(ChartSeries series, int labelsCount, string numberFormat, string separator)
        {
            for (int i = 0; i < labelsCount; i++)
            {
                ChartDataLabel label = series.DataLabels.Add(i);
                Assert.False(label.IsVisible);

                // Edit the appearance of the new data label
                label.ShowCategoryName = true;
                label.ShowSeriesName = true;
                label.ShowValue = true;
                label.ShowLeaderLines = true;
                label.ShowLegendKey = true;
                label.ShowPercentage = false;
                Assert.False(label.ShowDataLabelsRange);

                // Apply number format and separator
                label.NumberFormat.FormatCode = numberFormat;
                label.Separator = separator;

                // The label automatically becomes visible
                Assert.True(label.IsVisible);
            }
        }
        //ExEnd

        //ExStart
        //ExFor:Charts.ChartSeries.Smooth
        //ExFor:Charts.ChartDataPoint
        //ExFor:Charts.ChartDataPoint.Index
        //ExFor:Charts.ChartDataPointCollection
        //ExFor:Charts.ChartDataPointCollection.Add(System.Int32)
        //ExFor:Charts.ChartDataPointCollection.Clear
        //ExFor:Charts.ChartDataPointCollection.Count
        //ExFor:Charts.ChartDataPointCollection.GetEnumerator
        //ExFor:Charts.ChartDataPointCollection.Item(System.Int32)
        //ExFor:Charts.ChartDataPointCollection.RemoveAt(System.Int32)
        //ExFor:Charts.ChartMarker
        //ExFor:Charts.ChartMarker.Size
        //ExFor:Charts.ChartMarker.Symbol
        //ExFor:Charts.IChartDataPoint
        //ExFor:Charts.IChartDataPoint.InvertIfNegative
        //ExFor:Charts.IChartDataPoint.Marker
        //ExFor:Charts.MarkerSymbol
        //ExSummary:Shows how to customize chart data points.
        [Test]
        public void ChartDataPoint()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Add a line chart, which will have default data that we will use
            Shape shape = builder.InsertChart(ChartType.Line, 500, 350);
            Chart chart = shape.Chart;

            // Apply diamond-shaped data points to the line of the first series
            foreach (ChartSeries series in chart.Series)
            {
                ApplyDataPoints(series, 4, MarkerSymbol.Diamond, 15);
            }

            // We can further decorate a series line by smoothing it
            chart.Series[0].Smooth = true;

            // Get the enumerator for the data point collection from one series
            using (IEnumerator<ChartDataPoint> enumerator = chart.Series[0].DataPoints.GetEnumerator())
            {
                // And use it to go over all the data labels in one series and change their separator
                while (enumerator.MoveNext())
                {
                    Assert.False(enumerator.Current.InvertIfNegative);
                }
            }

            // If the chart looks too busy, we can remove data points one by one
            chart.Series[1].DataPoints.RemoveAt(2);

            // We can also clear an entire data point collection for one whole series
            chart.Series[2].DataPoints.Clear();

            doc.Save(ArtifactsDir + "Charts.ChartDataPoint.docx");
        }

        /// <summary>
        /// Applies a number of data points to a series
        /// </summary>
        private void ApplyDataPoints(ChartSeries series, int dataPointsCount, MarkerSymbol markerSymbol, int dataPointSize)
        {
            for (int i = 0; i < dataPointsCount; i++)
            {
                ChartDataPoint point = series.DataPoints.Add(i);
                point.Marker.Symbol = markerSymbol;
                point.Marker.Size = dataPointSize;

                Assert.AreEqual(i, point.Index);
            }
        }
        //ExEnd

        [Test]
        public void PieChartExplosion()
        {
            //ExStart
            //ExFor:Charts.IChartDataPoint.Explosion
            //ExSummary:Shows how to manipulate the position of the portions of a pie chart.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            Shape shape = builder.InsertChart(ChartType.Pie, 500, 350);
            Chart chart = shape.Chart;

            // In a pie chart, the portions are the data points, which cannot have markers or sizes applied to them
            // However, we can set this variable to move any individual "slice" away from the center of the chart
            ChartDataPoint cdp = chart.Series[0].DataPoints.Add(0);
            cdp.Explosion = 10;

            cdp = chart.Series[0].DataPoints.Add(1);
            cdp.Explosion = 40;

            doc.Save(ArtifactsDir + "Charts.PieChartExplosion.docx");
            //ExEnd
        }

        [Test]
        public void Bubble3D()
        {
            //ExStart
            //ExFor:Charts.ChartDataLabel.ShowBubbleSize
            //ExFor:Charts.IChartDataPoint.Bubble3D
            //ExSummary:Demonstrates bubble chart-exclusive features.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert a bubble chart with 3D effects on each bubble
            Shape shape = builder.InsertChart(ChartType.Bubble3D, 500, 350);
            Chart chart = shape.Chart;

            Assert.True(chart.Series[0].Bubble3D);

            // Apply a data label to each bubble that displays the size of its bubble
            for (int i = 0; i < 3; i++)
            {
                ChartDataLabel cdl = chart.Series[0].DataLabels.Add(i);
                cdl.ShowBubbleSize = true;
            }
            
            doc.Save(ArtifactsDir + "Charts.Bubble3D.docx");
            //ExEnd
        }

        //ExStart
        //ExFor:Charts.ChartAxis.Type
        //ExFor:Charts.ChartAxisType
        //ExFor:Charts.ChartType
        //ExFor:Charts.Chart.Series
        //ExFor:Charts.ChartSeriesCollection.Add(String,DateTime[],Double[])
        //ExFor:Charts.ChartSeriesCollection.Add(String,Double[],Double[])
        //ExFor:Charts.ChartSeriesCollection.Add(String,Double[],Double[],Double[])
        //ExFor:Charts.ChartSeriesCollection.Add(String,String[],Double[])
        //ExSummary:Shows an appropriate graph type for each chart series.
        [Test] //ExSkip
        public void ChartSeriesCollection()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            
            // There are 4 ways of populating a chart's series collection
            // 1: Each series has a string array of categories, each with a corresponding data value
            // Some of the other possible applications are bar, column, line and surface charts
            Chart chart = AppendChart(builder, ChartType.Column, 300, 300);

            // Create and name 3 categories with a string array
            string[] categories = { "Category 1", "Category 2", "Category 3" };

            // Create 2 series of data, each with one point for every category
            // This will generate a column graph with 3 clusters of 2 bars
            chart.Series.Add("Series 1", categories, new [] { 76.6, 82.1, 91.6 });
            chart.Series.Add("Series 2", categories, new [] { 64.2, 79.5, 94.0 });

            // Categories are distributed along the X-axis while values are distributed along the Y-axis
            Assert.AreEqual(ChartAxisType.Category, chart.AxisX.Type);
            Assert.AreEqual(ChartAxisType.Value, chart.AxisY.Type);

            // 2: Each series will have a collection of dates with a corresponding value for each date
            // Area, radar and stock charts are some of the appropriate chart types for this
            chart = AppendChart(builder, ChartType.Area, 300, 300);

            // Create a collection of dates to serve as categories
            DateTime[] dates = { new DateTime(2014, 3, 31),
                new DateTime(2017, 1, 23),
                new DateTime(2017, 6, 18),
                new DateTime(2019, 11, 22),
                new DateTime(2020, 9, 7)
            };

            // Add one series with one point for each date
            // Our sporadic dates will be distributed along the X-axis in a linear fashion 
            chart.Series.Add("Series 1", dates, new [] { 15.8, 21.5, 22.9, 28.7, 33.1 });

            // 3: Each series will take two data arrays
            // Appropriate for scatter plots
            chart = AppendChart(builder, ChartType.Scatter, 300, 300);

            // In each series, the first array contains the X-coordinates and the second contains respective Y-coordinates of points
            chart.Series.Add("Series 1", new[] { 3.1, 3.5, 6.3, 4.1, 2.2, 8.3, 1.2, 3.6 }, new[] { 3.1, 6.3, 4.6, 0.9, 8.5, 4.2, 2.3, 9.9 });
            chart.Series.Add("Series 2", new[] { 2.6, 7.3, 4.5, 6.6, 2.1, 9.3, 0.7, 3.3 }, new[] { 7.1, 6.6, 3.5, 7.8, 7.7, 9.5, 1.3, 4.6 });

            // Both axes are value axes in this case
            Assert.AreEqual(ChartAxisType.Value, chart.AxisX.Type);
            Assert.AreEqual(ChartAxisType.Value, chart.AxisY.Type);

            // 4: Each series will be built from three data arrays, used for bubble charts
            chart = AppendChart(builder, ChartType.Bubble, 300, 300);

            // The first two arrays contain X/Y coordinates like above and the third determines the thickness of each point
            chart.Series.Add("Series 1", new [] { 1.1, 5.0, 9.8 }, new [] { 1.2, 4.9, 9.9 }, new [] { 2.0, 4.0, 8.0 });

            doc.Save(ArtifactsDir + "Charts.ChartSeriesCollection.docx");
        }
        
        /// <summary>
        /// Get the DocumentBuilder to insert a chart of a specified ChartType, width and height and clean out its default data
        /// </summary>
        private Chart AppendChart(DocumentBuilder builder, ChartType chartType, double width, double height)
        {
            Shape chartShape = builder.InsertChart(chartType, width, height);
            Chart chart = chartShape.Chart;
            chart.Series.Clear();

            Assert.AreEqual(0, chart.Series.Count);

            return chart;
        }
        //ExEnd

        [Test]
        public void ChartSeriesCollectionModify()
        {
            //ExStart
            //ExFor:Charts.ChartSeriesCollection
            //ExFor:Charts.ChartSeriesCollection.Clear
            //ExFor:Charts.ChartSeriesCollection.Count
            //ExFor:Charts.ChartSeriesCollection.GetEnumerator
            //ExFor:Charts.ChartSeriesCollection.Item(Int32)
            //ExFor:Charts.ChartSeriesCollection.RemoveAt(Int32)
            //ExSummary:Shows how to work with a chart's data collection.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Use a document builder to insert a bar chart
            Shape chartShape = builder.InsertChart(ChartType.Column, 400, 300);
            Chart chart = chartShape.Chart;

            // All charts come with demo data
            // This column chart currently has 3 series with 4 categories, which means 4 clusters, 3 columns in each
            ChartSeriesCollection chartData = chart.Series;
            Assert.AreEqual(3, chartData.Count);

            // Iterate through the series with an enumerator and print their names
            using (IEnumerator<ChartSeries> enumerator = chart.Series.GetEnumerator())
            {
                // And use it to go over all the data labels in one series and change their separator
                while (enumerator.MoveNext())
                {
                    Console.WriteLine(enumerator.Current.Name);
                }
            }

            // We can add new data by adding a new series to the collection, with categories and data
            // We will match the existing category/series names in the demo data and add a 4th column to each column cluster
            string[] categories = { "Category 1", "Category 2", "Category 3", "Category 4" };
            chart.Series.Add("Series 4", categories, new[] { 4.4, 7.0, 3.5, 2.1 });

            Assert.AreEqual(4, chartData.Count);
            Assert.AreEqual("Series 4", chartData[3].Name);

            // We can remove series by index
            chartData.RemoveAt(2);

            Assert.AreEqual(3, chartData.Count);
            Assert.AreEqual("Series 4", chartData[2].Name);

            // We can also remove out all the series
            // This leaves us with an empty graph and is a convenient way of wiping out demo data
            chartData.Clear();

            Assert.AreEqual(0, chartData.Count);
            //ExEnd
        }

        [Test]
        public void AxisScaling()
        {
            //ExStart
            //ExFor:Charts.AxisScaleType
            //ExFor:Charts.AxisScaling
            //ExFor:Charts.AxisScaling.LogBase
            //ExFor:Charts.AxisScaling.Type
            //ExSummary:Shows how to set up logarithmic axis scaling.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert a scatter chart and clear its default data series
            Shape chartShape = builder.InsertChart(ChartType.Scatter, 450, 300);
            Chart chart = chartShape.Chart;
            chart.Series.Clear();

            // Insert a series with X/Y coordinates for 5 points
            chart.Series.Add("Series 1", new[] { 1.0, 2.0, 3.0, 4.0, 5.0 }, new[] { 1.0, 20.0, 400.0, 8000.0, 160000.0 });

            // The scaling of the X axis is linear by default, which means it will display "0, 1, 2, 3..."
            Assert.AreEqual(AxisScaleType.Linear, chart.AxisX.Scaling.Type);

            // Linear axis scaling is suitable for our X-values, but not our erratic Y-values 
            // We can set the scaling of the Y-axis to Logarithmic with a base of 20
            // The Y-axis will now display "1, 20, 400, 8000...", which is ideal for accurate representation of this set of Y-values
            chart.AxisY.Scaling.Type = AxisScaleType.Logarithmic;
            chart.AxisY.Scaling.LogBase = 20.0;

            doc.Save(ArtifactsDir + "Charts.AxisScaling.docx");
            //ExEnd
        }

        [Test]
        public void AxisBound()
        {
            //ExStart
            //ExFor:Charts.AxisBound.#ctor
            //ExFor:Charts.AxisBound.IsAuto
            //ExFor:Charts.AxisBound.Value
            //ExFor:Charts.AxisBound.ValueAsDate
            //ExSummary:Shows how to set custom axis bounds.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert a scatter chart, remove default data and populate it with data from a ChartSeries
            Shape chartShape = builder.InsertChart(ChartType.Scatter, 450, 300);
            Chart chart = chartShape.Chart;
            chart.Series.Clear();
            chart.Series.Add("Series 1", new[] { 1.1, 5.4, 7.9, 3.5, 2.1, 9.7 }, new[] { 2.1, 0.3, 0.6, 3.3, 1.4, 1.9 });

            // By default, the axis bounds are automatically defined so all the series data within the table is included
            Assert.True(chart.AxisX.Scaling.Minimum.IsAuto);

            // If we wish to set our own scale bounds, we need to replace them with new ones
            // Both the axis rulers will go from 0 to 10
            chart.AxisX.Scaling.Minimum = new AxisBound(0);
            chart.AxisX.Scaling.Maximum = new AxisBound(10);
            chart.AxisY.Scaling.Minimum = new AxisBound(0);
            chart.AxisY.Scaling.Maximum = new AxisBound(10);

            // These are custom and not defined automatically
            Assert.False(chart.AxisX.Scaling.Minimum.IsAuto);
            Assert.False(chart.AxisY.Scaling.Minimum.IsAuto);

            // Create a line graph
            chartShape = builder.InsertChart(ChartType.Line, 450, 300);
            chart = chartShape.Chart;
            chart.Series.Clear();

            // Create a collection of dates, which will make up the X axis
            DateTime[] dates = { new DateTime(1973, 5, 11),
                new DateTime(1981, 2, 4),
                new DateTime(1985, 9, 23),
                new DateTime(1989, 6, 28),
                new DateTime(1994, 12, 15)
            };

            // Assign a Y-value for each date 
            chart.Series.Add("Series 1", dates, new[] { 3.0, 4.7, 5.9, 7.1, 8.9 });

            // These particular bounds will cut off categories from before 1980 and from 1990 and onwards
            // This narrows the amount of categories and values in the viewport from 5 to 3
            // Note that the graph still contains the out-of-range data because we can see the line tend towards it
            chart.AxisX.Scaling.Minimum = new AxisBound(new DateTime(1980, 1, 1));
            chart.AxisX.Scaling.Maximum = new AxisBound(new DateTime(1990, 1, 1));

            doc.Save(ArtifactsDir + "Charts.AxisBound.docx");
            //ExEnd
        }

        [Test]
        public void ChartLegend()
        {
            //ExStart
            //ExFor:Charts.Chart.Legend
            //ExFor:Charts.ChartLegend
            //ExFor:Charts.ChartLegend.Overlay
            //ExFor:Charts.ChartLegend.Position
            //ExFor:Charts.LegendPosition
            //ExSummary:Shows how to edit the appearance of a chart's legend.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert a line graph
            Shape chartShape = builder.InsertChart(ChartType.Line, 450, 300);
            Chart chart = chartShape.Chart;

            // Get its legend
            ChartLegend legend = chart.Legend;

            // By default, other elements of a chart will not overlap with its legend
            Assert.False(legend.Overlay);

            // We can move its position by setting this attribute
            legend.Position = LegendPosition.TopRight;

            doc.Save(ArtifactsDir + "Charts.ChartLegend.docx");
            //ExEnd
        }

        [Test]
        public void AxisCross()
        {
            //ExStart
            //ExFor:Charts.ChartAxis.AxisBetweenCategories
            //ExFor:Charts.ChartAxis.CrossesAt
            //ExSummary:Shows how to get a graph axis to cross at a custom location.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert a column chart, which is populated by default values
            Shape shape = builder.InsertChart(ChartType.Column, 450, 250);
            Chart chart = shape.Chart;

            // Get the Y-axis to cross at a value of 3.0, making 3.0 the new Y-zero of our column chart
            // This effectively means that all the columns with Y-values about 3.0 will be above the Y-centre and point up,
            // while ones below 3.0 will point down
            ChartAxis axis = chart.AxisX;
            axis.AxisBetweenCategories = true;
            axis.Crosses = AxisCrosses.Custom;
            axis.CrossesAt = 3.0;

            doc.Save(ArtifactsDir + "Charts.AxisCross.docx");
            //ExEnd
        }

        [Test]
        public void ChartAxisDisplayUnit()
        {
            //ExStart
            //ExFor:Charts.AxisBuiltInUnit
            //ExFor:Charts.ChartAxis.DisplayUnit
            //ExFor:Charts.ChartAxis.MajorUnitIsAuto
            //ExFor:Charts.ChartAxis.MajorUnitScale
            //ExFor:Charts.ChartAxis.MinorUnitIsAuto
            //ExFor:Charts.ChartAxis.MinorUnitScale
            //ExFor:Charts.ChartAxis.TickLabelSpacing
            //ExFor:Charts.ChartAxis.TickLabelAlignment
            //ExFor:Charts.AxisDisplayUnit
            //ExFor:Charts.AxisDisplayUnit.CustomUnit
            //ExFor:Charts.AxisDisplayUnit.Unit
            //ExSummary:Shows how to manipulate the tick marks and displayed values of a chart axis.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Insert a scatter chart, which is populated by default values
            Shape shape = builder.InsertChart(ChartType.Scatter, 450, 250);
            Chart chart = shape.Chart;

            // Set they Y axis to show major ticks every at every 10 units and minor ticks at every 1 units
            ChartAxis axis = chart.AxisY;
            axis.MajorTickMark = AxisTickMark.Outside;
            axis.MinorTickMark = AxisTickMark.Outside;

            axis.MajorUnit = 10.0;
            axis.MinorUnit = 1.0;

            // Stretch out the bounds of the axis out to show 3 major ticks and 27 minor ticks
            axis.Scaling.Minimum = new AxisBound(-10);
            axis.Scaling.Maximum = new AxisBound(20);

            // Do the same for the X-axis
            axis = chart.AxisX;
            axis.MajorTickMark = AxisTickMark.Inside;
            axis.MinorTickMark = AxisTickMark.Inside;
            axis.MajorUnit = 10.0;
            axis.Scaling.Minimum = new AxisBound(-10);
            axis.Scaling.Maximum = new AxisBound(30);

            // We can also use this attribute to set minor tick spacing
            axis.TickLabelSpacing = 2;
            // We can define text alignment when axis tick labels are multi-line
            // MS Word aligns them to the center by default
            axis.TickLabelAlignment = ParagraphAlignment.Right;

            // Get the axis to display values, but in millions
            axis.DisplayUnit.Unit = AxisBuiltInUnit.Millions;

            // Besides the built-in axis units we can choose from,
            // we can also set the axis to display values in some custom denomination, using the following attribute
            // The statement below is equivalent to the one above
            axis.DisplayUnit.CustomUnit = 1000000.0;

            doc.Save(ArtifactsDir + "Charts.ChartAxisDisplayUnit.docx");
            //ExEnd
        }
    }
}