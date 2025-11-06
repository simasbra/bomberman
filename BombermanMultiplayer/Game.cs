using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using BombermanMultiplayer.Commands;
using BombermanMultiplayer.Commands.Interface;
using BombermanMultiplayer.Strategy;
using BombermanMultiplayer.Strategy.Interface.BombermanMultiplayer.Objects;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Game components 
    /// </summary>
    public class Game
    {
        public bool Paused = false;
        public bool Over = false;
        public byte Winner = 0;

        public World world;
        public Player[] players;

        public List<Bomb> BombsOnTheMap;
        public List<Mine> MinesOnTheMap;
        public List<Grenade> GrenadesOnTheMap;

        public System.Timers.Timer LogicTimer;

        // Command pattern - komandų istorija (replay sistemai ar debugging'ui)
        public List<ICommand> CommandHistory = new List<ICommand>();
        private const int tileWidth = 48;
        private const int tileHeight = 48;

        // Observer pattern - GameState for notifications
        private GameState gameState;
        private bool[] previousDeathStates = new bool[4]; // Track previous death states to detect changes

        //ctor when picture box size is determined
        public Game(int hebergeurWidth, int hebergeurHeight)
        {
            this.world = new World(hebergeurWidth, hebergeurHeight, tileWidth, tileHeight, 1);

            players = new Player[4];
            players[0] = new Player(1, 2, 33, 33, 1, 1, tileWidth, tileHeight, 80, 1);
            players[1] = new Player(1, 2, 33, 33, world.MapGrid.GetLength(0) - 2, world.MapGrid.GetLength(0) - 2, tileWidth, tileHeight, 80, 2);
            players[2] = new Player(1, 2, 33, 33, 1, world.MapGrid.GetLength(1) - 2, tileWidth, tileHeight, 80, 3);
            players[3] = new Player(1, 2, 33, 33, world.MapGrid.GetLength(0) - 2, 1, tileWidth, tileHeight, 80, 4);

            this.BombsOnTheMap = new List<Bomb>();
            this.MinesOnTheMap = new List<Mine>();
            this.GrenadesOnTheMap = new List<Grenade>();
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;

            // Initialize death state tracking
            for (int i = 0; i < 4; i++)
            {
                previousDeathStates[i] = false;
            }
            int rows = world.MapGrid.GetLength(0);
            int cols = world.MapGrid.GetLength(1);

            // Top-right corner
            world.MapGrid[1, cols - 2].Walkable = true;
            world.MapGrid[1, cols - 2].Destroyable = false;

            // Bottom-left corner
            world.MapGrid[rows - 2, 1].Walkable = true;
            world.MapGrid[rows - 2, 1].Destroyable = false;
        }

        //ctor when loading a game
        public Game(int hebergeurWidth, int hebergeurHeight, SaveGameData save)
        {
            this.world = new World(hebergeurWidth, hebergeurHeight, save.MapGrid);

            // Inicializuojam žaidėjų masyvą iš save struktūros
            this.players = new Player[4];
            for (int i = 0; i < 4; i++)
                this.players[i] = save.players[i];

            this.BombsOnTheMap = save.bombsOnTheMap;
            this.MinesOnTheMap = new List<Mine>();
            this.GrenadesOnTheMap = new List<Grenade>();
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;

            // Initialize death state tracking
            for (int i = 0; i < 4; i++)
            {
                previousDeathStates[i] = players[i].Dead;
            }
        }

        //default ctor
        public Game()
        {
            this.world = new World();
            this.players = new Player[4];
            this.BombsOnTheMap = new List<Bomb>();
            this.MinesOnTheMap = new List<Mine>();
            this.GrenadesOnTheMap = new List<Grenade>();
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;

            // Initialize death state tracking
            for (int i = 0; i < 4; i++)
            {
                previousDeathStates[i] = false;
            }
        }

        /// <summary>
        /// Vykdyti komandą ir išsaugoti į istoriją
        /// </summary>
        private void ExecuteCommand(ICommand command)
        {
            command.Execute();
            CommandHistory.Add(command);

            // Riboti istorijos dydį (pvz. paskutiniai 1000 veiksmų)
            if (CommandHistory.Count > 1000)
            {
                CommandHistory.RemoveAt(0);
            }
        }

        //Use a mask to represente the placement of different objects on the map
        public Byte[,] BuildMapMask()
        {
            Byte[,] mask = new Byte[world.MapGrid.GetLength(0), world.MapGrid.GetLength(1)];

            //0 = free tile
            //1 hard block
            //2 destructible block
            //3 fire

            //Bonus
            //10 PowerBomb and empty 
            //11 SpeedBoost and empty 
            //12 Desamorce and empty 
            //13 Armor and empty 

            //30 PowerBomb and Fire 
            //31 SpeedBoost and Fire 
            //32 Desamorce and Fire 
            //33 Armor and Fire 

            for (int i = 0; i < world.MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < world.MapGrid.GetLength(1); j++) //Colonne
                {
                    if (world.MapGrid[i, j].BonusHere != null)
                    {
                        switch (world.MapGrid[i, j].BonusHere.Type)
                        {
                            case Objects.BonusType.PowerBomb:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 30;
                                else mask[i, j] = 10;
                                break;
                            case Objects.BonusType.SpeedBoost:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 31;
                                else mask[i, j] = 11;
                                break;
                            case Objects.BonusType.Desamorce:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 32;
                                else mask[i, j] = 12;
                                break;
                            case Objects.BonusType.Armor:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 33;
                                else mask[i, j] = 13;
                                break;
                            case Objects.BonusType.None:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 3;
                                else mask[i, j] = 0;
                                break;
                        }
                    }
                    else
                    {
                        if (world.MapGrid[i, j].Walkable && !world.MapGrid[i, j].Fire)
                        {
                            mask[i, j] = 0;
                        }
                        else if (!world.MapGrid[i, j].Walkable && !world.MapGrid[i, j].Destroyable)
                        {
                            mask[i, j] = 1;
                        }
                        else if (!world.MapGrid[i, j].Walkable && world.MapGrid[i, j].Destroyable)
                        {
                            mask[i, j] = 2;
                        }
                        else if (world.MapGrid[i, j].Walkable && world.MapGrid[i, j].Fire)
                        {
                            mask[i, j] = 3;
                        }
                    }
                }
            }
            return mask;
        }

        /// <summary>
        /// Atnaujintas Game_KeyDown su Command pattern
        /// </summary>
        public void Game_KeyDown(Keys key)
        {
            ICommand command = null;

            // Žaidėjas 1 - WASD
            if (key == Keys.W)
            {
                if (!players[0].Dead)
                {
                    command = new MovePlayerCommand(players[0], Player.MovementDirection.UP, Properties.Resources.AT_UP);
                }
            }
            else if (key == Keys.S)
            {
                if (!players[0].Dead)
                {
                    command = new MovePlayerCommand(players[0], Player.MovementDirection.DOWN, Properties.Resources.AT_DOWN);
                }
            }
            else if (key == Keys.A)
            {
                if (!players[0].Dead)
                {
                    command = new MovePlayerCommand(players[0], Player.MovementDirection.LEFT, Properties.Resources.AT_LEFT);
                }
            }
            else if (key == Keys.D)
            {
                if (!players[0].Dead)
                {
                    command = new MovePlayerCommand(players[0], Player.MovementDirection.RIGHT, Properties.Resources.AT_RIGHT);
                }
            }
            else if (key == Keys.Space)
            {
                if (!players[0].Dead)
                {
                    command = new DropBombCommand(players[0], world.MapGrid, BombsOnTheMap, players[1]);
                }
            }
            else if (key == Keys.X)
            {
                if (!players[0].Dead)
                {
                    command = new DropMineCommand(players[0], world.MapGrid, MinesOnTheMap, players[1]);
                }
            }
            else if (key == Keys.Z)
            {
                if (!players[0].Dead)
                {
                    command = new DropGrenadeCommand(players[0], world.MapGrid, GrenadesOnTheMap, players[1]);
                }
            }

            // Žaidėjas 2 - Rodyklės
            else if (key == Keys.Up)
            {
                if (!players[1].Dead)
                {
                    command = new MovePlayerCommand(players[1], Player.MovementDirection.UP, Properties.Resources.T_UP);
                }
            }
            else if (key == Keys.Down)
            {
                if (!players[1].Dead)
                {
                    command = new MovePlayerCommand(players[1], Player.MovementDirection.DOWN, Properties.Resources.T_DOWN);
                }
            }
            else if (key == Keys.Left)
            {
                if (!players[1].Dead)
                {
                    command = new MovePlayerCommand(players[1], Player.MovementDirection.LEFT, Properties.Resources.T_LEFT);
                }
            }
            else if (key == Keys.Right)
            {
                if (!players[1].Dead)
                {
                    command = new MovePlayerCommand(players[1], Player.MovementDirection.RIGHT, Properties.Resources.T_RIGHT);
                }
            }
            else if (key == Keys.Enter)
            {
                if (!players[1].Dead)
                {
                    command = new DropBombCommand(players[1], world.MapGrid, BombsOnTheMap, players[0]);
                }
            }
            else if (key == Keys.M)
            {
                if (!players[1].Dead)
                {
                    command = new DropMineCommand(players[1], world.MapGrid, MinesOnTheMap, players[0]);
                }
            }
            else if (key == Keys.G)
            {
                if (!players[1].Dead)
                {
                    command = new DropGrenadeCommand(players[1], world.MapGrid, GrenadesOnTheMap, players[0]);
                }
            }

            // Žaidėjas 3 - IJKL
            else if (key == Keys.I)
            {
                if (!players[2].Dead)
                {
                    command = new MovePlayerCommand(players[2], Player.MovementDirection.UP, Properties.Resources.AT_UP);
                }
            }
            else if (key == Keys.K)
            {
                if (!players[2].Dead)
                {
                    command = new MovePlayerCommand(players[2], Player.MovementDirection.DOWN, Properties.Resources.AT_DOWN);
                }
            }
            else if (key == Keys.J)
            {
                if (!players[2].Dead)
                {
                    command = new MovePlayerCommand(players[2], Player.MovementDirection.LEFT, Properties.Resources.AT_LEFT);
                }
            }
            else if (key == Keys.L)
            {
                if (!players[2].Dead)
                {
                    command = new MovePlayerCommand(players[2], Player.MovementDirection.RIGHT, Properties.Resources.AT_RIGHT);
                }
            }
            else if (key == Keys.U)
            {
                if (!players[2].Dead)
                {
                    command = new DropBombCommand(players[2], world.MapGrid, BombsOnTheMap, players[3]);
                }
            }
            else if (key == Keys.OemOpenBrackets) // [ - Drop Mine
            {
                if (!players[2].Dead)
                {
                    command = new DropMineCommand(players[2], world.MapGrid, MinesOnTheMap, players[3]);
                }
            }
            else if (key == Keys.O)
            {
                if (!players[2].Dead)
                {
                    command = new DropGrenadeCommand(players[2], world.MapGrid, GrenadesOnTheMap, players[3]);
                }
            }

            // Žaidėjas 4 - NumPad
            else if (key == Keys.NumPad8)
            {
                if (!players[3].Dead)
                {
                    command = new MovePlayerCommand(players[3], Player.MovementDirection.UP, Properties.Resources.T_UP);
                }
            }
            else if (key == Keys.NumPad5)
            {
                if (!players[3].Dead)
                {
                    command = new MovePlayerCommand(players[3], Player.MovementDirection.DOWN, Properties.Resources.T_DOWN);
                }
            }
            else if (key == Keys.NumPad4)
            {
                if (!players[3].Dead)
                {
                    command = new MovePlayerCommand(players[3], Player.MovementDirection.LEFT, Properties.Resources.T_LEFT);
                }
            }
            else if (key == Keys.NumPad6)
            {
                if (!players[3].Dead)
                {
                    command = new MovePlayerCommand(players[3], Player.MovementDirection.RIGHT, Properties.Resources.T_RIGHT);
                }
            }
            else if (key == Keys.NumPad0)
            {
                if (!players[3].Dead)
                {
                    command = new DropBombCommand(players[3], world.MapGrid, BombsOnTheMap, players[2]);
                }
            }
            else if (key == Keys.Subtract) // NumPad - Drop Mine
            {
                if (!players[3].Dead)
                {
                    command = new DropMineCommand(players[3], world.MapGrid, MinesOnTheMap, players[2]);
                }
            }
            else if (key == Keys.Decimal) // NumPad . - Drop Grenade
            {
                if (!players[3].Dead)
                {
                    command = new DropGrenadeCommand(players[3], world.MapGrid, GrenadesOnTheMap, players[2]);
                }
            }

            else if (key == Keys.Escape)
            {
                Pause();
                return;
            }

            // Vykdyti komandą jei buvo sukurta
            if (command != null)
            {
                ExecuteCommand(command);
            }
        }

        //Manage key push for server side
        public void Game_KeyDownWithoutSprite(Keys key, Sender station)
        {
            ICommand command = null;

            // Surandam žaidėją pagal Sender
            int senderIndex = -1;
            switch (station)
            {
                case Sender.Player1: senderIndex = 0; break;
                case Sender.Player2: senderIndex = 1; break;
                case Sender.Player3: senderIndex = 2; break;
                case Sender.Player4: senderIndex = 3; break;
                default: return;
            }

            Player sender = players[senderIndex];
            if (sender.Dead) return;

            int otherIndex = (senderIndex + 1) % players.Length;
            Player otherPlayer = players[otherIndex];

            switch (key)
            {
                case Keys.Up:
                    command = new MovePlayerCommand(sender, Player.MovementDirection.UP);
                    break;
                case Keys.Down:
                    command = new MovePlayerCommand(sender, Player.MovementDirection.DOWN);
                    break;
                case Keys.Left:
                    command = new MovePlayerCommand(sender, Player.MovementDirection.LEFT);
                    break;
                case Keys.Right:
                    command = new MovePlayerCommand(sender, Player.MovementDirection.RIGHT);
                    break;
                case Keys.Space:
                    command = new DropBombCommand(sender, world.MapGrid, BombsOnTheMap, otherPlayer);
                    break;
                case Keys.M:
                    command = new DropMineCommand(sender, world.MapGrid, MinesOnTheMap, otherPlayer);
                    break;
                case Keys.G:
                    command = new DropGrenadeCommand(sender, world.MapGrid, GrenadesOnTheMap, otherPlayer);
                    break;
                case Keys.ControlKey:
                    sender.Deactivate(this.world.MapGrid, BombsOnTheMap, otherPlayer);
                    break;
                case Keys.Escape:
                    Pause();
                    break;
            }

            if (command != null)
            {
                ExecuteCommand(command);
            }
        }

        //Manage release of key for server side
        public void Game_KeyUpWithoutSprite(Keys key, Sender Station)
        {
            Player sender = null;

            switch (Station)
            {
                case Sender.Player1:
                    sender = players[0];
                    break;
                case Sender.Player2:
                    sender = players[1];
                    break;
                case Sender.Player3:
                    sender = players[2];
                    break;
                case Sender.Player4:
                    sender = players[3];
                    break;
                default:
                    break;
            }
            switch (key)
            {
                case Keys.Up:
                    sender.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Down:
                    sender.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Left:
                    sender.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Right:
                    sender.Orientation = Player.MovementDirection.NONE;
                    break;
            }
        }

        //Manage the release of the keys
        public void Game_KeyUp(Keys key)
        {
            // Žaidėjas 1: WASD
            if (key == Keys.W || key == Keys.S || key == Keys.A || key == Keys.D)
                players[0].Orientation = Player.MovementDirection.NONE;
            // Žaidėjas 2: Rodyklės
            else if (key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right)
                players[1].Orientation = Player.MovementDirection.NONE;
            // Žaidėjas 3: IJKL
            else if (key == Keys.I || key == Keys.K || key == Keys.J || key == Keys.L)
                players[2].Orientation = Player.MovementDirection.NONE;
            // Žaidėjas 4: NumPad
            else if (key == Keys.NumPad8 || key == Keys.NumPad5 || key == Keys.NumPad4 || key == Keys.NumPad6)
                players[3].Orientation = Player.MovementDirection.NONE;
        }

        private void GameOver()
        {
            int deadCount = 0;
            int lastAlive = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].Dead)
                    deadCount++;
                else
                    lastAlive = i;
            }

            if (deadCount >= players.Length - 1)
            {
                this.Over = true;
                this.Paused = true;
                if (deadCount == players.Length)
                    Winner = 0; // Lygiosios
                else
                    Winner = (byte)(lastAlive + 1); // Laimėtojo indeksas +1
            }
        }

        //Manage interactions between worlds and objects
        private void InteractionLogic()
        {
            for (int i = 0; i < world.MapGrid.GetLength(0); i++) // Row
            {
                for (int j = 0; j < world.MapGrid.GetLength(1); j++) // Column
                {
                    if (world.MapGrid[i, j].Fire)
                    {
                        for (int p = 0; p < players.Length; p++)
                        {
                            if (players[p].CasePosition[0] == i && players[p].CasePosition[1] == j
                                && players[p].BonusSlot[0] != Objects.BonusType.Armor && players[p].BonusSlot[1] != Objects.BonusType.Armor)
                            {
                                players[p].Dead = true;
                                players[p].LoadSprite(Properties.Resources.Blood);
                            }
                        }
                        if (world.MapGrid[i, j].FireTime <= 0)
                        {
                            world.MapGrid[i, j].Fire = false;
                            world.MapGrid[i, j].FireTime = 500;
                        }
                        else
                        {
                            world.MapGrid[i, j].FireTime -= (int)LogicTimer.Interval;
                        }
                    }
                }
            }
        }

        private void BombsLogic()
        {
            List<int> ToRemove = new List<int>();
            for (int i = 0; i < BombsOnTheMap.Count; i++)
            {
                BombsOnTheMap[i].UpdateFrame((int)LogicTimer.Interval);
                BombsOnTheMap[i].TimingExplosion((int)LogicTimer.Interval);
                if (BombsOnTheMap[i].Exploding == true)
                {
                    // Paduok visus žaidėjus
                    BombsOnTheMap[i].Explosion(world.MapGrid, players);
                    ToRemove.Add(i);
                }
            }
            for (int i = ToRemove.Count - 1; i >= 0; i--)
            {
                try { BombsOnTheMap.RemoveAt(ToRemove[i]); } catch (Exception) { }
            }
        }
        private void MinesLogic()
        {
            if (MinesOnTheMap == null || MinesOnTheMap.Count == 0)
                return;

            List<int> ToRemove = new List<int>();
            for (int i = 0; i < MinesOnTheMap.Count; i++)
            {
                MinesOnTheMap[i].CheckProximity(players);
                MinesOnTheMap[i].UpdateFrame((int)LogicTimer.Interval);
                MinesOnTheMap[i].TimingExplosion((int)LogicTimer.Interval);
                if (MinesOnTheMap[i].Exploding == true)
                {
                    MinesOnTheMap[i].Explosion(world.MapGrid, players);
                    ToRemove.Add(i);
                }
            }
            for (int i = ToRemove.Count - 1; i >= 0; i--)
            {
                try { MinesOnTheMap.RemoveAt(ToRemove[i]); } catch (Exception) { }
            }
        }

        private void GrenadesLogic()
        {
            if (GrenadesOnTheMap == null || GrenadesOnTheMap.Count == 0)
                return;

            List<int> ToRemove = new List<int>();
            for (int i = 0; i < GrenadesOnTheMap.Count; i++)
            {
                // Move grenade if it's being thrown
                GrenadesOnTheMap[i].MoveGrenade(world.MapGrid);
                
                GrenadesOnTheMap[i].UpdateFrame((int)LogicTimer.Interval);
                GrenadesOnTheMap[i].TimingExplosion((int)LogicTimer.Interval);
                if (GrenadesOnTheMap[i].Exploding == true)
                {
                    GrenadesOnTheMap[i].Explosion(world.MapGrid, players);
                    ToRemove.Add(i);
                }
            }
            for (int i = ToRemove.Count - 1; i >= 0; i--)
            {
                try { GrenadesOnTheMap.RemoveAt(ToRemove[i]); } catch (Exception) { }
            }
        }

        private void BonusLogic(Player player)
        {
            int freeSlot = -1;
            for (int i = 0; i < player.BonusSlot.Length; i++)
            {
                if (player.BonusSlot[i] != Objects.BonusType.None)
                {
                    if (player.BonusTimer[i] <= 0)
                    {
                        if (player.ActiveStrategies[i] != null)
                        {
                            player.ActiveStrategies[i].Remove(player, i);
                            player.ActiveStrategies[i] = null;
                        }
                    }
                    else
                    {
                        player.BonusTimer[i] -= (short)LogicTimer.Interval;
                        System.Diagnostics.Debug.WriteLine($"Player BonusTimer[{i}] = {player.BonusTimer[i]}ms");
                    }
                }
                else
                {
                    freeSlot = i;
                }
            }

            if (this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere != null)
            {
                //If Player already have the bonus
                if (player.BonusSlot[0] == this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere.Type ||
                    player.BonusSlot[1] == this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere.Type)
                    return;

                if (freeSlot != -1)
                {
                    var bonusType = this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere.Type;

                    IBonusEffectStrategy strategy = null;

                    switch (bonusType)
                    {
                        case Objects.BonusType.PowerBomb:
                            strategy = new PowerBombEffectStrategy();
                            break;
                        case Objects.BonusType.SpeedBoost:
                            strategy = new SpeedBoostEffectStrategy();
                            break;
                        case Objects.BonusType.Desamorce:
                            strategy = new DefuseBombEffectStrategy();
                            break;
                        case Objects.BonusType.Armor:
                            strategy = new ArmorEffectStrategy();
                            break;
                        default:
                            break;
                    }

                    if (strategy != null)
                    {
                        strategy.Apply(player, freeSlot, this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere);
                        player.ActiveStrategies[freeSlot] = strategy;
                    }

                    this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere = null;
                }
            }
        }
        private void PlayersLogic()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].LocationCheck(48, 48);
                BonusLogic(players[i]);
            }

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].Orientation != Player.MovementDirection.NONE)
                {
                    bool canMove = true;
                    for (int j = 0; j < players.Length; j++)
                    {
                        if (i != j && !CheckCollisionPlayer(players[i], players[j], world.MapGrid, players[i].Orientation))
                        {
                            canMove = false;
                            break;
                        }
                    }
                    if (canMove) players[i].Move();
                    players[i].UpdateFrame((int)LogicTimer.Interval);
                }
                else
                    players[i].frameindex = 1;
            }
        }

        //Collision management

        //Check collision between two rectangles
        public bool CheckCollisionRectangle(Rectangle Object1, Rectangle Object2)
        {

            if ((Object2.X >= Object1.X + Object1.Width)      // trop à droite
                || (Object2.X + Object2.Width <= Object1.X) // trop à gauche
                || (Object2.Y >= Object1.Y + Object1.Height) // trop en bas
                || (Object2.Y + Object2.Height <= Object1.Y))  // trop en haut
                return false;
            else
                return true;
            //True if there's a collision

        }

        public bool CheckCollisionPlayer(Player movingPlayer, Player player2, Tile[,] map, Player.MovementDirection direction)
        {
            int lig = movingPlayer.CasePosition[0];
            int col = movingPlayer.CasePosition[1];

            //Check for environnement collision (map)
            switch (direction)
            {
                case Player.MovementDirection.UP:
                    {
                        //UP
                        //Temporary version of player collision box with expected position after deplacement
                        Rectangle rect = new Rectangle(movingPlayer.Source.X, movingPlayer.Source.Y - movingPlayer.Vitesse, movingPlayer.Source.Width, movingPlayer.Source.Height);

                        if (!map[lig - 1, col - 1].Walkable || map[lig - 1, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col - 1].Source))
                                return false;
                        }
                        if (!map[lig - 1, col].Walkable || map[lig - 1, col].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col].Source))
                                return false;
                        }
                        if (!map[lig - 1, col + 1].Walkable || map[lig - 1, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col + 1].Source))
                                return false;
                        }
                        if (CheckCollisionRectangle(rect, player2.Source))
                            return false;
                    }
                    break;
                case Player.MovementDirection.DOWN:
                    {
                        //DOWN
                        Rectangle rect = new Rectangle(movingPlayer.Source.X, movingPlayer.Source.Y + movingPlayer.Vitesse, movingPlayer.Source.Width, movingPlayer.Source.Height);

                        if (!map[lig + 1, col - 1].Walkable || map[lig + 1, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col - 1].Source))
                                return false;
                        }
                        if (!map[lig + 1, col].Walkable || map[lig + 1, col].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col].Source))
                                return false;
                        }
                        if (!map[lig + 1, col + 1].Walkable || map[lig + 1, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col + 1].Source))
                                return false;
                        }
                        if (CheckCollisionRectangle(rect, player2.Source))
                            return false;
                    }
                    break;
                case Player.MovementDirection.LEFT:
                    {
                        //LEFT
                        Rectangle rect = new Rectangle(movingPlayer.Source.X - movingPlayer.Vitesse, movingPlayer.Source.Y, movingPlayer.Source.Width, movingPlayer.Source.Height);
                        if (!map[lig - 1, col - 1].Walkable || map[lig - 1, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col - 1].Source))
                                return false;
                        }
                        if (!map[lig, col - 1].Walkable || map[lig, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig, col - 1].Source))
                                return false;
                        }
                        if (!map[lig + 1, col - 1].Walkable || map[lig + 1, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col - 1].Source))
                                return false;
                        }
                        if (CheckCollisionRectangle(rect, player2.Source))
                            return false;
                    }
                    break;
                case Player.MovementDirection.RIGHT:
                    {
                        Rectangle rect = new Rectangle(movingPlayer.Source.X + movingPlayer.Vitesse, movingPlayer.Source.Y, movingPlayer.Source.Width, movingPlayer.Source.Height);
                        //RIGHT
                        if (!map[lig - 1, col + 1].Walkable || map[lig - 1, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col + 1].Source))
                                return false;
                        }
                        if (!map[lig, col + 1].Walkable || map[lig, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig, col + 1].Source))
                                return false;
                        }
                        if (!map[lig + 1, col + 1].Walkable || map[lig + 1, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col + 1].Source))
                                return false;
                        }
                        if (CheckCollisionRectangle(rect, player2.Source))
                            return false;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        private void LogicTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Save current death states before processing
            for (int i = 0; i < players.Length; i++)
            {
                previousDeathStates[i] = players[i].Dead;
            }

            InteractionLogic();
            PlayersLogic();
            BombsLogic();
            MinesLogic();
            GrenadesLogic();
            GameOver();

            // Check if any player just died (after all logic processing)
            CheckForPlayerDeath();
        }

        /// <summary>
        /// Check if any player just died and trigger auto-save if so
        /// </summary>
        private void CheckForPlayerDeath()
        {
            if (gameState == null) return;

            // Check if any player just died (was alive, now dead)
            for (int i = 0; i < players.Length; i++)
            {
                if (!previousDeathStates[i] && players[i].Dead)
                {
                    // Player just died - significant change, trigger auto-save
                    UpdateGameState();
                    break; // Only need to save once per frame, even if multiple deaths
                }
            }
        }

        /// <summary>
        /// Set the GameState instance for Observer pattern
        /// </summary>
        /// <param name="state">The GameState instance</param>
        public void SetGameState(GameState state)
        {
            this.gameState = state;
        }

        /// <summary>
        /// Update GameState and notify observers
        /// </summary>
        private void UpdateGameState()
        {
            if (gameState != null)
            {
                SaveGameData saveData = new SaveGameData(BombsOnTheMap, world.MapGrid, players);
                gameState.SetState(saveData);
            }
        }

        public void SaveGame(string fileName)
        {
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            System.IO.FileStream filestream = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            try
            {
                formatter.Serialize(filestream, new SaveGameData(BombsOnTheMap, world.MapGrid, players));
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured : " + ex.Message);
                return;
            }
            MessageBox.Show("File " + fileName + " saved successfuly !");
        }

        public void LoadGame(string fileName)
        {
            SaveGameData save;

            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            System.IO.FileStream filestream = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            try
            {
                save = (SaveGameData)formatter.Deserialize(filestream);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured : " + ex.Message);
                return;
            }
            this.BombsOnTheMap = save.bombsOnTheMap;
            this.world.MapGrid = save.MapGrid;
            this.players = save.players;

            this.Paused = true;
            this.LogicTimer.Stop();

            if (this.Over)
            {
                this.Over = false;
                this.Winner = 0;
            }
        }

        public void Pause()
        {
            //If the game is already over, no need for pause
            if (!Over)
            {
                if (Paused)
                {
                    LogicTimer.Start();
                    Paused = false;
                }
                else
                {
                    LogicTimer.Stop();
                    Paused = true;
                }
            }
        }
    }
}