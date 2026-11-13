using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace TheRailgun2.TheRailgun2Code.Extensions;

[HarmonyPatch(typeof(NCharacterSelectScreen), nameof(NCharacterSelectScreen.SelectCharacter))]
internal static class NCharacterSelectScreenSelectCharacterPatch
{
    [HarmonyPostfix]
    internal static void Postfix(CharacterModel characterModel)
    {
        if (characterModel is Character.TheRailgun2 && !RailgunTwoMainFile.RailgunModConfig.UploadMetricsFtueSeen)
            FtueMetricsCollectionForm.CreateAndShowDataCollectionForm();
    }
}

// code based on https://github.com/Blizzarre/Runesmith2-StS2/blob/ae49943718ecd101e46c871372d29bc5091734ca/Runesmith2Code/Models/FtueMetricsCollectionForm.cs
public static class FtueMetricsCollectionForm
{
    public static void CreateAndShowDataCollectionForm()
    {
        var promptPopup = NGenericPopup.Create();
        if (promptPopup == null || NModalContainer.Instance == null) return;

        promptPopup.Connect(Node.SignalName.Ready, Callable.From(() =>
        {
            var locStringBody = new LocString("main_menu_ui", "THERAILGUN2-RAILGUN_MOD_METRICS_FTUE_PROMPT.body");
            locStringBody.Add("Enabled", RailgunTwoMainFile.RailgunModConfig.UploadMetrics);

            var vPopup = promptPopup.GetNode<NVerticalPopup>((NodePath)"VerticalPopup");
            vPopup.SetText(new LocString("main_menu_ui", "THERAILGUN2-RAILGUN_MOD_METRICS_FTUE_PROMPT.header"),
                locStringBody);
            vPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), _ =>
            {
                OnConfirmation(promptPopup);
                AfterSelection(true);
            });
            vPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), _ =>
            {
                OnConfirmation(promptPopup);
                AfterSelection(false);
            });
        }), (uint)GodotObject.ConnectFlags.OneShot);
        
        NModalContainer.Instance.CallDeferred(NModalContainer.MethodName.Add, promptPopup, true);
    }

    private static void OnConfirmation(NGenericPopup popup)
    {
        popup.QueueFreeSafely();
        NModalContainer.Instance?.Clear();
    }

    private static void AfterSelection(bool choice)
    {
        RailgunTwoMainFile.RailgunModConfig.UploadMetrics = choice;
        RailgunTwoMainFile.RailgunModConfig.UploadMetricsFtueSeen = true;
        ModConfig.SaveDebounced<RailgunTwoMainFile.RailgunModConfig>();

        var messagePopup = NGenericPopup.Create();
        if (messagePopup == null || NModalContainer.Instance == null) return;

        messagePopup.Connect(Node.SignalName.Ready, Callable.From(() =>
        {
            var locStringHeader = new LocString("main_menu_ui", "THERAILGUN2-RAILGUN_MOD_METRICS_FTUE_MESSAGE.header");
            locStringHeader.Add("Enabled", RailgunTwoMainFile.RailgunModConfig.UploadMetrics);
            var locStringBody = new LocString("main_menu_ui", "THERAILGUN2-RAILGUN_MOD_METRICS_FTUE_MESSAGE.body");
            locStringBody.Add("Enabled", RailgunTwoMainFile.RailgunModConfig.UploadMetrics);

            var vPopup = messagePopup.GetNode<NVerticalPopup>((NodePath)"VerticalPopup");
            vPopup.SetText(locStringHeader, locStringBody);
            vPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.ok"),
                _ => OnConfirmation(messagePopup));
            vPopup.HideNoButton();
        }), (uint)GodotObject.ConnectFlags.OneShot);
        
        NModalContainer.Instance.CallDeferred(NModalContainer.MethodName.Add, messagePopup, true);
    }
}