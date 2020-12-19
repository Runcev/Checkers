using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Models;
using Shared.Models.Enums;

namespace Logic.Manager
{
    public static class BoardManager
    {
        public static Square GetSquare(this GameInfoData gameInfoData, int position)
        {
            return gameInfoData.Board.FirstOrDefault(square => square.Position == position);
        }

        public static List<Square> GetPlayerSquares(this GameInfoData gameInfoData, Player player)
        {
            return gameInfoData.Board.Where(square => square.Color == player).ToList();
        }

        private static readonly Dictionary<int, List<int>> RedNotKingCanMove = new();
        private static readonly Dictionary<int, List<int>> BlackNotKingCanMove = new();
        private static readonly Dictionary<int, List<int>> KingCanMove = new();

        private static readonly Dictionary<(int, int), int> CaptureMove = new()
        {
            [(1, 6)] = 10,
            [(2, 6)] = 9,
            [(2, 7)] = 11,
            [(3, 7)] = 10,
            [(3, 8)] = 12,
            [(4, 8)] = 11,
            [(5, 9)] = 14,
            [(6, 9)] = 13,
            [(6, 10)] = 15,
            [(7, 10)] = 14,
            [(7, 11)] = 16,
            [(8, 11)] = 15,
            [(9, 14)] = 18,
            // ...
            [(12, 8)] = 3,
            [(12, 16)] = 19,
            [(13, 9)] = 6,
            [(13, 17)] = 22,
            [(16, 11)] = 7,
            [(16, 19)] = 23,
            [(17, 14)] = 10,
            [(17, 22)] = 26,
            [(20, 16)] = 11,
            [(20, 24)] = 27,
            [(21, 17)] = 14,
            [(21, 25)] = 30,
            [(24, 19)] = 15,
            [(24, 27)] = 31,
            [(25, 22)] = 18,
            [(26, 22)] = 17,
            [(26, 23)] = 19,
            [(27, 23)] = 18,
            [(27, 24)] = 20,
            [(28, 24)] = 19,
            [(29, 25)] = 22,
            [(30, 26)] = 23,
            [(30, 25)] = 21,
            [(31, 26)] = 22,
            [(31, 27)] = 24,
            [(32, 27)] = 23,
        };

        static BoardManager()
        {
            for (int r = 0; r <= 7; r++)
            {
                for (int pos = r * 4 + 1; pos <= (r + 1) * 4; pos++)
                {
                    if (pos == 4)
                    {
                        RedNotKingCanMove[pos] = new() {pos + 4};
                        KingCanMove[pos] = new() {pos + 4};
                    }
                    else if (pos == 29)
                    {
                        BlackNotKingCanMove[pos] = new() {pos - 4};
                        KingCanMove[pos] = new() {pos - 4};
                    }
                    else if (pos > 29)
                    {
                        BlackNotKingCanMove[pos] = new() {pos - 4, pos - 5};
                        KingCanMove[pos] = new() {pos - 5, pos - 4};
                    }
                    else if (pos < 4)
                    {
                        RedNotKingCanMove[pos] = new() {pos + 4, pos + 5};
                        KingCanMove[pos] = new() {pos + 4, pos + 5};
                    } else if (pos == 5 || pos == 13 || pos == 21)
                    {
                        RedNotKingCanMove[pos] = new() {pos + 4};
                        BlackNotKingCanMove[pos] = new() {pos - 4};
                        KingCanMove[pos] = new() {pos - 4, pos + 4};
                    }
                    else if (pos == 12 || pos == 20 || pos == 28)
                    {
                        RedNotKingCanMove[pos] = new() {pos + 4};
                        BlackNotKingCanMove[pos] = new() {pos - 4};
                        KingCanMove[pos] = new() {pos - 4, pos + 4};
                    }
                    else if (r % 2 == 0)
                    {
                        RedNotKingCanMove[pos] = new() {pos + 4, pos + 5};
                        BlackNotKingCanMove[pos] = new() {pos - 4, pos - 3};
                        KingCanMove[pos] = new() {pos - 4, pos - 3, pos + 4, pos + 5};
                    }
                    else
                    {
                        RedNotKingCanMove[pos] = new() {pos + 3, pos + 4};
                        BlackNotKingCanMove[pos] = new() {pos - 4, pos - 5};
                        KingCanMove[pos] = new() {pos - 5, pos + 3, pos + 4, pos - 5};
                    }
                }
            }

            foreach (var pos in new[] {10, 11, 14, 15, 18, 19, 22, 23})
            {
                var (ul, ur, dl, dr) = pos switch
                {
                    10 or 11 or 18 or 19 => (pos - 4, pos + 4, pos - 3, pos + 5),
                    _ => (pos - 5, pos + 3, pos - 4, pos + 4)
                };

                CaptureMove[(pos, ul)] = pos - 9;
                CaptureMove[(pos, ur)] = pos + 7;
                CaptureMove[(pos, dl)] = pos - 7;
                CaptureMove[(pos, dr)] = pos + 9;
            }
        }

        public static List<(int from, int to)> PossibleMoves(this GameInfoData gameInfoData, Player player)
        {
            var squares = gameInfoData.GetPlayerSquares(player);

            var posDest = squares
                .Select(s =>
                    (s.Position, to: s.King
                        ? KingCanMove[s.Position]
                        : (s.Color == Player.Black ? BlackNotKingCanMove[s.Position] : RedNotKingCanMove[s.Position])))
                .Select(p => (p.Position,
                    (p.to.Where(pos => gameInfoData.GetSquare(pos) == null),
                        p.to.Where(pos => gameInfoData.GetSquare(pos) != null))));

            var canMove = posDest
                .SelectMany(p => p.Item2.Item1
                    .Select(i => (p.Position, i))).ToList();

            var captureMove = posDest
                .SelectMany(p => p.Item2.Item2
                    .Where(d => gameInfoData.GetSquare(d)?.Color != player && CaptureMove.ContainsKey((p.Position, d)) &&
                                gameInfoData.GetSquare(CaptureMove[(p.Position, d)]) == null)
                    .Select(i => (p.Position, CaptureMove[(p.Position, i)]))).ToList();

            return captureMove.Any() ? new (){captureMove[0]} : canMove;
        }
    }
}