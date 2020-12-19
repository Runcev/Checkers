using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Manager;
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

        public bool IsTerminal() => _gameInfo.Winner != null;

        public List<MoveAction> GetActions()
        {
            return new()
            {
                new() {Action = (9, 13)}
            };
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
            if (player == GetPlayer())
            {
                return GetPlayer() == Player.Red ? int.MaxValue : int.MinValue;
            }
            
            return GetPlayer() == Player.Red ? int.MinValue : int.MaxValue;
        }

        public int Eval()
        {
            return _gameInfo.GetPlayerSquares(GetPlayer()).Sum(s => s.King ? 4 : 1);
        }

        public object Clone()
        {
            return _gameInfo with
                {
                Board = _gameInfo.Board.Select(a => a with {}).ToArray(),
                LastMove = _gameInfo.LastMove with 
                    {
                     LastMoves = _gameInfo.LastMove.LastMoves.Select(l => l.Select(i => i).ToList()).ToList()
                    }
                };
        }
    }
}