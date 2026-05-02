using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.IO.Packaging;

namespace Bookstore.WPF.ViewModels
{
    public class BookGenreSaleChart
    {
        // LiveCharts v2 cần ISeries[] để vẽ
        public ISeries[] Data { get; set; }
        public class Items
        {
            public string Name { get; set; }
            public string BrandName { get; set; } = string.Empty;
            public double Value { get; set; }
            public DateTime Date { get; set; }
            public string StatusColor { get; set; } = "#000000";
            public string StatusText { get; set; } = string.Empty;
        }
        public BookGenreSaleChart()
        {
            // Khởi tạo dữ liệu mẫu trực tiếp thành các PieSeries
            Data = new ISeries[]
            {
                new PieSeries<double> { Name = "Maria", Values = new double[] { 8 }, Pushout = 30 },
                new PieSeries<double> { Name = "Susan", Values = new double[] { 6 } },
                new PieSeries<double> { Name = "Charles", Values = new double[] { 5 } },
                new PieSeries<double> { Name = "Fiona", Values = new double[] { 3 } },
                new PieSeries<double> { Name = "George", Values = new double[] { 3 } }
            };
        }
    }
}