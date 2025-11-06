using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Manages the creation and cloning of <see cref="Player"/> objects using the Prototype design pattern.
    /// </summary>
    public class PlayerManager
    {
        private Player _playerPrototype;

        public PlayerManager(Player prototype)
        {
            _playerPrototype = prototype;
        }

        /// <summary>
        /// Creates a shallow copy of the current player prototype.
        /// </summary>
        /// <returns>A <see cref="Player"/> instance that is a shallow copy of the player prototype.</returns>
        public Player CreatePlayerShallowCopy()
        {
            return (Player)_playerPrototype.Clone();
        }

        /// <summary>
        /// Creates a deep copy of the current player instance.
        /// </summary>
        /// <returns>A new <see cref="Player"/> instance that is a deep copy of the current player.</returns>
        public Player CreatePlayerDeepCopy()
        {
            return (Player)_playerPrototype.DeepClone();
        }

        /// <summary>
        /// Sets the prototype player instance to be used as a template for creating new players.
        /// </summary>
        /// <param name="prototype">The <see cref="Player"/> instance to use as the prototype.  This parameter cannot be <see langword="null"/>.</param>
        public void SetPrototype(Player prototype)
        {
            _playerPrototype = prototype;
        }

        #region Prototype Demonstration

        /// <summary>
        /// Demonstrates the differences between shallow and deep cloning of a <see cref="Player"/> object.
        /// </summary>
        public void DemonstrateCloneTypes()
        {
            Console.WriteLine("Prototipo demonstracija\n");

            // Originalus žaidėjas (šablonas)
            Console.WriteLine($"Orginalus žaidėjas:");
            Console.WriteLine($"Objekto adresas: {GetObjectAddress(_playerPrototype)}");
            Console.WriteLine($"BonusSlot adresas: {GetObjectAddress(_playerPrototype.BonusSlot)}");
            Console.WriteLine($"Name: {_playerPrototype.Name}\n");

            // Shllow copy
            Player shallowCopy = this.CreatePlayerShallowCopy();
            Console.WriteLine($"Shallow kopija:");
            Console.WriteLine($"Objekto adresas: {GetObjectAddress(shallowCopy)}");
            Console.WriteLine($"BonusSlot adresas: {GetObjectAddress(shallowCopy.BonusSlot)}");
            Console.WriteLine($"Yra tas pats BonusSlot? {ReferenceEquals(shallowCopy.BonusSlot, _playerPrototype.BonusSlot)}\n");

            // Deep copy
            Player deepCopy = this.CreatePlayerDeepCopy();
            Console.WriteLine($"Deep kopija:");
            Console.WriteLine($"Objekto adresas: {GetObjectAddress(deepCopy)}");
            Console.WriteLine($"BonusSlot adresas: {GetObjectAddress(deepCopy.BonusSlot)}");
            Console.WriteLine($"Yra tas pats BonusSlot? {ReferenceEquals(deepCopy.BonusSlot, _playerPrototype.BonusSlot)}\n");

            // Modification test
            Console.WriteLine("Modifikacijos bandymas:\n");
            
            _playerPrototype.BonusSlot[0] = BonusType.PowerBomb;
            Console.WriteLine($"Originalus BonusSlot[0]: {_playerPrototype.BonusSlot[0]}");
            Console.WriteLine($"Shallow kopija BonusSlot[0]: {shallowCopy.BonusSlot[0]}");
            Console.WriteLine($"Deep kopija BonusSlot[0]: {deepCopy.BonusSlot[0]}");
            
            _playerPrototype.BombNumb = 10;
            Console.WriteLine($"\nOriginalus BombNumb: {_playerPrototype.BombNumb}");
            Console.WriteLine($"Shallow kopija BombNumb: {shallowCopy.BombNumb}");
            Console.WriteLine($"Deep kopija BombNumb: {deepCopy.BombNumb}");

            Console.WriteLine("\nApibendirinimas:");
            Console.WriteLine("Shallow kopija dalijasi BonusSlot masyvu su originalu");
            Console.WriteLine("Deep kopija turi atskirą BonusSlot masyvą");
        }

        /// <summary>
        /// Retrieves the memory address representation of the specified object as a hexadecimal string.
        /// </summary>
        /// <param name="obj">The object whose memory address representation is to be retrieved. If <paramref name="obj"/> is <see
        /// langword="null"/>, the method returns "null".</param>
        /// <returns>A hexadecimal string representing the memory address of the specified object, or "null" if <paramref
        /// name="obj"/> is <see langword="null"/>.</returns>
        private static string GetObjectAddress(object obj)
        {
            if (obj == null) return "null";
            return obj.GetHashCode().ToString("X");
        }

        #endregion
    }
}
