using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class SurveyResultModel
    {
        public int Ranking { get; set; }
        public string ParkName { get; set; }
        public string ParkCode { get; set; }
        public string ParkDescription { get; set; }
    }
}