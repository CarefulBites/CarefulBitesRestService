using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System;
using CarefulBitesAPI.Models;
using System.Collections.Generic;

namespace CarefulBitesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MealController : Controller
    {
        private readonly MealsManager _manager = new MealsManager();
        // GET: MealController
        [HttpGet]
        public ActionResult GetMeals([FromQuery] string ingredient)
        {
            var meals = _manager.GetFood(ingredient);

            if (meals != null && meals.Any())
                return Ok(meals);

            return NoContent();
        }

        [HttpGet("foodById")]
        public ActionResult GetMealsById([FromQuery] string id)
        {
            var mealsById = _manager.GetFoodById(id);
            if (mealsById?.IdMeal != null)
            {
                return Ok(mealsById);
            }
            return NoContent();

        }
        [HttpGet("RandomMeals")]  // Prettry sure it has a bug or 2 but might work
        public ActionResult GetRandomMeals([FromQuery] string s_count)
        {
            int.TryParse(s_count, out int count);
            if (count < 0) count = 1;
            List<string> ingreds = _manager.GetRandomIngreds(count+1);

            Random rand = new Random();
            //making a loop that runs till succefull data is found eqrul to count
            //
            int test = 0;
            int succes_entrys = 0;
            List < TempMeal > result = new List<TempMeal>();
            while (test>= count)
            {
               if (succes_entrys > count) break;
               
               List<TempMeal>? meals = _manager.GetFood(ingreds[test]);
               test++;
               if (meals != null)
               {
                    succes_entrys++;
                    int index = (int)rand.Next(meals.Count);
                    result.Add(meals[index]);
               }
               
            }

            if (succes_entrys>= count)
            {
                return Ok(result);
            }
            return NoContent();

        }

    }
}
