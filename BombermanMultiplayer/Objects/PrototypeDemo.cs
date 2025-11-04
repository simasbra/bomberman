using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// PrototypeDemo - Demonstracija Prototype ðablono panaudojimo
    /// Naudingà atliekti projektinio darbo ataskaitai
    /// </summary>
    public class PrototypeDemo
    {
        /// <summary>
        /// Paleistina demonstracija Prototype ðablono su atminties adresais
        /// </summary>
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

            // Sukuriame PlayerManager su originalu þaidëju kaip ðablonu
            PlayerManager manager = new PlayerManager(originalPlayer);

            // Demonstruojame skirtumà tarp shallow ir deep kopijø
            manager.DemonstrateCloneTypes();

            // Gràþiname originalaus þaidëjo bonus á default
            originalPlayer.BonusSlot[0] = BonusType.None;
            originalPlayer.BombNumb = 3;

            Console.WriteLine("PRAKTINIS PANAUDOJIMAS");

            // PRAKTINIS PAVYZDYS: Multiplayer sesijos pradþia
            Console.WriteLine("Multiplayer sesijos pradþia\n");

            // Sukuriame 3 þaidëjø kopijas ið prototipo
            Player player1 = (Player)manager.CreatePlayerDeepCopy();
            player1.Name = "Red_Player";
            player1.CasePosition = new int[2] { 1, 1 };

            Player player2 = (Player)manager.CreatePlayerDeepCopy();
            player2.Name = "Blue_Player";
            player2.CasePosition = new int[2] { 1, 15 };

            Player player3 = (Player)manager.CreatePlayerDeepCopy();
            player3.Name = "Green_Player";
            player3.CasePosition = new int[2] { 15, 1 };

            Console.WriteLine($"   Þaidëjas 1 ({player1.Name}):");
            Console.WriteLine($"      - Adresas: {player1.GetHashCode():X}");
            Console.WriteLine($"      - Pozicija: [{player1.CasePosition[0]}, {player1.CasePosition[1]}]");
            Console.WriteLine($"      - Bombø: {player1.BombNumb}\n");

            Console.WriteLine($"   Þaidëjas 2 ({player2.Name}):");
            Console.WriteLine($"      - Adresas: {player2.GetHashCode():X}");
            Console.WriteLine($"      - Pozicija: [{player2.CasePosition[0]}, {player2.CasePosition[1]}]");
            Console.WriteLine($"      - Bombø: {player2.BombNumb}\n");

            Console.WriteLine($"   Þaidëjas 3 ({player3.Name}):");
            Console.WriteLine($"      - Adresas: {player3.GetHashCode():X}");
            Console.WriteLine($"      - Pozicija: [{player3.CasePosition[0]}, {player3.CasePosition[1]}]");
            Console.WriteLine($"      - Bombø: {player3.BombNumb}\n");

            player1.BombNumb = 5;
            player1.Lifes = 2;

            Console.WriteLine("MODIFIKAVÆ player1 (BombNumb = 5, Lifes = 2):\n");
            Console.WriteLine($"   Player1 - BombNumb: {player1.BombNumb}, Lifes: {player1.Lifes}");
            Console.WriteLine($"   Player2 - BombNumb: {player2.BombNumb}, Lifes: {player2.Lifes}");
            Console.WriteLine($"   Player3 - BombNumb: {player3.BombNumb}, Lifes: {player3.Lifes}");
            Console.WriteLine($"   Original - BombNumb: {originalPlayer.BombNumb}, Lifes: {originalPlayer.Lifes}");

            Console.WriteLine("\nIÐVADA - Deep Copy garantuoja objektø nepriklausomybæ!");

            Console.WriteLine("PANAUDOJIMO SCENARIJAI");

            Console.WriteLine("Multiplayer sesijos pradþia:");
            Console.WriteLine("    - Sukuriami þaidëjai ið ðablono su skirtingomis pozicijomis\n");

            Console.WriteLine("Save/Load sistemos:");
            Console.WriteLine("    - Saugomos þaidëjø snapshots (deep kopijos)\n");
        }

        /// <summary>
        /// Demonstracija su atminties adresø iðsave laipsniø
        /// </summary>
        public static void AdvancedMemoryDemo()
        {
            Console.WriteLine("ATMINTIES ADRESØ IÐSAMIOJI DEMONSTRACIJA");

            Player prototype = new Player(3, 4, 48, 48, 1, 1, 48, 48, 125, 1);
            prototype.BonusSlot[0] = BonusType.PowerBomb;

            // SHALLOW COPY
            Player shallow = (Player)prototype.Clone();

            // DEEP COPY
            Player deep = (Player)prototype.DeepClone();

            Console.WriteLine("ATMINTIES ANALIZË:\n");

            Console.WriteLine($"PROTOTYPE:");
            Console.WriteLine($"  Objekto ref:        {GCHandle.Alloc(prototype).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot ref:      {GCHandle.Alloc(prototype.BonusSlot).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot[0]:       {prototype.BonusSlot[0]}\n");

            Console.WriteLine($"SHALLOW COPY:");
            Console.WriteLine($"  Objekto ref:        {GCHandle.Alloc(shallow).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot ref:      {GCHandle.Alloc(shallow.BonusSlot).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot dalijasi? {ReferenceEquals(shallow.BonusSlot, prototype.BonusSlot)}\n");

            Console.WriteLine($"DEEP COPY:");
            Console.WriteLine($"  Objekto ref:        {GCHandle.Alloc(deep).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot ref:      {GCHandle.Alloc(deep.BonusSlot).Target.GetHashCode():X}");
            Console.WriteLine($"  BonusSlot dalijasi? {ReferenceEquals(deep.BonusSlot, prototype.BonusSlot)}\n");
        }
    }
}
