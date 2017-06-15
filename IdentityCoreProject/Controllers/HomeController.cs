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

            var notes = _webNoteService.GetUsersNotes(userId);
                return View(notes);
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
        public IActionResult MoveNoteUp(int idOfClickedNote, int idOfAboveNote)
        {
            _webNoteService.MoveNoteUp(idOfClickedNote, idOfAboveNote);
            return Ok();
        }

        [HttpPost("moveNoteDown")]
        public IActionResult MoveNoteDown(int idOfClickedNote, int idOfBelowNote)
        {
            _webNoteService.MoveNoteDown(idOfClickedNote, idOfBelowNote);
            return Ok();
        }

        [HttpPost("sendEmail")]
        public IActionResult SendEmail(string email, int id)
        {
            string loggedInEmail = User.Identity.Name;
            var note = _context.WebNotes.FirstOrDefault(x => x.Id == id);
            _webNoteService.SendEmail(note, loggedInEmail, email, id); 
            return Ok();
        }

        [HttpGet]
        public FileResult DownloadNotes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var myWebNotes = _webNoteService.GetUsersNotes(userId);
            var stream  = _webNoteService.DownloadNotes(userId, myWebNotes);

            return File(stream, "text/csv", "votes.csv");
        }
    }
}
