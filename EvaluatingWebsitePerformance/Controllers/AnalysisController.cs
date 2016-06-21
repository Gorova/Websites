using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AutoMapper;
using EvaluatingWebsitePerformance.Common.DTO;
using EvaluatingWebsitePerformance.ViewModel;
using WebGrease.Css.Extensions;

namespace EvaluatingWebsitePerformance.Controllers
{
    public class AnalysisController : BaseController<WebsiteDto>
    {
        protected List<string> list = new List<string>();
        private long timeResponse;
       
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(WebsiteViewModel viewModel)
        {
            CreateNewWebSite(viewModel);
            var htmlString = GetHtml(viewModel);
            var model = Mapper.Map<WebsiteDto, WebsiteViewModel>(handler.GetAll().FirstOrDefault(i => i.IsProcessed == false));
            ManagePage(htmlString, model);

            GetInnerWebsites(model);
      

            return RedirectToAction("Index");
        }

        public JsonResult GetAll()
        {
            var viewModels = Mapper.Map<IEnumerable<WebsiteDto>, IEnumerable<WebsiteViewModel>>(handler.GetAll());

            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }

        private string GetHtml(WebsiteViewModel viewModel)
        {
            HttpWebRequest request = null;
            
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[100000];

            request = (HttpWebRequest) WebRequest.Create(viewModel.Url);
            request.AllowAutoRedirect = false;
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

        public long GetTime(string reference)
        {
            HttpWebRequest request = null;
            request = (HttpWebRequest)WebRequest.Create(reference);
            request.AllowAutoRedirect = false;
            var stopwatch = Stopwatch.StartNew();
            var response = (HttpWebResponse)request.GetResponse();
            stopwatch.Stop();
            timeResponse = stopwatch.ElapsedMilliseconds;
            
            
            return timeResponse;
        }

        private void CreateNewWebSite( WebsiteViewModel viewModel)
        {
            var dto = Mapper.Map<WebsiteViewModel, WebsiteDto>(viewModel);
            dto.IsProcessed = false;
            dto.MillisecondsOfLoading = GetTime(dto.Url);
            handler.Add(dto);
        }

        private void ManagePage(string htmlString, WebsiteViewModel viewModel)
        {
            var mainHost = new Uri(viewModel.Url).Host;
            string reference;
            string referenceHost;
            string pattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            
            var match = Regex.Match(htmlString, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            
            while (match.Success)
            {
                reference = match.Groups[1].Value;
                Uri uri = null;
                if (Uri.TryCreate(reference, UriKind.Absolute, out uri) && uri.IsFile ==false)
                {
                    referenceHost = uri.Host;
                    if (referenceHost == mainHost && list.All(i => i != reference))
                    {
                        list.Add(reference);
                        var dto = new WebsiteDto();
                        dto.Url = reference;
                        dto.MillisecondsOfLoading = GetTime(reference);
                        dto.ParentId = viewModel.Id;
                        dto.IsProcessed = false;
                        handler.Add(dto);
                    }
                }
                match = match.NextMatch();
            }
        }

        private void GetInnerWebsites(WebsiteViewModel model)
        {
            var dto = Mapper.Map<WebsiteViewModel, WebsiteDto>(model);
            dto.IsProcessed = true;
            handler.Update(dto);
            
            var viewModels = Mapper.Map<IEnumerable<WebsiteDto>, IEnumerable<WebsiteViewModel>>(handler.GetAll().Where(i => i.ParentId == model.Id));

            viewModels.ForEach(i =>
            {
                var innerHtml = GetHtml(i);
                ManagePage(innerHtml, i);
            });
        }
    }
}
