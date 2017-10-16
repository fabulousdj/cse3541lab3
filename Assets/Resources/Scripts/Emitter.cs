using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour {

    public static float FORCE_MAGNITUDE = 10.0f;

    public Particle Particle;

    public float DeltaTime;
    public float Threshold = 1f;

    private List<Particle> particles;

	// Use this for initialization
	void Start () {
        particles = new List<Particle>();
        DeltaTime = 0.0f;
	}

    // Update is called once per frame
    void Update()
    {
        DeltaTime += Time.deltaTime;
        if (DeltaTime > Threshold) {
            DeltaTime = 0.0f;
            GenerateNewParticle();
        }
        foreach (Particle p in particles)
        {
            UpdateParticle(p, Time.deltaTime);
        }
        particles.RemoveAll(IsDisposable);
    }

    private bool IsDisposable(Particle p) {
        return p.Disposable;
    }

    void GenerateNewParticle() {
        Particle p = Particle.Create(new Vector3(0, 3, 0), new Vector3(1, 1, 1), 10.0f);
        particles.Add(p);
    }

    void UpdateParticle(Particle p, float deltaTime) {
        p.Age += deltaTime;
        if (p.Age > p.MaxAge)
        {
            p.Disposable = true;
        }
        p.ResetForce();
		PPCollisionHandler ();
        p.UpdatePhysics(deltaTime);
    }

	void PPCollisionHandler(){
		foreach (Particle p in particles) {
			foreach (Particle q in particles) {
				if (!p.Equals (q)) {
					float distance = Mathf.Pow (p.center.x - q.center.x, 2) + Mathf.Pow (p.center.y - q.center.y, 2) + Mathf.Pow(p.center.z - q.center.z, 2);
					if (Mathf.Sqrt (distance) <= (p.radius + q.radius)) {
                        Vector3 direction1 = p.center - q.center;
                        Vector3 direction2 = -direction1;

                        p.ApplyForce(direction1 * FORCE_MAGNITUDE);
                        q.ApplyForce(direction2 * FORCE_MAGNITUDE);

						//float newVelocity_x = ((float)p.Velocity.x + (float)q.Velocity.x) / 2;
						//float newVelocity_y = ((float)p.Velocity.y + (float)q.Velocity.y) / 2;
						//float newVelocity_z = ((float)p.Velocity.z + (float)q.Velocity.z) / 2;
						//Vector3 newVelocity = new Vector3 (newVelocity_x, newVelocity_y, newVelocity_z);
						////Vector3 tmp = p.Velocity
						//p.Velocity = newVelocity;
						//q.Velocity = newVelocity;
						////p.Velocity = q.Velocity;

					}
				}
			}
		}
	}
}
