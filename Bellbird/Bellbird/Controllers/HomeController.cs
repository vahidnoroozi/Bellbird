using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bellbird.Models;
using Bellbird.DAL.Models;
using Bellbird.Services;

namespace Bellbird.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IBellbirdService _bellbirdService;

        public HomeController(IBellbirdService bellbirdService)
        {
            _bellbirdService = bellbirdService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CreateAlarm()
        {
            ViewData["Message"] = "Create Alarm here.";
          //  var airports = _sampleService.GetAirports();

         //   SignUpViewModel sm = new SignUpViewModel();
         //   sm.Airports = new List<SelectListItem>();
            //foreach (var ap in airports)
            //{
            //    sm.Airports.Add(new SelectListItem { Text = ap.Name, Value = ap.Id.ToString() });
            //}


            return View();
        }

        [HttpPut("Upvote/{Id}")]
        public async Task<IActionResult> UpVote(int id)
        {
            await _bellbirdService.UpVoteAsync(id);
            return Ok(true);
        }


        [HttpGet("Alarms")]
        public async Task<IActionResult> GetAlarms()
        {
            List<Alarm> result = await _bellbirdService.GetAlarms();
            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAlarm(CreateAlarmViewModel alarm)
        {
            try
            {
                Alarm newAlarm = new Alarm
                {
                    Name = alarm.Name.ToUpper(),
                    CreatedDate = DateTime.Now,
                    Upvotes = 0
                };
                var alarmcreated = await _bellbirdService.CreateAsync(newAlarm);
                var postTask = _bellbirdService.PostAlarm(alarmcreated.Id);
                await postTask;
                return Ok(true);
            }
            catch (Exception e)
            {
                ViewData["ErrorMsg"] = e.Message;
                return View("Error");
            }

            return View("Thanks");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
