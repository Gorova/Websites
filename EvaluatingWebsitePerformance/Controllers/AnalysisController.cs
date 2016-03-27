using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.DataVisualization.Charting;
using AutoMapper;
using EvaluatingWebsitePerformance.Common.DTO;
using EvaluatingWebsitePerformance.ViewModel;


namespace EvaluatingWebsitePerformance.Controllers
{
    public class AnalysisController : BaseController<WebsiteDto>
    {
        protected List<string> list = new List<string>();
        protected List<long> listRequestTime = new List<long>();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(WebsiteViewModel viewModel)
        {
            var htmlString = GetHtml(viewModel);
            
            ManagePage(htmlString, viewModel);
            Parallel.ForEach(list, (currentElement) =>
            {
                GetInnerHtml(currentElement);
            });

            GetChart();

            return RedirectToAction("Index");
        }

        private string GetHtml(WebsiteViewModel viewModel)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[100000];
            var request = (HttpWebRequest)WebRequest.Create(viewModel.Url);
            var response = (HttpWebResponse) request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            int count = 0;
            do
            {
                count = responseStream.Read(buf, 0, buf.Length);
                if (count != null)
                {
                    sb.Append(Encoding.Default.GetString(buf, 0, count));
                }
            } 
            while (count > 0);

            var sbForAnalyzing = sb.ToString();

            return sbForAnalyzing;
        }

        private void ManagePage(string htmlString, WebsiteViewModel viewModel)
        {
            var mainHost = new Uri(viewModel.Url).Host;
            string reference;
            string referenceHost;
            string pattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            
            var dto = Mapper.Map<WebsiteViewModel, WebsiteDto>(viewModel);
            dto.Name = viewModel.Name;
            var match = Regex.Match(htmlString, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            
            while (match.Success)
            {
                reference = match.Groups[1].Value;
                Uri uri = null;
                if (Uri.TryCreate(reference, UriKind.Absolute, out uri))
                {
                    referenceHost = uri.Host;
                    if (referenceHost == mainHost && !list.Any(i => i == reference))
                    {
                        list.Add(reference);

                        dto.Url = reference;
                        handler.Add(dto);
                    }
                }

                match = match.NextMatch();
            }
        }

        private string GetInnerHtml(string reference)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[100000];
            var request = (HttpWebRequest)WebRequest.Create(reference);
            var stopwatch = Stopwatch.StartNew();
            var response = (HttpWebResponse)request.GetResponse();
            stopwatch.Stop();
            var timeResponse = stopwatch.ElapsedMilliseconds;
            listRequestTime.Add(timeResponse);
            Stream responseStream = response.GetResponseStream();

            int count = 0;
            do
            {
                count = responseStream.Read(buf, 0, buf.Length);
                if (count != null)
                {
                    sb.Append(Encoding.Default.GetString(buf, 0, count));
                }
            }
            while (count > 0);

            var sbForAnalyzing = sb.ToString();

            return sbForAnalyzing;
        }

        public FileContentResult GetChart()
        {
            List<Tuple<string, long>> datas = new List<Tuple<string, long>>();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j < listRequestTime.Count; j++)
                {
                    datas.Add(new Tuple<string, long>(list[i], listRequestTime[j]));
                    break;
                }
            }
            
            var chart = new Chart();
            chart.Width = 700;
            chart.Height = 300;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle());
            chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(datas, SeriesChartType.Line, Color.Red));
            chart.ChartAreas.Add(CreateArea());

            var memoryStream = new MemoryStream();
            chart.SaveImage(memoryStream);
            return File(memoryStream.GetBuffer(), @"image/png");
        }

        [NonAction]
        public Title CreateTitle()
        {
            Title title = new Title();
            title.Text = "result";
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);

            return title;
        }

        [NonAction]
        public Series CreateSeries(IList<Tuple<string, long>> datas, SeriesChartType chartType, Color color)
        {
            var seriesDetail = new Series();
            seriesDetail.Name = "result";
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = color;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            seriesDetail["DrawingStyle"] = "Cylinder";
            seriesDetail["pieDrawingStyle"] = "SoftEdge";
            DataPoint point;

            if (datas != null)
            {
                foreach (var data in datas)
                {
                    point = new DataPoint();
                    point.AxisLabel = data.Item1;
                    point.YValues = new double[] { data.Item2 };
                    seriesDetail.Points.Add(point);
                }
            }
            
            seriesDetail.ChartArea = "result";

            return seriesDetail;
        }

        [NonAction]
        public Legend CreateLegend()
        {
            var legend = new Legend();
            legend.Name = "result";
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 9);
            legend.LegendStyle = LegendStyle.Row;

            return legend;
        }

        [NonAction]
        public ChartArea CreateArea()
        {
            var chartArea = new ChartArea();
            chartArea.Name = "result";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;

            return chartArea;
        }
    }
}
