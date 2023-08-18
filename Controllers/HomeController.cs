using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CountryDetailsSystem.Controllers
{
    public class HomeController : Controller
    {

        public async Task<ActionResult> Dashboard()
        {
        var apiAllCountries = "https://restcountries.com/v3.1/all";
            try
            {
                var apiResponse = await GetApiDataAsync(apiAllCountries);

                JArray countries = JArray.Parse(apiResponse);
                List<string> countryNames = countries.Select(c => c["name"]["common"].ToString()).ToList();
              
                ViewBag.CountryNames = countryNames;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }

        }

        public async Task<ActionResult> CountryView(string selectedCountry)
        {
            if (string.IsNullOrEmpty(selectedCountry))
            {
                return View("Wrong");
            }

            var apiCountry = $"https://restcountries.com/v3.1/name/{selectedCountry}";

            try
            {
                var apiResponse = await GetApiDataAsync(apiCountry);
                var jArray = JArray.Parse(apiResponse);

                List<string> countryNames = jArray.Select(c => c["name"]["common"].ToString()).ToList();
                List<string> countryOfficialNames = jArray.Select(c => c["name"]["official"].ToString()).ToList();
                List<string> flagUrls = jArray.Select(c => c["flags"]["png"].ToString()).ToList();
                List<string> population = jArray.Select(c => c["population"].ToString()).ToList();
                List<string> mapLink = jArray.Select(c => c["maps"]["googleMaps"].ToString()).ToList();




                ViewBag.CountryNames = countryNames;
                ViewBag.CountryOfficialNames = countryOfficialNames;
                ViewBag.CountryFlags = flagUrls;
                ViewBag.Population = population;
                ViewBag.MapLink = mapLink;



                return View();
            }
            catch (Exception)
            {
                return View("Wrong");
            }
        }



        private async Task<string> GetApiDataAsync(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
