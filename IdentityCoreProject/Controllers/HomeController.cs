using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityCoreProject.Models;
using Microsoft.AspNetCore.Hosting;
using IdentityCoreProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IdentityCoreProject.Services;
using Microsoft.Extensions.Logging;
using AutoMapper;
using RazorLight;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IdentityCoreProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebNoteService _webNoteService;

        public HomeController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebNoteService webNoteService)
        {
            _context = context;
            _userManager = userManager;
            _webNoteService = webNoteService;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var sortingOption = _webNoteService.GetUsersSortingOption(userId);
            var notes = _webNoteService.GetUsersNotes(userId, sortingOption);
                return View(notes);
        }

        [HttpGet("getWebNotes")]
        public IActionResult GetWebNotes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var sortingOption = _webNoteService.GetUsersSortingOption(userId);
            var webnotes = _webNoteService.GetUsersNotes(userId, sortingOption);


            return Json(new { notes = webnotes });
        }

        [HttpPost("updateNoteTitle")]
        public IActionResult UpdateNoteTitle(int id, string title)
        {
            _webNoteService.UpdateTitle(id, title);
            return Ok();
        }

        [HttpPost("updateNoteContent")]
        public IActionResult UpdateNoteContent(int id, string content)
        {
            _webNoteService.UpdateContent(id, content);
            return Ok();
        }

        [HttpPost("deleteNote")]
        public IActionResult DeleteNote(int id)
        {
            _webNoteService.DeleteNote(id);
            return Ok();
        }

        [HttpPost("saveNote")]
        public async Task<IActionResult> Save(WebNote webNote)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _webNoteService.CreateNote(webNote, user);
            return Ok();
        }

        [HttpPost("moveNoteUp")]
        public IActionResult MoveNoteUp(int idOfClickedNote)
        {
            _webNoteService.MoveNoteUp(idOfClickedNote);
            return Ok();
        }

        [HttpPost("moveNoteDown")]
        public IActionResult MoveNoteDown(int idOfClickedNote)
        {
            _webNoteService.MoveNoteDown(idOfClickedNote);
            return Ok();
        }

        [HttpPost("sendEmail")]
        public IActionResult SendEmail(string email, WebNote note)
        {
            string loggedInEmail = User.Identity.Name;
            _webNoteService.SendEmail(note, loggedInEmail, email); 
            return Ok();
        }

        [HttpGet("getSingleWebNote")]
        public IActionResult getSingleWebNote(int id)
        {
            WebNote webnote = _context.WebNotes.FirstOrDefault(x => x.Id == id);
            return Json(webnote);
        }

        [HttpPost("groupByPriority")]
        public IActionResult groupByPriority()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var toUpdate = _context.Users.FirstOrDefault(x => x.Id == userId);
            var currentSortingOption = _webNoteService.GetUsersSortingOption(userId);

            if (currentSortingOption == "byPriority")
            {
                toUpdate.WebnoteSortingOption = "byDate";
                _context.Update(toUpdate);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                toUpdate.WebnoteSortingOption = "byPriority";
                _context.Update(toUpdate);
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpGet]
        public FileResult DownloadNotes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var sortingOption = _webNoteService.GetUsersSortingOption(userId);
            var myWebNotes = _webNoteService.GetUsersNotes(userId, sortingOption);
            var stream  = _webNoteService.DownloadNotes(userId, myWebNotes);

            return File(stream, "text/csv", "WebNotes("+ User.Identity.Name +").csv");
        }
    }
}
