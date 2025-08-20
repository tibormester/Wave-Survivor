using UnityEngine;

public static class CircleUtility {
	public static Vector2 GetCenter(Vector2 a, Vector2 b) {
		return 0.5f * (a + b);
	}

	public static float GetRadius(Vector2 a, Vector2 b) {
		return 0.5f * (a - b).magnitude;
	}

	public static Vector2 ProjectPointToCircle(Vector2 c, Vector2 a, Vector2 b) {
		var o = GetCenter(a, b);
		var r = GetRadius(a, b);
		var dir = c - o;
		if (dir.sqrMagnitude == 0f) {
			return o + new Vector2(r, 0f); // arbitrary direction if c == center
		}
		return o + dir.normalized * r;
	}

	public static Vector2 TangentDirectionAt(Vector2 p, Vector2 a, Vector2 b) {
		var o = GetCenter(a, b);
		var radial = p - o;
		var tangent = new Vector2(-radial.y, radial.x);
		return tangent.sqrMagnitude > 0f ? tangent.normalized : new Vector2(1f, 0f);
	}

	// Deterministic orientation: set clockwise = false for CCW (left), true for CW (right)
	public static Vector2 TangentDirectionAt(Vector2 p, Vector2 a, Vector2 b, bool clockwise) {
		var o = GetCenter(a, b);
		var radial = p - o;
		if (radial.sqrMagnitude == 0f) {
			return new Vector2(1f, 0f);
		}
		var ccw = new Vector2(-radial.y, radial.x).normalized; // +90° (left)
		return clockwise ? -ccw : ccw;
	}

	// Projects c to the circle defined by a,b and returns the tangent direction at the projection.
	// Use clockwise = false for CCW (left-hand) tangent, true for CW (right-hand) tangent.
	public static Vector2 TangentDirectionAtProjection(Vector2 c, Vector2 a, Vector2 b, bool clockwise, out Vector2 projection) {
		projection = ProjectPointToCircle(c, a, b);
		return TangentDirectionAt(projection, a, b, clockwise);
	}

	public static bool TryTangentLine(Vector2 p, Vector2 a, Vector2 b, out float slope, out float intercept, out bool isVertical, out float xIntercept) {
		var o = GetCenter(a, b);
		var radial = p - o;

		// Vertical tangent when radial.y == 0
		if (Mathf.Approximately(radial.y, 0f)) {
			isVertical = true;
			xIntercept = p.x;
			slope = 0f;
			intercept = 0f;
			return true;
		}

		isVertical = false;
		xIntercept = 0f;
		slope = -radial.x / radial.y;
		intercept = p.y - slope * p.x;
		return true;
	}
}