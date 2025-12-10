using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Provides a demonstration of object cloning techniques and their practical applications, including the creation
    /// of player objects, deep and shallow copy comparisons, and usage scenarios such as multiplayer sessions and
    /// save/load systems.
    /// </summary>
    public class PrototypeDemo
    {
        /// <summary>
        /// Demonstrates the creation and usage of player objects, including cloning techniques and their practical
        /// applications.
        /// </summary>
        /// <remarks></remarks>
        public static void RunDemo()
        {
            Player originalPlayer = new Player(
                lifes: 3,
                totalFrames: 4,
                frameWidth: 48,
                frameHeight: 48,
                caseligne: 1,
                casecolonne: 1,
                TileWidth: 48,
                TileHeight: 48,
                frameTime: 125,
                playerNumero: 1
            );

            originalPlayer.Name = "Player_Original";
            originalPlayer.Vitesse = 5;
            originalPlayer.BombNumb = 3;
            originalPlayer.BonusSlot[0] = BonusType.None;

            // Create PlayerManager with the original player as prototype
            PlayerManager manager = new PlayerManager(originalPlayer);

            // Demonstrate cloning types
            manager.DemonstrateCloneTypes();
        }

        /// <summary>
        /// Demonstracija su atminties adresø iðsave laipsniø
        /// </summary>
        public static void AdvancedMemoryDemo()
        {
            Console.WriteLine("Atminties adresø iðsamus rodymas");

            Player prototype = new Player(3, 4, 48, 48, 1, 1, 48, 48, 125, 1);
            prototype.BonusSlot[0] = BonusType.PowerBomb;

            // SHALLOW COPY
            Player shallow = (Player)prototype.Clone();

            // DEEP COPY
            Player deep = (Player)prototype.DeepClone();

            Console.WriteLine("Atminties analizë:\n");

            Console.WriteLine($"Prototipas:");
            Console.WriteLine($"  Objekto ref:        {GCHandle.Alloc(prototype).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot ref:      {GCHandle.Alloc(prototype.BonusSlot).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot[0]:       {prototype.BonusSlot[0]}\n");

            Console.WriteLine($"Shallow kopija:");
            Console.WriteLine($"  Objekto ref:        {GCHandle.Alloc(shallow).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot ref:      {GCHandle.Alloc(shallow.BonusSlot).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot dalijasi? {ReferenceEquals(shallow.BonusSlot, prototype.BonusSlot)}\n");

            Console.WriteLine($"Deep Kopija:");
            Console.WriteLine($"  Objekto ref:        {GCHandle.Alloc(deep).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot ref:      {GCHandle.Alloc(deep.BonusSlot).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot dalijasi? {ReferenceEquals(deep.BonusSlot, prototype.BonusSlot)}\n");
        }
    }
}
