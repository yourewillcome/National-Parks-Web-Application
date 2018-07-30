using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Web.DAL
{
    public interface IParksWeatherDAL
    {
        List<ParkIndexModel> GetParks();
        Parks GetSpecificPark(string parkId);
        List<Weather> GetForecast(string parkId);
        bool SaveSurvey(Surveys survey);
        List<SurveyResultModel> GetSurveyResults();
        List<ParkCodeAndNameModel> GetParkNamesAndCodes();
    }
}
