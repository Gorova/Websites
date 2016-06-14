using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EvaluatingWebsitePerformance.Common.DTO;
using EvaluatingWebsitePerformance.ViewModel;

namespace EvaluatingWebsitePerformance.Controllers
{
    public class ArchiveController : BaseController<WebsiteDto>
    {
        public ActionResult Index()
        {
            var mainPages = Mapper.Map<IEnumerable<WebsiteDto>, IEnumerable<WebsiteViewModel>>(handler.GetAll().Where(i => i.ParentId == 0));

            return View(mainPages);
        }

        public JsonResult GetArchive(int id)
        {
            var newList = new List<WebsiteDto>();
            var allWebsites = handler.GetAll().ToList();
            var list = allWebsites.Where(i => i.ParentId == id).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < allWebsites.Count; j++)
                {
                    if (list[i].Id == allWebsites[j].ParentId)
                    {
                        newList.Add(allWebsites[j]);
                    }

                }
            }
            newList.AddRange(list);
            newList.Add(allWebsites.FirstOrDefault(i => i.Id == id));

            return Json(newList, JsonRequestBehavior.AllowGet);
        }
    }
}