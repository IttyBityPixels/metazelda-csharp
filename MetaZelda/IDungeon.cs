using System;
using System.Collections.Generic;
using System.Text;

namespace MetaZelda
{
    /// <summary>
    /// Represents the spacial layout of a lock-and-key puzzle and contains all
    /// <see cref="Symbol"/>s, <see cref="Room"/>s, and <see cref="Edge"/>s within
    /// the puzzle.
    /// </summary>
    interface IDungeon
    {
        ICollection<Room> GetRooms();
        int RoomCount();
        Room Get(int id);
        void Add(Room room);
        void LinkOneWay(Room room1, Room room2);
        void Link(Room room1, Room room2);
        void LinkOneWay(Room room1, Room room2, Symbol cond);
        void Link(Room room1, Room room2, Symbol cond);
        bool RoomsAreLinked(Room room1, Room room2);
        Room FindStart();
        Room FindBoss();
        Room FindGoal();
        Room FindSwitch();
        Rect2I GetExtentBounds();
    }
}
