using HarmonyLib;
using SandBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace UnlimitedMercenaryClans
{
    public class LordConversationsCampaignBehaviorPatch
    {
        public static void Apply(Harmony harmony)
        {
            try
            {
                var klass = typeof(AgentBehavior).Assembly.GetType("SandBox.LordConversationsCampaignBehavior", true);
                var method = klass.GetMethod("conversation_player_want_to_hire_mercenary_on_condition");
                harmony.Patch(method, postfix: new HarmonyMethod(typeof(LordConversationsCampaignBehaviorPatch), nameof(Postfix)));
            }
            catch (Exception ex)
            {
                File.WriteAllText(Environment.ExpandEnvironmentVariables("%TEMP%\\error.UnlimitedMercenaryClans.log"), ex.ToString());
            }
        }

        public static void Postfix(CampaignBehaviorBase __instance, ref bool __result)
        {
            if (Hero.MainHero.MapFaction != null
                && Hero.MainHero.MapFaction.IsKingdomFaction
                && ((Kingdom)Hero.MainHero.MapFaction).Ruler == Hero.MainHero
                && Hero.OneToOneConversationHero.IsMinorFactionHero
                && !FactionManager.IsAtWarAgainstFaction(Hero.OneToOneConversationHero.MapFaction, Hero.MainHero.MapFaction)
                && Hero.OneToOneConversationHero.Clan.MapFaction != Hero.MainHero.MapFaction)
            {
                __result = true;
            }
        }
    }
}
