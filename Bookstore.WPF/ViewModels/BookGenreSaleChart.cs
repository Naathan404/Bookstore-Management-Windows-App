using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Bookstore.WPF.ViewModels
{
    public class BookGenreSaleChart
    {
        // LiveCharts v2 cần ISeries[] để vẽ
        public ISeries[] Data { get; set; }

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