using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AutoMapper;
using EvaluatingWebsitePerformance.Common.DTO;
using EvaluatingWebsitePerformance.ViewModel;
using SimpleChart = System.Web.Helpers;

namespace EvaluatingWebsitePerformance.Controllers
{
    public class AnalysisController : BaseController<WebsiteDto>
    {
        protected HttpWebRequest request;
        protected HttpWebResponse response;
        protected List<TimeSpan> listRequestTime = new List<TimeSpan>();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(WebsiteViewModel viewmodel)
        {
            
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[100000];
            request = (HttpWebRequest)WebRequest.Create(viewmodel.Url);
            var stopwatch = Stopwatch.StartNew();
            response = (HttpWebResponse) request.GetResponse();
            stopwatch.Stop();
            var timeResponse = stopwatch.Elapsed;
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

            var sb1 = sb.ToString();
            ManagePage(sb1, viewmodel);
            
            return RedirectToAction("Index");
        }

        private void ManagePage(string st, WebsiteViewModel viewModel)
        {
            var list = new List<string>();
            Match m;
            string pattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            var dto = Mapper.Map<WebsiteViewModel, WebsiteDto>(viewModel);
            dto.Name = viewModel.Name;
            m = Regex.Match(st, pattern, RegexOptions.IgnoreCase);
            while (m.Success)
            {
                dto.Url = m.Groups[1].Value;
                handler.Add(dto);
                m = m.NextMatch();
            }
        }

        public ActionResult CreateGrafic()
        {
            var chart = new SimpleChart.Chart(width: 500, height: 200)
                .AddTitle("graphical line of response time")
                .AddSeries(
                    name: "my programm",
                    chartType: "Line",
                    xValue: new[]{"peter", "ira", "jula"},
                    yValues:new[]{1,8,5})
                .AddLegend()
                .Write();
            return null;
        }
    }
}