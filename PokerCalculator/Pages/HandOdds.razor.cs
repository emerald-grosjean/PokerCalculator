using Microsoft.AspNetCore.Components;
using PokerEvalApi.Const;
using PokerEvalApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PokerCalculator.Pages;

public partial class HandOdds
{
    [SupplyParameterFromForm]
    private HandOddsInputModel? Model { get; set; } = new();

    private PokerOddsResponse? Item { get; set; }

    private bool IsCalculating = false;

    private async Task Submit()
    {
        Item = null;

        if (Model == null) return;

        IsCalculating = true;

        try
        {
            var request = new PokerOddsRequest()
            {
                Pockets = [
                    new() { Pocket = Model.Pocket1!  },
                    new() { Pocket = Model.Pocket2!  },
                ],
                Board = Model.Board,
            };

            Item = await Task.Run(() =>
            {
                return Utils.HandOddsUtils.CalculateHandOdds(request);
            });
        }
        finally
        {
            IsCalculating = false;
        }
    }

    public class HandOddsInputModel
    {
        [RegularExpression(Patterns.BoardPattern)]
        public string? Board { get; set; }

        [Required]
        [RegularExpression(Patterns.PocketPattern)]
        public string Pocket1 { get; set; } = "AsAh";

        [Required]
        [RegularExpression(Patterns.PocketPattern)]
        public string Pocket2 { get; set; } = "7d2c";
    }
}
