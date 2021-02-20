using num6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace num6.Data.Repository
{
    public interface IRepository
    {
        Room GetRoom(string id);
        List<Room> GetAllRooms();
        List<Room> GetAllEmptyRooms();
        Room SearchRoomByPlayer(string id);
        void AddINTOWhiteList(string words);
        List<WhiteList> GetWhiteLists();
        List<Room> GetRoomsByTags(string tags);
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        Task SaveChangesAsync();
    }
}
