using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

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
        public List<Player> players = new List<Player>();

        public List<Bomb> BombsOnTheMap;
        public System.Timers.Timer LogicTimer;

        //ctor when picture box size is determined
        public Game(int hostWidth, int hostHeight)
        {
            this.world = new World(hostWidth, hostHeight, 48, 48, 1);

            players.Add(new Player(1, 2, 33, 33, 1, 1, 48, 48, 80, 1));
            players.Add(new Player(1, 2, 33, 33, this.world.MapGrid.GetLength(0) - 2, this.world.MapGrid.GetLength(0) - 2, 48, 48, 80, 2));
            players.Add(new Player(1, 2, 33, 33, 1, this.world.MapGrid.GetLength(0) - 2, 48, 48, 80, 3));
            players.Add(new Player(1, 2, 33, 33, this.world.MapGrid.GetLength(0) - 2, 1, 48, 48, 80, 4));

            this.BombsOnTheMap = new List<Bomb>();
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;
        }

        //ctor when loading a game
        public Game(int hostWidth, int hostHeight, SaveGameData save)
        {
            this.world = new World(hostWidth, hostHeight, save.MapGrid);

            players = save.players;

            this.BombsOnTheMap = save.bombsOnTheMap;
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;
        }
        //default ctor
        public Game()
        {
            this.world = new World();
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;
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
            //12 Deactivate and empty 
            //13 Armor and empty 

            //30 PowerBomb and Fire 
            //31 SpeedBoost and Fire 
            //32 Deactivate and Fire 
            //33 Armor and Fire 

            for (int i = 0; i < world.MapGrid.GetLength(0); i++) //Row
            {
                for (int j = 0; j < world.MapGrid.GetLength(1); j++) //Column
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
                            case Objects.BonusType.Deactivate:
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

        public void Game_KeyDown(Keys key)
        {
            // Player 1
            if (!players[0].Dead)
            {
                switch (key)
                {
                    case Keys.Z:
                        players[0].Orientation = Player.MovementDirection.UP;
                        players[0].LoadSprite(Properties.Resources.AT_UP);
                        break;
                    case Keys.S:
                        players[0].Orientation = Player.MovementDirection.DOWN;
                        players[0].LoadSprite(Properties.Resources.AT_DOWN);
                        break;
                    case Keys.Q:
                        players[0].Orientation = Player.MovementDirection.LEFT;
                        players[0].LoadSprite(Properties.Resources.AT_LEFT);
                        break;
                    case Keys.D:
                        players[0].Orientation = Player.MovementDirection.RIGHT;
                        players[0].LoadSprite(Properties.Resources.AT_RIGHT);
                        break;
                    case Keys.Space:
                        players[0].DropBomb(this.world.MapGrid, this.BombsOnTheMap, players);
                        break;
                    case Keys.A:
                        players[0].Deactivate(this.world.MapGrid, BombsOnTheMap, players);
                        break;
                }
            }

            // Player 2
            if (!players[1].Dead)
            {
                switch (key)
                {
                    case Keys.Up:
                        players[1].Orientation = Player.MovementDirection.UP;
                        players[1].LoadSprite(Properties.Resources.T_UP);
                        break;
                    case Keys.Down:
                        players[1].Orientation = Player.MovementDirection.DOWN;
                        players[1].LoadSprite(Properties.Resources.T_DOWN);
                        break;
                    case Keys.Left:
                        players[1].Orientation = Player.MovementDirection.LEFT;
                        players[1].LoadSprite(Properties.Resources.T_LEFT);
                        break;
                    case Keys.Right:
                        players[1].Orientation = Player.MovementDirection.RIGHT;
                        players[1].LoadSprite(Properties.Resources.T_RIGHT);
                        break;
                    case Keys.ControlKey:
                        players[1].DropBomb(this.world.MapGrid, this.BombsOnTheMap, players);
                        break;
                    case Keys.Shift:
                        players[1].Deactivate(this.world.MapGrid, BombsOnTheMap, players);
                        break;
                }
            }

            // Player 3
            if (players.Count > 2 && !players[2].Dead)
            {
                switch (key)
                {
                    case Keys.I:
                        players[2].Orientation = Player.MovementDirection.UP;
                        players[2].LoadSprite(Properties.Resources.T_UP);
                        break;
                    case Keys.K:
                        players[2].Orientation = Player.MovementDirection.DOWN;
                        players[2].LoadSprite(Properties.Resources.T_DOWN);
                        break;
                    case Keys.J:
                        players[2].Orientation = Player.MovementDirection.LEFT;
                        players[2].LoadSprite(Properties.Resources.T_LEFT);
                        break;
                    case Keys.L:
                        players[2].Orientation = Player.MovementDirection.RIGHT;
                        players[2].LoadSprite(Properties.Resources.T_RIGHT);
                        break;
                    case Keys.NumPad0:
                        players[2].DropBomb(this.world.MapGrid, this.BombsOnTheMap, players);
                        break;
                    case Keys.NumPad1:
                        players[2].Deactivate(this.world.MapGrid, BombsOnTheMap, players);
                        break;
                }
            }

            // Player 4
            if (players.Count > 3 && !players[3].Dead)
            {
                switch (key)
                {
                    case Keys.T:
                        players[3].Orientation = Player.MovementDirection.UP;
                        players[3].LoadSprite(Properties.Resources.T_UP);
                        break;
                    case Keys.G:
                        players[3].Orientation = Player.MovementDirection.DOWN;
                        players[3].LoadSprite(Properties.Resources.T_DOWN);
                        break;
                    case Keys.F:
                        players[3].Orientation = Player.MovementDirection.LEFT;
                        players[3].LoadSprite(Properties.Resources.T_LEFT);
                        break;
                    case Keys.H:
                        players[3].Orientation = Player.MovementDirection.RIGHT;
                        players[3].LoadSprite(Properties.Resources.T_RIGHT);
                        break;
                    case Keys.PageUp:
                        players[3].DropBomb(this.world.MapGrid, this.BombsOnTheMap, players);
                        break;
                    case Keys.PageDown:
                        players[3].Deactivate(this.world.MapGrid, BombsOnTheMap, players);
                        break;
                }
            }

            if (key == Keys.Escape)
            {
                Pause();
            }
        }

        //Manage key push for server side
        public void Game_KeyDownWithoutSprite(Keys key, Sender Station)
        {
            Player sender = players[(int)Station - 1];

            if (sender.Dead)
                return;

            switch (key)
            {
                case Keys.Up:
                    sender.Orientation = Player.MovementDirection.UP;
                    break;
                case Keys.Down:
                    sender.Orientation = Player.MovementDirection.DOWN;
                    break;
                case Keys.Left:
                    sender.Orientation = Player.MovementDirection.LEFT;
                    break;
                case Keys.Right:
                    sender.Orientation = Player.MovementDirection.RIGHT;
                    break;
                case Keys.Space:
                    sender.DropBomb(this.world.MapGrid, this.BombsOnTheMap, players);
                    break;
                case Keys.ControlKey:
                    sender.Deactivate(this.world.MapGrid, BombsOnTheMap, players);
                    break;
                case Keys.Escape:
                    Pause();
                    break;
            }
        }

        //Manage release of key for server side
        public void Game_KeyUpWithoutSprite(Keys key, Sender Station)
        {
            Player sender = players[(int)Station - 1];

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

        public void Game_KeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.Z:
                case Keys.S:
                case Keys.Q:
                case Keys.D:
                    players[0].Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    players[1].Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.I:
                case Keys.K:
                case Keys.J:
                case Keys.L:
                    if (players.Count > 2)
                        players[2].Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.T:
                case Keys.G:
                case Keys.F:
                case Keys.H:
                    if (players.Count > 3)
                        players[3].Orientation = Player.MovementDirection.NONE;
                    break;
            }
        }

        private void GameOver()
        {
            int deadPlayers = 0;
            Player alivePlayer = null;
            foreach (Player player in players)
            {
                if (player.Dead)
                {
                    deadPlayers++;
                }
                else
                {
                    alivePlayer = player;
                }
            }

            if (deadPlayers >= 3)
            {
                this.Over = true;
                this.Paused = true;
                if (deadPlayers == 4)
                {
                    Winner = 0; // Draw
                }
                else
                {
                    Winner = (byte)alivePlayer.PlayerNumber;
                }
            }
        }

        //Manage interactions between worlds and objects
        private void InteractionLogic()
        {
            for (int i = 0; i < world.MapGrid.GetLength(0); i++) //Row
            {
                for (int j = 0; j < world.MapGrid.GetLength(1); j++) //Column
                {

                    if (world.MapGrid[i, j].Fire)
                    {
                        foreach (Player player in players)
                        {
                            if (player.CasePosition[0] == i && player.CasePosition[1] == j
                                && player.BonusSlot[0] != Objects.BonusType.Armor && player.BonusSlot[1] != Objects.BonusType.Armor)
                            {
                                player.Dead = true;
                                player.LoadSprite(Properties.Resources.Blood);
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

            //Check for bomb explosion
            for (int i = 0; i < BombsOnTheMap.Count; i++)
            {
                BombsOnTheMap[i].UpdateFrame((int)LogicTimer.Interval);
                BombsOnTheMap[i].TimingExplosion((int)LogicTimer.Interval);
                if (BombsOnTheMap[i].Explosing == true)
                {
                    BombsOnTheMap[i].Explosion(this.world.MapGrid, players);
                    ToRemove.Add(i);
                }
            }

            //Remove exploded bombs
            for (int i = 0; i < ToRemove.Count; i++)
            {
                try
                {
                    BombsOnTheMap.RemoveAt(ToRemove[i]);
                }
                catch (Exception){}
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
                        if (player.BonusSlot[i] == Objects.BonusType.SpeedBoost)
                        {
                            player.Speed /= 2;
                        }

                        player.BonusSlot[i] = Objects.BonusType.None;
                    }
                    else
                    {
                        player.BonusTimer[i] -= (short)LogicTimer.Interval;
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
                    switch (this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere.Type)
                    {
                        case Objects.BonusType.PowerBomb:
                            player.BonusSlot[freeSlot] = Objects.BonusType.PowerBomb;
                            player.BonusTimer[freeSlot] = 5000;
                            break;
                        case Objects.BonusType.SpeedBoost:
                            player.BonusSlot[freeSlot] = Objects.BonusType.SpeedBoost;
                            player.Speed *= 2;
                            player.BonusTimer[freeSlot] = 5000;
                            break;
                        case Objects.BonusType.Deactivate:
                            player.BonusSlot[freeSlot] = Objects.BonusType.Deactivate;
                            player.BonusTimer[freeSlot] = 10000;
                            break;
                        case Objects.BonusType.Armor:
                            player.BonusSlot[freeSlot] = Objects.BonusType.Armor;
                            player.BonusTimer[freeSlot] = 5000;
                            break;
                        default:
                            break;
                    }
                    this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere = null;
                }
            }
        }

        private void PlayersLogic()
        {
            foreach (Player player in players)
            {
                player.LocationCheck(48, 48);
                BonusLogic(player);

                if (player.Orientation != Player.MovementDirection.NONE)
                {
                    if (CheckCollisionPlayer(player, players, world.MapGrid, player.Orientation))
                    {
                        player.Move();
                    }
                    player.UpdateFrame((int)LogicTimer.Interval);
                }
                else
                    player.frameindex = 1;
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
        public bool CheckCollisionPlayer(Player movingPlayer, List<Player> otherPlayers, Tile[,] map, Player.MovementDirection direction)
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
                        Rectangle rect = new Rectangle(movingPlayer.Source.X, movingPlayer.Source.Y - movingPlayer.Speed, movingPlayer.Source.Width, movingPlayer.Source.Height);

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
                        foreach (Player otherPlayer in otherPlayers)
                        {
                            if (movingPlayer != otherPlayer && CheckCollisionRectangle(rect, otherPlayer.Source))
                                return false;
                        }
                    }
                    break;
                case Player.MovementDirection.DOWN:
                    {
                        //DOWN
                        Rectangle rect = new Rectangle(movingPlayer.Source.X, movingPlayer.Source.Y + movingPlayer.Speed, movingPlayer.Source.Width, movingPlayer.Source.Height);

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
                        foreach (Player otherPlayer in otherPlayers)
                        {
                            if (movingPlayer != otherPlayer && CheckCollisionRectangle(rect, otherPlayer.Source))
                                return false;
                        }
                    }
                    break;
                case Player.MovementDirection.LEFT:
                    {
                        //LEFT
                        Rectangle rect = new Rectangle(movingPlayer.Source.X - movingPlayer.Speed, movingPlayer.Source.Y, movingPlayer.Source.Width, movingPlayer.Source.Height);
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
                        foreach (Player otherPlayer in otherPlayers)
                        {
                            if (movingPlayer != otherPlayer && CheckCollisionRectangle(rect, otherPlayer.Source))
                                return false;
                        }
                    }
                    break;
                case Player.MovementDirection.RIGHT:
                    {
                        Rectangle rect = new Rectangle(movingPlayer.Source.X + movingPlayer.Speed, movingPlayer.Source.Y, movingPlayer.Source.Width, movingPlayer.Source.Height);
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
                        foreach (Player otherPlayer in otherPlayers)
                        {
                            if (movingPlayer != otherPlayer && CheckCollisionRectangle(rect, otherPlayer.Source))
                                return false;
                        }
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
        private void LogicTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            InteractionLogic();
            PlayersLogic();
            BombsLogic();
            GameOver();
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