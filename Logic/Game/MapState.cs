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
            var (from, to) = action.Action;

            var (whoseTurn, winner, board, lastMove) = _gameInfo;

            var newBoard = board.ToList();
            newBoard.Remove(_gameInfo.GetSquare(from));
            newBoard.Add(new Square
            {
                Color = GetPlayer(),
                Position = to,
                King = 
                    (GetPlayer() == Player.Red && to >= 29) ||
                    (GetPlayer() == Player.Black && to <= 4)
            });

            _gameInfo = _gameInfo with
                {
                WhoseTurn = whoseTurn == Player.Black ? Player.Red : Player.Black,
                Winner = (_gameInfo.GetPlayerSquares(Player.Black).Count, _gameInfo.GetPlayerSquares(Player.Red).Count) switch
                {
                    (0, _) => Player.Red,
                    (_, 0) => Player.Black,
                    _ => null
                },
                LastMove = lastMove with
                    {
                        Player = GetPlayer(),
                        LastMoves = lastMove.Player == GetPlayer()
                        ? lastMove.LastMoves.Append(new (){from, to}).ToList()
                        : new (){new () {from, to}}
                    },
                Board = newBoard.ToArray()
                };
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