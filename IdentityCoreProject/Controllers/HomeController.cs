using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityCoreProject.Models;
using Microsoft.AspNetCore.Hosting;
using IdentityCoreProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;
using IdentityCoreProject.Services;
using Microsoft.Extensions.Logging;

namespace IdentityCoreProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebNoteService _webNoteService;
        private readonly ILogger _logger;


        public HomeController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, IWebNoteService webNoteService, ILogger<HomeController> logger)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _webNoteService = webNoteService;
            _logger = logger;
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
        public IActionResult SendEmail(string email)
        {
            //MAIL TEST
            //var engine = EngineFactory.CreatePhysical(System.IO.Path.Combine(_hostingEnvironment.ContentRootPath, "Templates", "Email"));
            //var model = new
            //{
            //    Name = "John Doe",
            //    Title = "RazorLight"
            //};
            //string result = engine.Parse("basic.cshtml", model);

            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Eu", "NEIMPLEMENTAT"));
            //message.To.Add(new MailboxAddress("Tu", email));
            //message.Subject = email;
            //message.Body = new TextPart("html")
            //{
            //    Text = result
            //};

            //using (var client = new SmtpClient())
            //{
            //    client.Connect("smtp.mailgun.org", 587, false);
            //    // Note: since we don't have an OAuth2 token, disable // the XOAUTH2 authentication mechanism
            //    client.AuthenticationMechanisms.Remove("XOAUTH2");
            //    // Note: only needed if the SMTP server requires authentication
            //    client.Authenticate("NEIMPLEMENTAT", "NEIMPLEMENTAT");

            //    client.Send(message);
            //    client.Disconnect(true);
            //}
            //
            return Ok();
        }

        [HttpGet]
        public FileResult DownloadNotes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var myWebNotes = _webNoteService.GetUsersNotes(userId);

            var trimmedWebNotes = myWebNotes.Select(note => new CsvNotes
            {
                Id = note.Id.ToString(),
                Title = note.Title,
                Content = note.Content
            }).ToList();
   
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var csv = new CsvWriter(writer);
            csv.WriteRecords(trimmedWebNotes);
            writer.Flush();
            stream.Position = 0;

            return File(stream, "text/csv", "votes.csv");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
