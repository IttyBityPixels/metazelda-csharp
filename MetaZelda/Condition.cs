using System;
using System.Collections.Generic;
using System.Text;

namespace MetaZelda
{
    public static class SwitchStateFunctions
    {
        /// <summary>
        /// Convert this SwitchState to a <see cref="Symbol"/>.
        /// </summary>
        /// <returns>A symbol representing the required state of the switch or
        /// null if the switch may be in any state.</returns>
        public static Symbol ToSymbol(this SwitchState switchState)
        {
            switch (switchState)
            {
                case SwitchState.OFF:
                    return new Symbol(Symbol.SWITCH_OFF);
                case SwitchState.ON:
                    return new Symbol(Symbol.SWITCH_ON);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Invert the required state of the switch.
        /// </summary>
        /// <returns>A SwitchState with the opposite required swtch state or
        /// this SwitchState if now particular state is required.</returns>
        public static SwitchState Invert(this SwitchState switchState)
        {
            switch (switchState)
            {
                case SwitchState.OFF: return SwitchState.ON;
                case SwitchState.ON: return SwitchState.OFF;
                default:
                    return switchState;
            }
        }
    }

    /// <summary>
    /// A type to represent the required state of a switch for the Condition to
    /// be satisfied.
    /// </summary>
    public enum SwitchState
    {
        /// <summary>
        /// The switch may be in any state.
        /// </summary>
        EITHER,

        /// <summary>
        /// The switch must be off.
        /// </summary>
        OFF,

        /// <summary>
        /// The switch must be on.
        /// </summary>
        ON
    };

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
        protected int keyLevel;
        protected SwitchState switchState;

        /// <summary>
        /// Create a Condition that is always satisfied.
        /// </summary>
        public Condition()
        {
            keyLevel = 0;
            switchState = SwitchState.EITHER;
        }

        /// <summary>
        /// Creates a Condition that requires the player to have a particular <see cref="Symbol"/>.
        /// </summary>
        /// <param name="e">The symbol that the player must have for the Condition to be satisfied.</param>
        public Condition(Symbol e)
        {
            if (e.GetValue() == Symbol.SWITCH_OFF)
            {
                keyLevel = 0;
                switchState = SwitchState.OFF;
            }
            else if (e.GetValue() == Symbol.SWITCH_ON)
            {
                keyLevel = 0;
                switchState = SwitchState.ON;
            }
            else
            {
                keyLevel = e.GetValue() + 1;
                switchState = SwitchState.EITHER;
            }
        }

        /// <summary>
        /// Creates a Condition from another Condition (copy it).
        /// </summary>
        /// <param name="other">The other Condition.</param>
        public Condition(Condition other)
        {
            keyLevel = other.keyLevel;
            switchState = other.switchState;
        }

        /// <summary>
        /// Creates a Condition that requires the switch to be in a particuar state.
        /// </summary>
        /// <param name="switchState">The required state for the switch to be in.</param>
        public Condition(SwitchState switchState)
        {
            keyLevel = 0;
            this.switchState = switchState;
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
                Condition c = (Condition)obj;

                return keyLevel == c.keyLevel && switchState == c.switchState;
            }
        }

        private void Add(Symbol sym)
        {
            if (sym.GetValue() == Symbol.SWITCH_OFF)
                switchState = SwitchState.OFF;
            else if (sym.GetValue() == Symbol.SWITCH_ON)
                switchState = SwitchState.ON;
            else
                keyLevel = Math.Max(keyLevel, sym.GetValue() + 1);
        }

        private void Add(Condition cond)
        {
            if (switchState == SwitchState.EITHER)
                switchState = cond.switchState;

            keyLevel = Math.Max(keyLevel, cond.keyLevel);
        }

        /// <summary>
        /// Creates a new Condition that requires this Condition to be satisfied
        /// and requires another <see cref="Symbol"/> to be obtained as well.
        /// </summary>
        /// <param name="sym">The added symbol the player must have for the new Condition to be satisfied.</param>
        /// <returns>The new Condition.</returns>
        public Condition And(Symbol sym)
        {
            Condition result = new Condition(this);
            result.Add(sym);
            return result;
        }

        /// <summary>
        /// Creates a new Condition that requires this Condition and another
        /// Condition to both be satisfied.
        /// </summary>
        /// <param name="other">The other Condition that must be satisfied.</param>
        /// <returns>The new Condition.</returns>
        public Condition And(Condition other)
        {
            if (other == null)
                return this;

            Condition result = new Condition(this);
            result.Add(other);

            return result;
        }

        /// <summary>
        /// Determines whether another Condition is necessarily true if this one is.
        /// </summary>
        /// <param name="other">The other Condition.</param>
        /// <returns>Whether the other Condition is implied by this one.</returns>
        public bool Implies(Condition other)
        {
            return keyLevel >= other.keyLevel &&
                (switchState == other.switchState ||
                other.switchState == SwitchState.EITHER);
        }

        /// <summary>
        /// Determines whether this Condition implies that a particular
        /// <see cref="Symbol"/> has been obtained.
        /// </summary>
        /// <param name="s">The Symbol.</param>
        /// <returns>Whether the Symbol is implied by this Condition.</returns>
        public bool Implies(Symbol s)
        {
            return Implies(new Condition(s));
        }

        public Symbol SingleSymbolDifference(Condition other)
        {
            // If the difference between this and other can be made up by obtaining
            // a single new symbol, this returns the symbol. If multiple or no
            // symbols are required, returns null.

            if (this.Equals(other))
                return null;

            if (switchState == other.switchState)
            {
                return new Symbol(Math.Max(keyLevel, other.keyLevel) - 1);
            }
            else
            {
                if (keyLevel != other.keyLevel)
                    return null;

                if (switchState != SwitchState.EITHER && other.switchState != SwitchState.EITHER)
                    return null;

                SwitchState nonEither = switchState != SwitchState.EITHER ? switchState : other.switchState;

                return new Symbol(nonEither == SwitchState.ON
                    ? Symbol.SWITCH_ON
                    : Symbol.SWITCH_OFF);
            }
        }

        public override string ToString()
        {
            string result = "";

            if (keyLevel != 0)
                result += new Symbol(keyLevel - 1).ToString();

            if (switchState != SwitchState.EITHER)
            {
                if (!result.Equals(""))
                    result += ",";

                result += switchState.ToString().ToString();
            }

            return result;
        }

        /// <summary>
        /// Get the number of keys that need to have been obtained for this Condition to be satisfied.
        /// </summary>
        /// <returns>The number of keys.</returns>
        public int GetKeyLevel()
        {
            return keyLevel;
        }

        /// <summary>
        /// Get the state the switch is required to be in for this Condition to be satisfied.
        /// </summary>
        /// <returns>The required switch state.</returns>
        public SwitchState GetSwitchState()
        {
            return switchState;
        }
    }
}
