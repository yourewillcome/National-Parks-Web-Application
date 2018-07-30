using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Capstone.Web.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class ParksWeatherSqlDAL : IParksWeatherDAL
    {
        //member variable
        private string _connectionString;

        //constructor
        public ParksWeatherSqlDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Methods

        /// <summary>
        /// Retrieves park code, name, state, and description from Park table in NationalParkForecast DB
        /// </summary>
        /// <returns>ParkIndexModel List</returns>
        public List<ParkIndexModel> GetParks()
        {
            const string parkSQL = "SELECT parkCode, parkName, state, parkDescription FROM park;";

            List<ParkIndexModel> parkList = new List<ParkIndexModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(parkSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        ParkIndexModel parks = GetSingleParkIndexModel(reader);
                        parkList.Add(parks);
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return parkList;
        }

        /// <summary>
        /// Populates a ParkIndexModel object from a SqlDataReader object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>ParkIndexModel object</returns>
        public ParkIndexModel GetSingleParkIndexModel(SqlDataReader reader)
        {
            return new ParkIndexModel()
            {
                ParkCode = Convert.ToString(reader["parkCode"]).ToLower(),
                ParkName = Convert.ToString(reader["parkName"]),
                State = Convert.ToString(reader["state"]),
                ParkDescription = Convert.ToString(reader["parkDescription"])
            };
        }

        /// <summary>
        /// Retrieves a list of Weather objects from Weather table in NationalParkForecast DB
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns>Weather List</returns>
        public List<Weather> GetForecast(string parkId)
        {
            List<Weather> forecast = new List<Weather>();
            const string getWeatherSql = "SELECT * FROM weather WHERE parkCode = @parkCode;";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(getWeatherSql, conn);
                    cmd.Parameters.AddWithValue("@parkCode", parkId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        forecast.Add(GetSingleWeatherObject(reader));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return forecast;
        }

        /// <summary>
        /// Populates a weather object from a SqlDataReader object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Weather object</returns>
        public Weather GetSingleWeatherObject(SqlDataReader reader)
        {
            return new Weather()
            {
                ParkCode = Convert.ToString(reader["parkCode"]).ToLower(),
                FiveDayForecastValue = Convert.ToInt32(reader["fiveDayForecastValue"]),
                Low = Convert.ToInt32(reader["low"]),
                High = Convert.ToInt32(reader["high"]),
                Forecast = Convert.ToString(reader["forecast"]),
            };
        }

        /// <summary>
        /// Gets information on a specific park from Park table in NationalParkForecast DB
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns>Park object</returns>
        public Parks GetSpecificPark(string parkId)
        {
            Parks park = new Parks();
            const string getParkSql = "SELECT * FROM park WHERE parkCode = @parkCode;";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(getParkSql, conn);
                    cmd.Parameters.AddWithValue("@parkCode", parkId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        park = GetSinglePark(reader);
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return park;
        }

        /// <summary>
        /// Populates a Parks object from a SqlDataReader object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Parks object</returns>
        public Parks GetSinglePark(SqlDataReader reader)
        {
            return new Parks()
            {
                ParkCode = Convert.ToString(reader["parkCode"]).ToLower(),
                ParkName = Convert.ToString(reader["parkName"]),
                State = Convert.ToString(reader["state"]),
                Acreage = Convert.ToInt32(reader["acreage"]),
                Elevation = Convert.ToInt32(reader["elevationInFeet"]),
                MilesOfTrail = Convert.ToDouble(reader["milesOfTrail"]),
                NumOfCampsites = Convert.ToInt32(reader["numberOfCampsites"]),
                Climate = Convert.ToString(reader["climate"]),
                YearFounded = Convert.ToInt32(reader["yearFounded"]),
                AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]),
                InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]),
                InspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]),
                ParkDescription = Convert.ToString(reader["parkDescription"]),
                EntryFee = Convert.ToInt32(reader["entryFee"]),
                NumOfAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]),
            };
        }

        /// <summary>
        /// Gets information on all Surveys from survey_result table in NationalParkForecast DB
        /// </summary>
        /// <returns>Survey List</returns>
        public List<SurveyResultModel> GetSurveyResults()
        {
            List<SurveyResultModel> surveys = new List<SurveyResultModel>();
            const string surveySql = "SELECT Count(survey_result.parkCode) as Ranking, parkName, survey_result.parkCode, park.parkDescription " +
                                     "FROM survey_result JOIN park ON survey_result.parkCode = park.parkCode " +
                                     "GROUP BY survey_result.parkCode, parkName, parkDescription " +
                                     "ORDER BY Count(survey_result.parkCode) DESC;";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(surveySql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        surveys.Add(GetSingleSurvey(reader));
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return surveys;
        }

        /// <summary>
        /// Populates a Surveys object from a SqlDataReader object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Surveys object</returns>
        public SurveyResultModel GetSingleSurvey(SqlDataReader reader)
        {
            return new SurveyResultModel()
            {
                //SurveyId = Convert.ToInt32(reader["surveyId"]),
                ParkCode = Convert.ToString(reader["parkCode"]),
                ParkName = Convert.ToString(reader["parkName"]),
                Ranking = Convert.ToInt32(reader["Ranking"]),
                ParkDescription = Convert.ToString(reader["parkDescription"]),
            };
        }

        public bool SaveSurvey(Surveys survey)
        {
            int rowsAffected = 0;
            const string insertSurveySql = "INSERT INTO survey_result (parkCode, emailAddress, [state], activityLevel) " +
                                           "VALUES(@parkCode, @emailAddress, @state, @activityLevel);";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insertSurveySql, conn);
                    cmd.Parameters.AddWithValue("@parkCode", survey.ParkCode);
                    cmd.Parameters.AddWithValue("@emailAddress", survey.EmailAddress);
                    cmd.Parameters.AddWithValue("@state", survey.State);
                    cmd.Parameters.AddWithValue("@activityLevel", survey.ActivityLevel);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch(Exception)
            {
                throw;
            }
            return (rowsAffected > 0);
        }

        public List<ParkCodeAndNameModel> GetParkNamesAndCodes()
        {
            const string parkSQL = "SELECT parkCode, parkName FROM park;";

            List<ParkCodeAndNameModel> parkNameList = new List<ParkCodeAndNameModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(parkSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ParkCodeAndNameModel parkName = new ParkCodeAndNameModel();
                        parkName.ParkName = Convert.ToString(reader["parkName"]);
                        parkName.ParkCode = Convert.ToString(reader["parkCode"]);
                        parkNameList.Add(parkName);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return parkNameList;
        }

        #endregion
    }
}