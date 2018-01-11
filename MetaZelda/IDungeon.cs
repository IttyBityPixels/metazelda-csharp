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
    public interface IDungeon
    {
        /// <returns>The rooms within the dungeon.</returns>
        ICollection<Room> GetRooms();

        /// <returns>The number of rooms in the dungeon.</returns>
        int RoomCount();

        /// <param name="id">The id of the room.</param>
        /// <returns>The room with the given id.</returns>
        Room Get(int id);

        /// <summary>
        /// Adds a new room to the dungeon, overwriting any rooms already in it
        /// that have the same coordinates.
        /// </summary>
        /// <param name="room">The room to add.</param>
        void Add(Room room);

        /// <summary>
        /// Adds a one-way unconditional edge between the given rooms.
        /// A one-way edge may be used to travel from room1 to room2,
        /// but not room2 to room1.
        /// </summary>
        /// <param name="room1">The first room to link.</param>
        /// <param name="room2">The second room to link.</param>
        void LinkOneWay(Room room1, Room room2);

        /// <summary>
        /// Adds a two-way unconditional edge between the given rooms.
        /// A two-way edge may be used to travel from each room to the other.
        /// </summary>
        /// <param name="room1">The first room to link.</param>
        /// <param name="room2">The second room to link.</param>
        void Link(Room room1, Room room2);

        /// <summary>
        /// Adds a one-way conditional edge between the given rooms.
        /// A one-way edge may be used to travel from room1 to room2,
        /// but not room2 to room1.
        /// </summary>
        /// <param name="room1">The first room to link.</param>
        /// <param name="room2">The second room to link.</param>
        /// <param name="cond">The condition on the edge.</param>
        void LinkOneWay(Room room1, Room room2, Symbol cond);

        /// <summary>
        /// Adds a two-way conditional edge between the given rooms.
        /// A two-way edge may be used to travel from each room to the other.
        /// </summary>
        /// <param name="room1">The first room to link.</param>
        /// <param name="room2">The second room to link.</param>
        /// <param name="cond">The condition on the edge.</param>
        void Link(Room room1, Room room2, Symbol cond);

        /// <summary>
        /// Tests whether two rooms are linked.
        /// Two rooms are linked if there are
        /// any edges(in any direction) between them.
        /// </summary>
        /// <param name="room1">The first room to test.</param>
        /// <param name="room2">The second room to test.</param>
        /// <returns>True if the rooms are linked, false otherwise.</returns>
        bool RoomsAreLinked(Room room1, Room room2);

        /// <returns>The room containing the START symbol.</returns>
        Room FindStart();

        /// <returns>The room containing the BOOS symbol.</returns>
        Room FindBoss();
        
        /// <returns>The room containing the GOAL symbol.</returns>
        Room FindGoal();
        
        /// <returns>The room containing the SWITCH symbol.</returns>
        Room FindSwitch();

        /// <summary>
        /// Gets the <see cref="Rect2I"/> that encloses every room within the dungeon.
        /// <para>
        /// The Bounds object has the X coordinate of the West-most room, the Y
        /// coordinate of the North-most room, the 'right' coordinate of the
        /// East-most room, and the 'bottom' coordinate of the South-most room.
        /// </para>
        /// </summary>
        /// <returns>The rectangle enclosing every room within the dungeon.</returns>
        Rect2I GetExtentBounds();
    }
}
