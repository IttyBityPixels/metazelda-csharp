using System;
using System.Collections.Generic;
using System.Text;

namespace MetaZelda
{
    /// <summary>
    /// Links two <see cref="Room"/>s.
    /// <para>
    /// The attached <see cref="Symbol"/> is a condition that must be satisfied for the
    /// player to pass from one of the linked Rooms to the other via this Edge. It is
    /// implemented as a <see cref="Symbol"/> rather than a <see cref="Condition"/> to
    /// simplify the interface to clients of the library so that they don't have to handle
    /// the case where multiple Symbols are required to pass through an Edge.
    /// </para>
    /// <para>
    /// An unconditional edge is one that may always be used to go from one of the linked
    /// Rooms to the other.
    /// </para>
    /// </summary>
    public class Edge
    {
        protected int targetRoomId;
        protected Symbol symbol;

        /// <summary>
        /// Creates an unconditional Edge.
        /// </summary>
        /// <param name="targetRoomId">The Room being linked to.</param>
        public Edge(int targetRoomId) : this(targetRoomId, null) { }

        /// <summary>
        /// Creates an Edge that requires a particular Symbol to be collected before
        /// it may be used by the player to travel between the Rooms.
        /// </summary>
        /// <param name="targetRoomId">The Room being linked to.</param>
        /// <param name="symbol">The Symbol that must be obtained.</param>
        public Edge(int targetRoomId, Symbol symbol)
        {
            this.targetRoomId = targetRoomId;
            this.symbol = symbol;
        }
        
        /// <returns>Whether the edge is conditional.</returns>
        public bool HasSymbol()
        {
            return symbol != null;
        }

        // TODO: Is it better to replace these Get and Set methods with a property?
        /// <returns>
        /// The Symbol that must be obtained to pass along this Edge
        /// or null if there are no required symbols. 
        /// </returns>
        public Symbol GetSymbol()
        {
            return symbol;
        }

        /// <param name="symbol">The Symbol that must be obtained to pass along this Edge.</param>
        public void SetSymbol(Symbol symbol)
        {
            this.symbol = symbol;
        }
        
        /// <returns>The id of the Room being linked to.</returns>
        public int GetTargetRoomId()
        {
            return targetRoomId;
        }

        // TODO: override GetHashCode()?
        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Edge e = (Edge)obj;

                return targetRoomId == e.targetRoomId
                    && (symbol == e.symbol || symbol.Equals(e.symbol));
            }
        }
    }
}
