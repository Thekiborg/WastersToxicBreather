using UnityEngine;
using Verse;

namespace Thek_WasterToxBreather
{
	[StaticConstructorOnStartup]
	public class Gizmo_ToxBreather : Gizmo
	{
		public int? maxcharges;
		public int? currentcharges;


		public override float GetWidth(float maxWidth)
		{
			return 110f;
		}


		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			rect3.height = rect.height / 2f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect3, "Days worth of charges");
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect4 = rect;
			rect4.y += rect3.height - 5f;
			rect4.height = rect.height / 2f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, currentcharges.ToString() + "/" + maxcharges.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			return new GizmoResult(GizmoState.Clear);
		}
	}
}
