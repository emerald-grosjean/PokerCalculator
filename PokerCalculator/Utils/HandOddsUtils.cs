using HoldemHand;
using PokerEvalApi.Models;

namespace PokerCalculator.Utils;

public static class HandOddsUtils
{
    public static PokerOddsResponse CalculateHandOdds(PokerOddsRequest item)
    {
        const int RateDigits = 4;

        var wins = new long[item.Pockets.Count];
        var losses = new long[item.Pockets.Count];
        var ties = new long[item.Pockets.Count];
        long totalHands = 0;

        Hand.HandOdds(
            pockets: item.Pockets.Select(_ => _.Pocket).ToArray(),
                board: item.Board,
                dead: string.Empty,
                wins: wins,
                losses: losses,
                ties: ties,
                totalHands: ref totalHands);

        var items = new List<PokerOddsResponseItem>();

        for (int i = 0; i < item.Pockets.Count; i++)
        {
            var winRate = Math.Round((wins[i] + ties[i] / (double)item.Pockets.Count) / totalHands, RateDigits);
            items.Add(new()
            {
                Pocket = item.Pockets[i].Pocket,
                WinRate = winRate,
            });
        }

        return new()
        {
            Pockets = items,
            Board = item.Board,
        };
    }
}
