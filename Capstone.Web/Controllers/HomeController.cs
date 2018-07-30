using Capstone.Web.DAL;
using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {
        //member variable
        private IParksWeatherDAL _dal;

        //constructor
        public HomeController(IParksWeatherDAL dal)
        {
            _dal = dal;
        }

        // GET: Home
        public ActionResult Index()
        {
            List<ParkIndexModel> parks = new List<ParkIndexModel>();

            parks = _dal.GetParks();

            return View("Index", parks);
        }

        public ActionResult Detail(string parkCode, string scale)
        {
            if(parkCode == null)
            {
                parkCode = Session["ParkCode"] as string;
                if(parkCode == null)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Session["ParkCode"] = parkCode;
            }

            if(scale != null)
            {
                Session["Scale"] = scale;
            }
            Parks park = new Parks();
            List<Weather> parkForecast = new List<Weather>();
            park = _dal.GetSpecificPark(parkCode);
            parkForecast = _dal.GetForecast(parkCode);
            Tuple<Parks, List<Weather>> parkWithForecast = new Tuple<Parks, List<Weather>>(park, parkForecast);

            return View("Detail", parkWithForecast);
        }

        public ActionResult Survey()
        {
            List<ParkCodeAndNameModel> parkNames = new List<ParkCodeAndNameModel>();
            Surveys survey = new Surveys();
            parkNames = _dal.GetParkNamesAndCodes();
            if (parkNames == null)
            {
                parkNames = Session["parkNames"] as List<ParkCodeAndNameModel>;
            }
            else
            {
                Session["parkNames"] = parkNames;
            }
            return View("Survey");
        }

        [HttpPost]
        public ActionResult Survey(Surveys survey)
        {
            ActionResult result;
            if(!ModelState.IsValid)
            {
                result = View("Survey", survey);
            }
            else
            {
                if (_dal.SaveSurvey(survey))
                {
                    result = RedirectToAction("SurveyResult");
                }
                else
                {
                    result = View("Survey", survey);
                }
            }
            return result;
        }

        public ActionResult SurveyResult()
        {
            List<SurveyResultModel> surveys = new List<SurveyResultModel>();

            surveys = _dal.GetSurveyResults();

            return View("SurveyResult", surveys);
        }
    }
}