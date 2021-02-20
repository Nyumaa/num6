using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using num6.Data.Repository;
using num6.Models;
using num6.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace num6.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;

        public HomeController(IRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(string id, string name, string tags, string matrix)
        {
            _repo.AddRoom(new Room { Id = id, Name = name, Tags = tags, Matrix = matrix, Status = false});
            await _repo.SaveChangesAsync();
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> JoinGame(string id, string idPlayer)
        {
            Room room = _repo.GetRoom(id);
            room.Status = true;
            room.SecondPlayer = idPlayer;
            _repo.UpdateRoom(room);
            await _repo.SaveChangesAsync();
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWhiteList(string words)
        {
            _repo.AddINTOWhiteList(words);
            await _repo.SaveChangesAsync();
            return View("Index");
           
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
