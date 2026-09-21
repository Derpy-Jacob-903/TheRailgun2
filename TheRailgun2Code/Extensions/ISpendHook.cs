using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheRailgun2.TheRailgun2Code.Extensions;

public interface IAfterOrbSpent
{
    Task AfterOrbSpent(PlayerChoiceContext ctx, CardPlay play);
}

