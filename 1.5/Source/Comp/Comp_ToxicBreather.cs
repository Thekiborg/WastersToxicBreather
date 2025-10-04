using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Thek_WasterToxBreather
{
	public class Comp_ToxBreather : ThingComp
	{
		private CompApparelReloadable compApparelReloadable;


		public CompProperties_ToxBreather Props => (CompProperties_ToxBreather)props;
		private Pawn Wearer => (parent as Apparel).Wearer;
		private CompApparelReloadable CompApparelReloadable
		{
			get
			{
				compApparelReloadable ??= parent.GetComp<CompApparelReloadable>();
				return compApparelReloadable;
			}
		}


		public override void CompTickLong()
		{
			base.CompTickLong();
			if (Wearer is null || !CompApparelReloadable.CanBeUsed(out var _))
				return;

			bool geneHediffFound = false;
			foreach (HediffDef geneHediff in Props.wasterGeneHediffs)
			{
				if (geneHediff == HediffDefOf.PollutionStimulus)
				{
					// PollutionStimulus sucks ass, i have to handle this myself
					if (Wearer.genes.GetFirstGeneOfType<Gene_PollutionRush>() is not null
						&& !Wearer.health.hediffSet.HasHediff(geneHediff))
					{
						Hediff hediff = HediffMaker.MakeHediff(geneHediff, Wearer);
						hediff.Severity = Props.wasterHediffSeverityOffset;
						Wearer.health.AddHediff(hediff);
					}
					continue;
				}

				if (Wearer.health.hediffSet.HasHediff(geneHediff))
				{
					geneHediffFound = true;

					Hediff hediff = HediffMaker.MakeHediff(geneHediff, Wearer);
					hediff.Severity = Props.wasterHediffSeverityOffset;
					Wearer.health.AddHediff(hediff);
				}
			}

			if (!geneHediffFound)
			{
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.ToxicBuildup, Wearer);
				hediff.Severity = Props.toxicBuildupSeverityOffset;
				Wearer.health.AddHediff(hediff);
			}
			CompApparelReloadable.UsedOnce();
		}


		public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			foreach (Gizmo item in base.CompGetWornGizmosExtra())
			{
				yield return item;
			}

			if (!Wearer.Drafted)
			{
				yield return new Gizmo_ToxBreather()
				{
					currentcharges = parent.GetComp<CompApparelReloadable>()?.RemainingCharges,
					maxcharges = parent.GetComp<CompApparelReloadable>()?.MaxCharges
				};
			}
		}
	}
}
