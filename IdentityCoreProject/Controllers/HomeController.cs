﻿using System.Linq;
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
        private readonly string sortingOption;

        public HomeController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebNoteService webNoteService,
            string _sortingOption)
        {
            _context = context;
            _userManager = userManager;
            _webNoteService = webNoteService;

            _sortingOption = _webNoteService.GetUsersSortingOption();
            sortingOption = _sortingOption;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var notes = _webNoteService.GetUsersNotes(userId, sortingOption);
                return View(notes);
        }

        [HttpGet("getWebNotes")]
        public IActionResult GetWebNotes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
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
            var notes = _webNoteService.GetUsersNotes(userId, sortingOption);
            return View(notes);
        }

        [HttpGet]
        public FileResult DownloadNotes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var myWebNotes = _webNoteService.GetUsersNotes(userId, sortingOption);
            var stream  = _webNoteService.DownloadNotes(userId, myWebNotes);

            return File(stream, "text/csv", "WebNotes("+ User.Identity.Name +").csv");
        }
    }
}
