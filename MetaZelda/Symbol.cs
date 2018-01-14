using System;
using System.Collections.Generic;
using System.Text;

namespace MetaZelda
{
    /// <summary>
    /// Represents a single key or lock within the lock-and-key puzzle.
    /// <para>
    /// Each Symbol has a 'value'. Two Symbols are equivalent if they have the same
    /// 'value'.
    /// </para>
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Symbol map with special meanings.
        /// <para>
        /// Certain items (such as START, GOAL, BOSS) serve no purpose in the puzzle
        /// other than as markers for the client of the library to place special game
        /// objects.
        /// </para>
        /// <para>
        /// The SWITCH_ON and SWITCH_OFF symbols do not appear in rooms, only in
        /// <see cref="Condition"/>s and <see cref="Edge"/>s.
        /// </para>
        /// </summary>
        public const int
            START = -1,
            GOAL = -2,
            BOSS = -3,
            SWITCH_ON = -4,     // used as a condition (lock)
            SWITCH_OFF = -5,    // used as a condition (lock)
            SWITCH = -6;        // used as an item (key) within a room

        protected readonly int value;
        protected readonly String name;

        /// <summary>
        /// Creates a Symbol with the given value.
        /// </summary>
        /// <param name="value">Value to give the Symbol.</param>
        public Symbol(int value)
        {
            this.value = value;

            if (value == START)
                name = "Start";
            else if (value == GOAL)
                name = "Goal";
            else if (value == BOSS)
                name = "Boss";
            else if (value == SWITCH_ON)
                name = "ON";
            else if (value == SWITCH_OFF)
                name = "OFF";
            else if (value == SWITCH)
                name = "SW";
            else if (value >= 0 && value < 26)
                name = Char.ToString((char)((int)'A' + value));
            else
                name = value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return value == ((Symbol)obj).value;
            }
        }

        public static bool Equals(Symbol a, Symbol b)
        {
            if (a == b) return true;
            if (b == null) return a.Equals(b);
            return b.Equals(a);
        }

        public override int GetHashCode()
        {
            return value;
        }
        
        /// <returns>The value of the Symbol.</returns>
        public int GetValue()
        {
            return value;
        }
        
        /// <returns>Whether the Symbol is the special START symbol.</returns>
        public bool IsStart()
        {
            return value == START;
        }
        
        /// <returns>Whether the Symbol is the special GOAL symbol.</returns>
        public bool IsGoal()
        {
            return value == GOAL;
        }
        
        /// <returns>Whether the Symbol is the special BOSS symbol.</returns>
        public bool IsBoss()
        {
            return value == BOSS;
        }
        
        /// <returns>Whether the Symbol is the speciall SWITCH symbol.</returns>
        public bool IsSwitch()
        {
            return value == SWITCH;
        }
        
        /// <returns>Whether the Symbol is one of the special SWITCH_{ON, OFF} symbols.</returns>
        public bool IsSwitchState()
        {
            return value == SWITCH_ON || value == SWITCH_OFF;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
