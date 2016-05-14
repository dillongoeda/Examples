using OdataDemo;
using ODataDemo.Models;
using System;
using System.Web.Mvc;

namespace ODataDemoClient.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            var context = new OdataDemoContainer(new Uri("http://localhost:64874/api"));
            var person = context.People.ByKey(1).GetValue();
            var person2 = context.People.ByKey(2).GetValue();

            context.AddLink(person, "Friends", person2);

            context.SaveChanges();

            return View();
        }
    }
}