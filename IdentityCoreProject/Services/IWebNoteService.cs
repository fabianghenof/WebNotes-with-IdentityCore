﻿using System.Collections.Generic;
using IdentityCoreProject.Models;

namespace IdentityCoreProject.Services
{
    public interface IWebNoteService
    {
        void CreateNote(WebNote webNote, ApplicationUser user);
        void DeleteNote(int id);
        List<WebNote> GetUsersNotes(string userId);
        void MoveNoteDown(int idOfClickedNote, int idOfBelowNote);
        void MoveNoteUp(int idOfClickedNote, int idOfAboveNote);
        void UpdateContent(int id, string content);
        void UpdateTitle(int id, string title);
    }
}