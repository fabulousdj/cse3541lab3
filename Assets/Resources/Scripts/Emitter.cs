using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour {

    public static float FORCE_MAGNITUDE = 10.0f;
    public static float MAX_SPEED = 5.0f;
    public static float MIN_SPEED = 1.0f;

    public Particle Particle;
    public int MaxNumOfParticles = 20;
    public float DeltaTime = 8.0f;
    public float Threshold = 10.0f;

    private List<Particle> particles;

	// Use this for initialization
	void Start () {
        particles = new List<Particle>();
	}

    // Update is called once per frame
    void Update()
    {
        DeltaTime += Time.deltaTime;
        if (DeltaTime > Threshold) {
            DeltaTime = 0.0f;
            GenerateParticles();
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
		//ParticleParticleCollisionHandler ();
        p.UpdatePhysics(deltaTime);
    }

    void ParticleParticleCollisionHandler(){
		foreach (Particle p in particles) {
			foreach (Particle q in particles) {
				if (!p.Equals (q)) {
					float distance = Mathf.Pow (p.center.x - q.center.x, 2) + Mathf.Pow (p.center.y - q.center.y, 2) + Mathf.Pow(p.center.z - q.center.z, 2);
					if (Mathf.Sqrt (distance) <= (p.radius + q.radius)) {
                        Vector3 pForceDirection = p.center - q.center;
                        Vector3 qForceDirection = -pForceDirection;

                        p.ApplyForce(pForceDirection * FORCE_MAGNITUDE);
                        q.ApplyForce(qForceDirection * FORCE_MAGNITUDE);

					}
				}
			}
		}
	}

    void GenerateParticles() {
        for (int i = 0; i < this.MaxNumOfParticles; i++) {
            Vector3 origin = new Vector3(0, 1.5f, 0);
            Vector3 pos = GetRandomPosition(origin);
            Particle p = Particle.Create(pos, GetRandomVelocity(origin, pos), 10.0f);
            particles.Add(p);
        }
    }

    Vector3 GetRandomPosition(Vector3 origin) {
        float posX = origin.x + Random.Range(-1.0f, 1.0f);
        float posY = origin.y + Random.Range(-1.0f, 1.0f);
        float posZ = origin.z + Random.Range(-1.0f, 1.0f);

        while (Mathf.Sqrt(Mathf.Pow(posX - origin.x, 2) + Mathf.Pow(posY - origin.y, 2) + Mathf.Pow(posZ - origin.z, 2)) > 1) {
            posX = origin.x + Random.Range(-1.0f, 1.0f);
            posY = origin.y + Random.Range(0.0f, 1.0f);
            posZ = origin.z + Random.Range(-1.0f, 1.0f);
        }

        return new Vector3(posX, posY, posZ);
    }

    Vector3 GetRandomVelocity(Vector3 origin, Vector3 pos) {
        Vector3 direction = (pos - origin).normalized;
        return direction * Random.Range(MIN_SPEED, MAX_SPEED);
    }
}
