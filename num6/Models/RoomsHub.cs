using Microsoft.AspNetCore.SignalR;
using num6.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace num6.Models
{
    public class RoomsHub : Hub
    {
        private readonly IRepository _repo;
        public RoomsHub(IRepository repo)
        {
            _repo = repo;
        }
        public override async Task OnConnectedAsync()
        {
            List<(string group, string name, string tags)> r1 = new List<(string group, string name, string tags)>();
            List<Room> result = _repo.GetAllEmptyRooms();
            result.ForEach(x => r1.Add((x.Id, x.Name, x.Tags)));
            await Groups.AddToGroupAsync(Context.ConnectionId, "User");
            string room = "";
            r1.ForEach(x => room = String.Join(',', new string[]{ room, $"{x.group}|||{x.name}|||{x.tags}" }));

            List<WhiteList> whiteLists = _repo.GetWhiteLists();
            string whitelist = "";
            whiteLists.ForEach(x => whitelist = String.Join(',', new string[] { whitelist, $"{x.Id}" }));
            await Clients.Caller.SendAsync("LoadInfoOnConnect", string.IsNullOrEmpty(room) ? "" : room.Substring(1), string.IsNullOrEmpty(whitelist) ? "" : whitelist.Substring(1));
            await base.OnConnectedAsync();
        }

        public async Task Search(string data)
        {
            List<(string group, string name, string tags)> r1 = new List<(string group, string name, string tags)>();
            List<Room> rooms = _repo.GetRoomsByTags(data);
            rooms.ForEach(x => r1.Add((x.Id, x.Name, x.Tags)));
            string room = "";
            r1.ForEach(x => room = String.Join(',', new string[] { room, $"{x.group}|||{x.name}|||{x.tags}" }));
            await Clients.Caller.SendAsync("Search", string.IsNullOrEmpty(room) ? "" : room.Substring(1));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Room room = _repo.SearchRoomByPlayer(Context.ConnectionId);
            await Clients.Group(room.Id).SendAsync("EnemyLeave");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CreateGame(string group, string name, string tags)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "User");
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.Group("User").SendAsync("AddGame", group, name, tags);
        }

        public async Task JoinGame(string group)
        {
            Room room = _repo.GetRoom(group);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "User");
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.Group(group).SendAsync("JoinGame", room.Id, room.Name, room.Matrix, room.Tags);
        }

        public async Task MoveOnBoard(string matrix, string game)
        {
            await Clients.Group(game).SendAsync("MoveOnBoard", matrix);
        }

        public async Task UpdateStatuses()
        {
            List<(string group, string name, string tags)> r1 = new List<(string group, string name, string tags)>();
            List<Room> rooms = _repo.GetAllEmptyRooms();
            rooms.ForEach(x => r1.Add((x.Id, x.Name, x.Tags)));
            string room = "";
            r1.ForEach(x => room = String.Join(',', new string[] { room, $"{x.group}|||{x.name}|||{x.tags}" }));
            await Task.Delay(1000);
            await Clients.Group("User").SendAsync("UpdateStatuses", string.IsNullOrEmpty(room) ? "" : room.Substring(1));
        }

        public async Task EndGame(string group, string status)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
            await Clients.Group(group).SendAsync("EndGame", status);
        }
    }
}
