using System;
using UnityEngine;

public class Particle : MonoBehaviour {

	public Vector3 Position;
	public Vector3 Velocity;
	public Vector3 Force;
	public TimeSpan Age;
	public TimeSpan MaxAge;
	public float Mass = 1.0f;
	public GameObject Apperance;

	private Vector3 acceleration;

	// Use this for initialization
	void Start () {
		acceleration = Force / Mass;
	}
	
	// Update is called once per frame
	void Update () {
		float deltaTime = Time.deltaTime;
		this.Position = CalculateNewPosition(deltaTime);
		this.Velocity = CalculateNewVelocity(deltaTime);
		this.transform.position = this.Position;
	}

	private Vector3 CalculateNewPosition(float deltaTime)
	{
		return this.Position + CalculateNewVelocity(deltaTime / 2) * deltaTime;
	}

	private Vector3 CalculateNewVelocity(float deltaTime)
	{
		return this.Velocity + this.acceleration * deltaTime;
	}

}
