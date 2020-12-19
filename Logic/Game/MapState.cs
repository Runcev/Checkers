using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Models;
using Shared.Models.Enums;

namespace Logic.Game
{
    public class MapState : ICloneable
    {
        private GameInfoData _gameInfo;
        public MapState(GameInfoData gameInfoData)
        {
            _gameInfo = gameInfoData;
        }
        
        public Player GetPlayer() => _gameInfo.WhoseTurn;

        public bool IsTerminal()
        {
            if (_gameInfo.Winner != null)
            {
                return true;
            }

            return false;
        }
        

        public List<MoveAction> GetActions()
        {
            var moves = new List<MoveAction>
            {
                new MoveAction(MoveActionEnum.ForwardRight),
                new MoveAction(MoveActionEnum.ForwardLeft),
                new MoveAction(MoveActionEnum.BackLeft),
                new MoveAction(MoveActionEnum.BackRight),
            };
            
            return moves;
        }
        
        public void Move(MoveAction action)
        {
            var player = GetPlayer();

            switch (action.Action)
            {
                case MoveActionEnum.ForwardLeft:
                    break;
                case MoveActionEnum.ForwardRight:
                    break;
                case MoveActionEnum.BackLeft:
                    break;
                case MoveActionEnum.BackRight:
                    break;
            }
        }

        public int GetUtility(Player player)
        {
            if (player == Player.Black)
            {
                return Map[player.X][player.Y].Contains(TileState.Skovoroda) ? int.MinValue : int.MaxValue;
            }
            
            return Map[player.X][player.Y].Contains(TileState.World) ? int.MinValue : int.MaxValue;
        }

        public int Eval()
        {
           
        }

        public object Clone()
        {
            var result = (MapState) MemberwiseClone();

            result.Map = new List<TileState>[Map.Length][];

            for (int i = 0; i < Map.Length; i++)
            {
                result.Map[i] = new List<TileState>[Map[i].Length];
                for (int j = 0; j < Map[i].Length; j++)
                {
                    result.Map[i][j] = new List<TileState>();
                    result.Map[i][j].AddRange(Map[i][j]);
                }
            }

            result.Skovoroda = Skovoroda.Clone() as Skovoroda;

            result.Worlds = Worlds.Select(world => world.Clone() as World).ToList();

            result._players = new List<Player> {result.Skovoroda};
            result._players.AddRange(result.Worlds);

            return result;
        }
    }
}