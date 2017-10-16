using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour {

    public Particle Particle;

    private List<Particle> particles;
    private bool shouldGenerateNewParticle;
    private float deltaTime;
    private float threshold = 0.1f;

	// Use this for initialization
	void Start () {
        shouldGenerateNewParticle = false;
        particles = new List<Particle>();
        deltaTime = 0.0f;
	}

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime > threshold) {
            deltaTime = 0.0f;
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
    }
}
