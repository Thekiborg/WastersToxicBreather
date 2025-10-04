using System.Collections.Generic;
using Verse;

namespace Thek_WasterToxBreather
{
	public class CompProperties_ToxBreather : CompProperties
	{
		public CompProperties_ToxBreather()
		{
			compClass = typeof(Comp_ToxBreather);
		}


		public List<HediffDef> wasterGeneHediffs = [];
		public float wasterHediffSeverityOffset;
		public float toxicBuildupSeverityOffset;
	}
}
