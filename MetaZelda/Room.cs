using System;
using System.Collections.Generic;
using System.Text;

// TODO: reference Vec2I and Vec2ISet from gameutil

namespace MetaZelda
{
    /// <summary>
    /// Represents an individual space within the dungeon.
    /// <para>
    /// A Room contains:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// an item (<see cref="Symbol"/>) that the player may (at their choice)
    /// collect by passing through this Room
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// an intensity, which is a measure of the relative difficulty of the room
    /// and ranges from 0.0 to 1.0
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="Edge"/>s for each door to an adjacent Room
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </summary>
    public class Room
    {
        protected Condition precond;
        public readonly int id;
        protected ISet<Vec2I> coords;
        protected Vec2I center;
        protected Symbol item;
        protected List<Edge> edges;
        protected double intensity;
        protected Room parent;
        protected List<Room> children;

        // TODO: make sure that links in summary work when generating documentation
        /// <summary>
        /// Creates a Room at the given coordinates, with the given parent,
        /// containing a specific item, and having a certain pre-<see cref="Conidition"/>.
        /// <para>
        /// The parent of a room is the parent node of this Room in the initial
        /// tree of the dungeon during <see cref="DungeonGenerator.Generate()"/>, and
        /// before <see cref="DungeonGenerator.Graphify()"/>.
        /// </para>
        /// </summary>
        /// <param name="id">The id of the new room.</param>
        /// <param name="coords">The coordinates of the new room.</param>
        /// <param name="parent">The parent room or null if it is the root / entry room.</param>
        /// <param name="item">The symbol to place in the room or null if no item.</param>
        /// <param name="precond">The precondition of the room.</param>
        /// <seealso cref="Condition"/>
        public Room(int id, ISet<Vec2I> coords, Room parent, Symbol item, Condition precond)
        {
            this.id = id;
            this.coords = coords;
            this.item = item;
            edges = new List<Edge>();
            this.precond = precond;
            intensity = 0.0;
            this.parent = parent;
            children = new List<Room>(3);
            // all edges initially null

            int x = 0, y = 0;

            foreach (Vec2I xy in coords)
            {
                x += xy.x;
                y += xy.y;
            }

            center = new Vec2I(x / coords.Count, y / coords.Count);
        }

        // TODO: verify that the second parameter is functionally equilalent to 'new Vec2ISet(Arrays.asList(coords))' in Java
        public Room(int id, Vec2I coords, Room parent, Symbol item, Condition precond)
            : this(id, new Vec2ISet(new List<Vec2I>(coords)), parent, item, precond)
        {

        }

        // TODO: Is it better to replace these Get and Set methods with a property?
        /// <returns> The intensity of the Room.</returns>
        /// <seealso cref="Room"/>
        public double GetIntensity()
        {
            return intensity;
        }

        /// <param name="intensity">The value to set the Room's intensity to.</param>
        /// <seealso cref="Room"/>
        public void SetIntensity(double intensity)
        {
            this.intensity = intensity;
        }
        
        /// <returns>The item contained in the Room, or null if there is none.</returns>
        public Symbol GetItem()
        {
            return item;
        }
        
        /// <param name="item">The item to place in the Room.</param>
        public void SetItem(Symbol item)
        {
            this.item = item;
        }

        // NOTE: the Java version of this method seems to have the wrong documentation
        // TODO: finish documenting
        /// <summary>
        /// Gets the Edge object... (finish this)
        /// </summary>
        /// <param name="targetRoomId">blank</param>
        /// <returns>blank</returns>
        public Edge GetEdge(int targetRoomId)
        {
            foreach (Edge e in edges)
            {
                if (e.getTargetRoomId() == targetRoomId)
                    return e;
            }

            return null;
        }

        // TODO: add documentation
        public SetEdge(int targetRoomId, Symbol symbol)
        {
            Edge e = GetEdge(targetRoomId);

            if (e != null)
            {
                e.symbol = symbol;
            }
            else
            {
                e = new Edge(targetRoomId, symbol);
                edges.Add(e);
            }

            return e;
        }

        /// <summary>
        /// Gets the number of Rooms this Room is linked to.
        /// </summary>
        /// <returns>The number of links.</returns>
        public int LinkCount()
        {
            return edges.Count;
        }
        
        /// <returns>Whether this room is the entry to the dungeon.</returns>
        public bool IsStart()
        {
            return item != null && item.IsStart();
        }
        
        /// <returns>Whether this room is the goal room of the dungeon.</returns>
        public bool IsGoal()
        {
            return item != null && item.IsGoal();
        }
        
        /// <returns>Whether this room contains the dungeon's boss.</returns>
        public bool IsBoss()
        {
            return item != null && item.IsBoss();
        }
        
        /// <returns>Whether this room contains the dungeon's switch object.</returns>
        public bool IsSwitch()
        {
            return item != null && item.IsSwitch();
        }
        
        /// <returns>The precondition for this Room.</returns>
        /// <seealso cref="Condition"/>
        public Condition GetPrecond()
        {
            return precond;
        }
        
        /// <param name="precond">The precondition to set this Room's to.</param>
        /// <seealso cref="Condition"/>
        public void SetPrecond(Condition precond)
        {
            this.precond = precond;
        }
        
        /// <returns>The parent of this Room.</returns>
        public Room GetParent()
        {
            return parent;
        }
        
        /// <param name="parent">The Room to set this Room's parent to.</param>
        public void SetParent(Room parent)
        {
            this.parent = parent;
        }
        
        /// <returns>The collection of Rooms this Room is a parent of.</returns>
        public ICollection<Room> GetChildren()
        {
            return children;
        }

        /// <summary>
        /// Registers this Room as a parent of another.
        /// Does not modify the child room's parent property.
        /// </summary>
        /// <param name="child">The room to parent.</param>
        public void AddChild(Room child)
        {
            children.Add(child);
        }

        // TODO: add documentation for these three methods
        public ISet<Vec2I> GetCoords()
        {
            return coords;
        }

        public Vec2I GetCenter()
        {
            return center;
        }

        public override String ToString()
        {
            return String.Format("Room({0})", coords.ToString());
        }
    }
}
