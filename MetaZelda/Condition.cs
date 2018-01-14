using System;
using System.Collections.Generic;
using System.Text;

namespace MetaZelda
{
    /// <summary>
    /// Used to represent <see cref="Room"/>s' preconditions.
    /// <para>
    /// A Room's precondition can be considered the set of Symbols from the other
    /// Rooms that the player must have collected to be able to reach this room. For
    /// instance, if the Room is behind a locked door, the precondition for the
    /// Room includes the key for that lock.
    /// </para>
    /// <para>
    /// In practice, since there is always a time ordering on the collection of keys,
    /// this can be implemented as a count of the number of keys the player must have
    /// (the 'keyLevel').
    /// </para>
    /// <para>
    /// The state of the <see cref="Dungeon"/>'s switch is also recorded in the Condition.
    /// A Room behind a link that requires the switch to be flipped into a particular
    /// state will have a precondition that includes the switch's state.
    /// </para>
    /// <para>
    /// A Condition is 'satisfied' when the player has all the keys it requires and
    /// when the dungeon's switch is in the state that it requires.
    /// </para>
    /// <para>
    /// A Condition x implies a Condition y if and only if y is satisfied whenever x is.
    /// </para>
    /// </summary>
    public class Condition
    {
        // NOTE: can't use 'static' keyword here, make sure that SwitchState can be referrenced without instantiating the class
        /// <summary>
        /// A type to represent the required state of a switch for the Condition to
        /// be satisfied.
        /// </summary>
        public enum SwitchState
        {
            /// <summary>
            /// The switch may be in any state.
            /// </summary>
            Either,

            /// <summary>
            /// The switch must be off.
            /// </summary>
            Off,

            /// <summary>
            /// The switch must be on.
            /// </summary>
            On
        };

        public Symbol ToSymbol()
        {

        }
    }
}
