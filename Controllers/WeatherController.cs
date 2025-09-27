using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Web.Script.Serialization;
using FirstTasks.Models;

namespace FirstTasks.Controllers
{
    public class WeatherController : Controller
    {

        // GET: Weather 
        public ActionResult Weather()
        {
            //create view here with template Empty and Model as Empty  
            return View();
        }
        [HttpPost]
        public String WeatherDetail(string City)
        {
            //Assign API KEY which received from OPENWEATHERMAP.ORG 
            string appId = "8113fcc5a7494b0518bd91ef3acc074f";
            //API path with CITY parameter and other parameters. 
            string url =
string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, appId);
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                //Converting to OBJECT from JSON string. 
                RootObject weatherInfo = (new JavaScriptSerializer()).Deserialize<RootObject>(json);
                //Special VIEWMODEL design to send only required fields not all fields which received from
                //www.openweathermap.org api 
                WeatherModel rslt = new WeatherModel();
                rslt.Country = weatherInfo.sys.country;
                rslt.City = weatherInfo.name;
                rslt.Lat = Convert.ToString(weatherInfo.coord.lat);
                rslt.Lon = Convert.ToString(weatherInfo.coord.lon);
                rslt.Description = weatherInfo.weather[0].description;
                rslt.Humidity = Convert.ToString(weatherInfo.main.humidity);
                rslt.Temp = Convert.ToString(weatherInfo.main.temp);
                rslt.TempFeelsLike = Convert.ToString(weatherInfo.main.feels_like);
                rslt.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                rslt.TempMin = Convert.ToString(weatherInfo.main.temp_min);
                rslt.WeatherIcon = weatherInfo.weather[0].icon;
                //Converting OBJECT to JSON String 
                var jsonstring = new JavaScriptSerializer().Serialize(rslt);
                //Return JSON string. 
                return jsonstring;
            }
        }
    }
}