using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;
using System.Web.Mvc;

namespace GenderReveal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public class GenderItem
        {
            public string Gender { get; set; }
            public int Value { get; set; }
        }

        [HttpGet]
        public JsonResult GetPollResults()
        {
            List<GenderItem> items = null;
            using (StreamReader r = new StreamReader(Server.MapPath("/data/poll.json")))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<GenderItem>>(json);
            }
            return Json(new{ data= items },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitVote(bool isBoy)
        {
            List<GenderItem> items = null;
            
            using (StreamReader r = new StreamReader(Server.MapPath("/data/poll.json")))
            {
                var json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<GenderItem>>(json);
            }
            if (isBoy)
            {
                items.First(x => x.Gender == "boy").Value = items.First(x => x.Gender == "boy").Value + 1;
            }
            else
            {
                items.First(x => x.Gender == "girl").Value = items.First(x => x.Gender == "girl").Value + 1;
            }

            using (StreamWriter file = new StreamWriter(Server.MapPath("/data/poll.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, items, typeof(List<GenderItem>));
            }

            return Json(new { data = items }, JsonRequestBehavior.AllowGet);
        }
    }
}