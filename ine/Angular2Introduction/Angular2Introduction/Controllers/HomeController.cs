using System.Collections.Generic;
using System.Web.Mvc;

namespace Angular2Introduction.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MainComponent()
        {
            return View();
        }

        public ActionResult CoursesComponent()
        {
            return View();
        }

        public ActionResult PersonList()
        {
            return View();
        }

        public JsonResult GetPersons(string name)
        {
            var person = new List<Models.Person>() { new Models.Person() { Name = "Andre", Id = 1 }, new Models.Person() { Name = "Hans", Id = 2 }, new Models.Person() { Name = name, Id = 1337 } };
            return Json(person, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddPerson(Models.Person person)
        {
            person.Name = person.Name + " POSTED";
            person.Id = 1337;

            return Json(person, JsonRequestBehavior.AllowGet);
        }
    }
}