using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.Arm;
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
            return _gameInfo.PossibleMoves(GetPlayer()).Select(p => new MoveAction
            {
                Action = p
            }).ToList();
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
                LastMove = lastMove != null ? lastMove with
                    {
                        Player = GetPlayer(),
                        LastMoves = lastMove.Player == GetPlayer()
                        ? lastMove.LastMoves.Append(new (){from, to}).ToList()
                        : new (){new () {from, to}}
                    } : new LastMoveInfo
                {
                    Player = GetPlayer(),
                    LastMoves = new (){new () {from, to}}
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
            return Count_Curr_Move();
        }

        private int Count_Curr_Move()
        {
            var numPawns = 0;
            var numKings = 0;
            var numEdgePawn = 0;
            var numEdgeKing = 0;
            var numDefendedSquare = 0;
            var numTopThree = 0;
            var numCenterKing = 0;
            var numCenterPawn = 0;
            var triangle = 0;
            var bridge = 0;
            var dog = 0;
            var oreo = 0;
            var kingsCorner = 0;

            foreach (var square in _gameInfo.Board)
            {
                var column = square.Column;
                var row = square.Row;

                if (square.King)
                {
                    if (square.Color == GetPlayer())
                    {
                        numKings += 1;
                        if (CentrallyPositioned(row, column))
                        {
                            numCenterKing += 1;
                        }

                        if (AdjacentToTheEdge(row, column))
                        {
                            numEdgeKing += 1;
                        }

                        if (OnLowerTwoPlayers(column, square.Color))
                        {
                            numDefendedSquare += 1;
                        }
                    }
                    else
                    {
                        numKings -= 1;
                        if (CentrallyPositioned(row, column))
                        {
                            numCenterKing -= 1;
                        }

                        if (AdjacentToTheEdge(row, column))
                        {
                            numEdgeKing -= 1;
                        }

                        if (OnLowerTwoPlayers(column, square.Color))
                        {
                            numDefendedSquare -= 1;
                        }
                    }
                }
                else
                {
                    if (square.Color == GetPlayer())
                    {
                        numPawns += 1;
                        if (AdjacentToTheEdge(row, column))
                        {
                            numEdgePawn += 1;
                        }

                        if (OnTopThreePlayers(column, square.Color))
                        {
                            numTopThree += 1;
                        }

                        if (CentrallyPositioned(row, square.Column))
                        {
                            numCenterPawn += 1;
                        }

                        if (OnLowerTwoPlayers(column, square.Color))
                        {
                            numDefendedSquare += 1;
                        }
                    }
                    else
                    {
                        numPawns -= 1;
                        if (AdjacentToTheEdge(row, column))
                        {
                            numEdgePawn -= 1;
                        }

                        if (OnTopThreePlayers(column, square.Color))
                        {
                            numTopThree -= 1;
                        }

                        if (CentrallyPositioned(row, square.Column))
                        {
                            numCenterPawn -= 1;
                        }

                        if (OnLowerTwoPlayers(column, square.Color))
                        {
                            numDefendedSquare -= 1;
                        }
                    }
                }


                if (GetPlayer() == Player.Red)
                {
                    triangle = Convert.ToInt32(Triangle(GetPlayer())) - Convert.ToInt32(Triangle(Player.Black));
                    bridge = Convert.ToInt32(Bridge(GetPlayer())) - Convert.ToInt32(Bridge(Player.Black));
                    dog = Convert.ToInt32(Dog(GetPlayer())) - Convert.ToInt32(Dog(Player.Black));
                    oreo = Convert.ToInt32(Oreo(GetPlayer())) - Convert.ToInt32(Oreo(Player.Black));
                    kingsCorner = Convert.ToInt32(KingInCorner(GetPlayer())) -
                                  Convert.ToInt32(KingInCorner(Player.Black));
                }
                else
                {
                    triangle = Convert.ToInt32(Triangle(Player.Black)) - Convert.ToInt32(Triangle(GetPlayer()));
                    bridge = Convert.ToInt32(Bridge(Player.Black)) - Convert.ToInt32(Bridge(GetPlayer()));
                    dog = Convert.ToInt32(Dog(Player.Black)) - Convert.ToInt32(Dog(GetPlayer()));
                    oreo = Convert.ToInt32(Oreo(Player.Black)) - Convert.ToInt32(Oreo(GetPlayer()));
                    kingsCorner = Convert.ToInt32(KingInCorner(Player.Black)) -
                                  Convert.ToInt32(KingInCorner(GetPlayer()));
                }
            }
            
            return CountHeuristic(numPawns, numKings, numEdgePawn,
                numEdgeKing, numDefendedSquare, numTopThree,
                numCenterKing, numCenterPawn, triangle,
                bridge, dog, oreo, kingsCorner);
        }

            

        private bool AdjacentToTheEdge(int i, int j)
        {
            return i == 0 || j == 0 || i == 7 || j == 7;
        }

        private static bool OnLowerTwoPlayers(int i, Player player)
        {
            if (player == Player.Red)
            {
                if (0 <= i && i <= 1)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (6 <= i && i <= 7)
                {
                    return true;
                }

                return false;
            }
        }
        
        private bool OnTopThreePlayers(int i, Player player)
        {
            if (player == Player.Red)
            {
                if (4 < i && i <= 7)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (0 <= i && i <= 2)
                {
                    return true;
                }

                return false;
            }
        }

        private bool CentrallyPositioned(int i, int j)
        {
            if ((2 <= i && i <= 5) && (2 <= j && j <= 5))
            {
                return true;
            }

            return false;
        }

        private bool OnPositionWhite(int i)
        {
            var square = _gameInfo.GetSquare(i);
            if (square == null)
            {
                return false;
            }

            return square.Color == _gameInfo.WhoseTurn;
        }
        
        private bool OnPositionBlack(int i)
        {
            var square = _gameInfo.GetSquare(i);
            if (square == null)
            {
                return false;
            }

            return square.Color == Player.Black;
        }

        private bool Triangle(Player player)
        {
            if (player == Player.Red)
            {
                return OnPositionWhite(1) && OnPositionWhite(2) && OnPositionWhite(6);
            }

            return OnPositionBlack(27) && OnPositionBlack(31) && OnPositionBlack(32);
        }
        
        private bool Oreo(Player player)
        {
            if (player == Player.Red)
            {
                return OnPositionWhite(2) && OnPositionWhite(3) && OnPositionWhite(7);
            }

            return OnPositionBlack(26) && OnPositionBlack(30) && OnPositionBlack(31);
        }

        private bool Bridge(Player player)
        {
            if (player == Player.Red)
            {
                return OnPositionWhite(1) && OnPositionWhite(3);
            }

            return OnPositionBlack(30) && OnPositionBlack(32);
        }

        private bool Dog(Player player)
        {
            if (player == Player.Red)
            {
                return OnPositionWhite(1) && OnPositionBlack(5);
            }

            return OnPositionWhite(28) && OnPositionBlack(32);
        }

        private bool KingInCorner(Player player)
        {
            if (player == Player.Red)
            {
                var square = _gameInfo.GetSquare(29);
                if (square == null)
                {
                    return false;
                }

                return square.Color == GetPlayer() && square.King;
            }
            else
            {
                var square = _gameInfo.GetSquare(4);
                if (square == null)
                {
                    return false;
                }

                return square.Color != GetPlayer() && square.King;
            }
        }
        
        
        private int CountHeuristic(
            int numPawns, int numKings, int numEdgePawn,
            int numEdgeKing, int numDefendedSquare, int numTopThree,
            int numCenterKing, int numCenterPawn, int triangle,
            int bridge, int dog, int oreo, int kingsCorner
        )
        {
            var res = 0;

            res += 4 * numPawns;
            res += 7 * numKings;
            res += 2 * numEdgePawn;
            res += numEdgeKing;
            res += 2 * numDefendedSquare;
            res += numTopThree;
            res += 2 * numCenterKing;
            res += numCenterPawn;
            res += triangle;
            res += bridge;
            res += dog;
            res += oreo;
            res += kingsCorner;
            
            return res;
        }

        public object Clone()
        {
            return new MapState(_gameInfo with
                {
                Board = _gameInfo.Board.Select(a => a with {}).ToArray(),
                LastMove = _gameInfo.LastMove != null
                    ? _gameInfo.LastMove with
                        {
                        LastMoves = _gameInfo.LastMove != null
                            ? _gameInfo.LastMove.LastMoves.Select(l => l.Select(i => i).ToList()).ToList()
                            : null
                        }
                    : null
                });
        }
    }
}