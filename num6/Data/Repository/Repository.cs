using num6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace num6.Data.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _ctx;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Room GetRoom(string id)
        {
            return _ctx.Rooms
                .FirstOrDefault(r => r.Id == id);
        }

        public Room SearchRoomByPlayer(string id)
        {
            return _ctx.Rooms.First(x => x.Id == id || x.SecondPlayer == id);
        }
        public List<Room> GetAllRooms()
        {
            return _ctx.Rooms.ToList();
        }
        public List<Room> GetAllEmptyRooms()
        {
            return _ctx.Rooms.Where(x => x.Status == false).ToList();
        }
        public void UpdateRoom(Room room)
        {
            _ctx.Rooms.Update(room);
        }

        public void AddRoom(Room room)
        {
            _ctx.Rooms.Add(room);
        }
        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
        public void AddINTOWhiteList(string words)
        {
            var wordsList = new List<WhiteList>();
            words.Split('.').ToList().ForEach(x => wordsList.Add(new WhiteList { Id = x }));
            _ctx.WhiteLists.AddRange(wordsList);
        }
        public List<WhiteList> GetWhiteLists()
        {
            return _ctx.WhiteLists.ToList();
        }
        public List<Room> GetRoomsByTags(string tags)
        {
            if (tags == "")
            {
                return GetAllEmptyRooms();
            }
            List<Room> result = new List<Room>();
            List<Room> rooms = GetAllRooms();
            string[] tag = tags.Split('.');
            
            for(var i = 0; i < rooms.Count; i++)
            {
                var def = tag.Intersect(rooms[i].Tags.Split('.')).ToList();
                if(def.Count == tag.Length && rooms[i].Status == false)
                    result.Add(rooms[i]);
            }
            return result;
        }
    }
}
