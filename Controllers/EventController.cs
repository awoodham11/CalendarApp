using CalendarApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace CalendarApp.Controllers
{
    [Route("event")]
    public class EventController : Controller
    {
        private readonly EventData _eventData;

        public EventController(EventData eventData)
        {
            _eventData = eventData;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            List<Event> events = _eventData.GetAllEvents();
            return View(events);
        }


        [HttpGet("viewevents")]
        public IActionResult ViewEvents()
        {
            List<Event> events = _eventData.GetAllEvents();
            return View(events);
        }



        [HttpGet("createevent")]
        public IActionResult CreateEvent()
        {
            return View();
        }
        [HttpPost("createevent")]
        public IActionResult CreateEvent(Event calendarEvent)
        {
            if (ModelState.IsValid)
            {
                _eventData.AddEvent(calendarEvent);
                return RedirectToAction(nameof(CreateEvent));
            }
            return View(calendarEvent);
        }

        [HttpGet("GetEvents")]
        public JsonResult GetEvents()
        {
            var events = _eventData.GetAllEvents();

            var calendarEvents = events.Select(
                e => new
                {
                    id = e.Id,
                    title = e.Title,
                    start = e.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = e.EndTime.ToString("yyyy-MM-ddTHH:mm:ss")

                }
            );

            return new JsonResult(calendarEvents);
        }

        [HttpGet("EventDetails/{id}")]
        public IActionResult EventDetails(int id)
        {
            var calendarEvent = _eventData.GetEventById(id);

            if (calendarEvent == null)
            {
                return NotFound();
            }

            return View(calendarEvent);
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var calendarEvent = _eventData.GetEventById(id);
            if (calendarEvent == null)
            {
                return NotFound();
            }
            return View(calendarEvent);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _eventData.DeleteEvent(id);
            return RedirectToAction("Index"); // Redirect back to the calendar
        }


        [HttpGet("EditEvent/{id}")]
        public IActionResult EditEvent(int id)
        {
            var calendarEvent = _eventData.GetEventById(id); // Fetch the event by ID
            if (calendarEvent == null)
            {
                return NotFound();
            }
            return View(calendarEvent); // Pass the event to the edit view
        }

        [HttpPost("EditEvent/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditEvent(int id, Event updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _eventData.UpdateEvent(updatedEvent); // Update the event in the database
                return RedirectToAction("Index"); // Redirect back to the calendar or list page
            }

            return View(updatedEvent); // Return the same view with validation errors if invalid
        }


        [HttpGet("error")]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}