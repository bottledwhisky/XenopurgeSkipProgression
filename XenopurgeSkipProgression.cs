using HarmonyLib;
using MelonLoader;
using SpaceCommander.EndGame;
using SpaceCommander.UI;
using System.Linq;

[assembly: MelonInfo(typeof(XenopurgeSkipProgression.XenopurgeSkipProgression), "Xenopurge Skip Progression Screen", "1.0.0", "Felix Hao")]
[assembly: MelonGame("Traptics", "Xenopurge")]
namespace XenopurgeSkipProgression
{
    public class XenopurgeSkipProgression : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Xenopurge Skip Progression Screen Loaded!");
        }
    }

    [HarmonyPatch(typeof(EndGameWindowController))]
    public class GenerateLocationsPatch
    {
        static EndGameResultData _endGameResultData;

        [HarmonyPatch("ShowEndScreen")]
        [HarmonyPostfix]
        public static void ShowEndScreen_Prefix(EndGameWindowController __instance, EndGameResultData endGameResultData)
        {
            _endGameResultData = endGameResultData;
        }

        [HarmonyPatch("OnShowCommanderProgressionClicked")]
        [HarmonyPrefix]
        public static bool OnShowCommanderProgressionClicked_Prefix(EndGameWindowController __instance)
        {
            if (_endGameResultData.MustLeaveToSeeLogTerminal || _endGameResultData.RanksGained.Any() || _endGameResultData.ChallengesCompleted.Any() || _endGameResultData.HasUnlockedDifficulty || _endGameResultData.HasUnlockedVariant || _endGameResultData.NextVariant != null)
            {
                return true;
            }

            var _endGameWindowView = AccessTools.Field(typeof(EndGameWindowController), "_endGameWindowView").GetValue(__instance) as EndGameWindowView;
            _endGameWindowView.gameObject.SetActive(false);

            if (_endGameResultData.IsVictory)
            {
                AccessTools.Method(typeof(EndGameWindowController), "OnGoToPartyCustmizationButtonClicked").Invoke(__instance, null);
            }
            else
            {
                AccessTools.Method(typeof(EndGameWindowController), "OnExitButtonClicked").Invoke(__instance, null);
            }
            return false;
        }
    }
}
