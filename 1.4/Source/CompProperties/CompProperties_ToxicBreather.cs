using System.Collections.Generic;
using Verse;

namespace Thek_WasterToxicBreather
{
    public class CompProperties_ToxicBreather : CompProperties
    {
        public CompProperties_ToxicBreather()
        {
            compClass = typeof(Comp_ToxicBreather);
        }
        public List<HediffDef> PollutionList = new();
    }
}
