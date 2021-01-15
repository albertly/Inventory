using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        class MyClass
        {
            public int MyProperty { get; set; }
            public string MyPropertyString { get; set; }
        }

        [HttpGet("GiveMeName")]
        public string GiveMeYourName()
        {
            return "Dudu";
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            SortedList<string,MyClass> a = new SortedList<string, MyClass>();
            
            a.Add("dfsadf", null);            
            a.Add("dsafasd", new MyClass { MyProperty = 1, MyPropertyString = "ddd" });

            MyClass c = new MyClass
            {
                MyProperty = 1,
                MyPropertyString = "sdfasd"
            };

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
