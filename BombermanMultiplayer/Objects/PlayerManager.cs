using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// PlayerManager - Prototype šablono implementacija
    /// Naudoja esamą žaidėjo objektą kaip šabloną naujų žaidėjų kūrimui
    /// </summary>
    public class PlayerManager
    {
        private Player _playerPrototype;

        public PlayerManager(Player prototype)
        {
            _playerPrototype = prototype;
        }

        /// <summary>
        /// Sukuria žaidėjo shallow kopija (shallow copy)
        /// </summary>
        public Player CreatePlayerShallowCopy()
        {
            return (Player)_playerPrototype.Clone();
        }

        /// <summary>
        /// Sukuria žaidėjo deep kopija (deep copy)
        /// </summary>
        public Player CreatePlayerDeepCopy()
        {
            return (Player)_playerPrototype.DeepClone();
        }

        /// <summary>
        /// Užmezga žaidėjo šabloną (panaudoti jį kaip bazę klonavimuui)
        /// </summary>
        public void SetPrototype(Player prototype)
        {
            _playerPrototype = prototype;
        }

        #region Demonstraciniai Metodai - Atminties Lyginimui

        /// <summary>
        /// Demonstruoja shallow ir deep kopijų skirtumus
        /// </summary>
        public void DemonstrateCloneTypes()
        {
            Console.WriteLine("PROTOTYPE DEMONSTRACIJA\n");

            // Originalus žaidėjas (šablonas)
            Console.WriteLine($"Orginalus žaidėjas:");
            Console.WriteLine($"Objekto adresas: {GetObjectAddress(_playerPrototype)}");
            Console.WriteLine($"BonusSlot adresas: {GetObjectAddress(_playerPrototype.BonusSlot)}");
            Console.WriteLine($"Name: {_playerPrototype.Name}\n");

            // Sallow copy
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

            // Modifikacija
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
            Console.WriteLine("Shallow kopija dalijasi BonusSlot masyvu su originalu!");
            Console.WriteLine("Deep kopija turi atskirą BonusSlot masyvą!");
            Console.WriteLine("Primityvūs lauko (int, byte) visada kopijuojami!\n");
        }

        /// <summary>
        /// Grąžina objekto atminties adresą
        /// </summary>
        private static string GetObjectAddress(object obj)
        {
            if (obj == null) return "null";
            return obj.GetHashCode().ToString("X");
        }

        #endregion
    }
}
