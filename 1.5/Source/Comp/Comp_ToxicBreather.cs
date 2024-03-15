using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Thek_WasterToxicBreather
{ //I am in pain
    [StaticConstructorOnStartup]
    public static class OnLoadMessage
    {
        static OnLoadMessage()
        {
            Log.Message("<color=#702963>Thek was here:</color> WasterToxicBreather <color=#702963>loaded!</color>");
        }
    }
    public class Comp_ToxicBreather : ThingComp
    {
        public bool IsApparel => parent is Apparel;//To check if something is apparel, for the gizmos down below
        /// <summary>
        /// This one should handle the ticking to handle the ticking and fuel usage of the breather.
        /// </summary>
        public override void CompTick()
        {
            var pawn = (parent as Apparel)?.Wearer; //Gets the pawn that wears this breather, so I can access the hediffset, where hediffs are stored
            if (parent.IsHashIntervalTick(60))
            {
                if (pawn != null && parent.GetComp<CompApparelReloadable>()?.CanBeUsed(out var _) == true) //Checks if the breather has charges to do a whiff, if it doesn't it does nothing
                {
                    if (pawn.genes.Xenotype.defName != "Waster") //If the pawn is not a Waster...
                    {
                        if (!pawn.health.hediffSet.HasHediff(HediffDefOf.ToxicBuildup)) //and he doesn't have toxic buildup
                        {
                            HealthUtility.AdjustSeverity(pawn, HediffDefOf.ToxicBuildup, 0.5f); //...Give toxic buildup
                            parent.GetComp<CompApparelReloadable>().UsedOnce(); //Use up a charge
                        }
                        else if (pawn.genes.Xenotype.defName != "Waster") //If the pawn is not a waster
                        {
                            if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup).Severity < 0.489f)//but he has toxic buildup and it's under 0.489 severity
                            {
                                pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup).Severity = 0.5f;//set the severity at 0.5f, this is me trying to make the usage of the mask equal for wasters and non wasters
                                parent.GetComp<CompApparelReloadable>().UsedOnce(); //Use up a charge
                            }
                        }
                    }

                    else if (ModsConfig.IsActive("vanillaracesexpanded.waster")) //If the pawn is a VE waster...
                    {
                        foreach (HediffDef heddifDef in Props.PollutionList) //This will iterate every entry in HediffDefOfs, and make heddifDef be each one of the entries in said list for each iteration
                        {
                            Hediff pawnHediff = pawn.health.hediffSet.GetFirstHediffOfDef(heddifDef);
                            //Grabs each hediff Def off the hediffDef variable, that will be different for each iteration, then will look in the pawn's health for the first hediff it can find that corresponds to that def
                            //The results of this will be given to pawnHediff
                            if (pawnHediff != null)
                            {
                                if (pawnHediff.Severity < 0.02f)//If the severity falls under 0.02 (hediff deactivated)
                                {
                                    pawnHediff.Severity += 0.47f;//Adds 0.47 severity, putting it at around 0.49 severity
                                    parent.GetComp<CompApparelReloadable>()?.UsedOnce();//Uses up charge, using Comp_Reloadable
                                                                                 //From a quick test you need around 7 charges go to through a single day
                                }
                            }
                        }
                    }

                    else if (!pawn.health.hediffSet.HasHediff(HediffDefOf.PollutionStimulus)) //If the pawn is a Biotech waster and doesn't have pollutionstimulus...
                    {
                        HealthUtility.AdjustSeverity(pawn, HediffDefOf.PollutionStimulus, 0.5f); //Make pollutionstimulus
                        parent.GetComp<CompApparelReloadable>()?.UsedOnce(); //Use up a charge
                    }
                    else if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PollutionStimulus).Severity < 0.02f) //If the pawn is a Biotech waster and does have pollutionstimulus under 0.02 severity
                    {
                        pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PollutionStimulus).Severity = 0.5f; //Increase severity to 0.5
                        parent.GetComp<CompApparelReloadable>().UsedOnce(); //Use up a charge
                    }
                }
            }
        }

        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {

            var pawn = (parent as Apparel)?.Wearer;
            if (!pawn.Drafted) //Makes the gizmo appear only if the pawn is not drafted
            {
                foreach (Gizmo item in base.CompGetWornGizmosExtra())
                {
                    yield return item;
                }
                if (IsApparel)
                {
                    foreach (Gizmo gizmo in GetGizmos())
                    {
                        yield return gizmo;
                    }
                }
            }
        }
        public IEnumerable<Gizmo> GetGizmos()
        {

            var pawn = (parent as Apparel)?.Wearer;
            var aparel = parent as Apparel;
            if (pawn.apparel.Contains(aparel))
            {
                gizmo_ToxicBreatherLabel gizmo_ToxicBreatherLabel = new gizmo_ToxicBreatherLabel();
                gizmo_ToxicBreatherLabel.currentcharges = parent.GetComp<CompApparelReloadable>()?.RemainingCharges / 7;
                gizmo_ToxicBreatherLabel.maxcharges = parent.GetComp<CompApparelReloadable>()?.MaxCharges / 7;
                yield return gizmo_ToxicBreatherLabel;
            }
        }
        public CompProperties_ToxicBreather Props => (CompProperties_ToxicBreather)props;
    }
}
