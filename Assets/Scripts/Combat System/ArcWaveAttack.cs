using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class ArcWaveAttack : Attack
{
	public float waveSpeed = 1f;
	public float waveMaxSize = 5f;
	public float waveDamage = 1f;
	public float arcAngleDegrees = 60f;

	private readonly HashSet<HealthController> _alreadyHit = new HashSet<HealthController>();
	private LineRenderer _lineRenderer;
	private CircleCollider2D _circleCollider;
	public float currentRadius = 0f;

	private float _directionAngleDegrees;
	const int ARC_SEGMENTS = 60;

	void Start()
	{
		_lineRenderer = GetComponent<LineRenderer>();
		_circleCollider = GetComponent<CircleCollider2D>();
		if (_lineRenderer != null)
		{
			_lineRenderer.loop = false;
		}

		// Determine cast direction from the source to the mouse at cast time
		Vector3 origin = transform.position;
		Vector3 mouseWorld = origin + Vector3.right;
		if (Camera.main != null)
		{
			var mp = Input.mousePosition;
			mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mp.x, mp.y, -Camera.main.transform.position.z));
		}
		Vector2 castDir = (mouseWorld - origin);
		if (castDir.sqrMagnitude < 0.0001f)
		{
			castDir = transform.right;
		}
		_directionAngleDegrees = Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg;

		DrawArc(currentRadius);
	}

	void Update()
	{
		currentRadius += waveSpeed * Time.deltaTime;
		DrawArc(currentRadius);
		_circleCollider.radius = currentRadius;

		if (currentRadius > waveMaxSize)
		{
			Destroy(gameObject);
		}
	}

	private void DrawArc(float radius)
	{
		if (_lineRenderer == null) return;

		int pointsCount = Mathf.Max(2, ARC_SEGMENTS + 1);
		Vector3[] points = new Vector3[pointsCount];

		float halfArc = arcAngleDegrees * 0.5f;
		float startAngle = _directionAngleDegrees - halfArc;
		float endAngle = _directionAngleDegrees + halfArc;

		for (int i = 0; i < pointsCount; i++)
		{
			float t = (float)i / (pointsCount - 1);
			float angleDeg = Mathf.Lerp(startAngle, endAngle, t);
			float angleRad = angleDeg * Mathf.Deg2Rad;
			float x = Mathf.Cos(angleRad) * radius;
			float y = Mathf.Sin(angleRad) * radius;
			points[i] = new Vector3(x, y, -2f);
		}

		_lineRenderer.positionCount = pointsCount;
		_lineRenderer.SetPositions(points);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other == null) return;

		// Ignore source and its hierarchy
		if (SourceObject != null && other.transform.IsChildOf(SourceObject.transform)) return;

		var health = other.GetComponentInParent<HealthController>();
		if (health == null) return;
		if (SourceObject != null && health.transform.IsChildOf(SourceObject.transform)) return;
		if (_alreadyHit.Contains(health)) return;

		// Check if the object lies within the arc sector at the moment of intersection
		Vector2 toTarget = (Vector2)(health.transform.position - transform.position);
		float angleToTarget = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
		float delta = Mathf.Abs(Mathf.DeltaAngle(_directionAngleDegrees, angleToTarget));
		if (delta > arcAngleDegrees * 0.5f) return;

		health.AdjustHealth(-(int)waveDamage, gameObject);
		_alreadyHit.Add(health);
	}
}


