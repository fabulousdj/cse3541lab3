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
    public GameObject BackWall;
    public GameObject FrontWall;
    public GameObject LeftWall;
    public GameObject RightWall;
    public GameObject Ceiling;
    public GameObject Floor;
    
    
    private Vector3 acceleration;
    
    // Use this for initialization
    void Start () {
        BackWall = GameObject.Find("BackWall");
        FrontWall = GameObject.Find ("FrontWall");
        LeftWall = GameObject.Find ("LeftWall");
        RightWall = GameObject.Find ("RightWall");
        Ceiling = GameObject.Find ("Ceiling");
        Floor = GameObject.Find ("Floor");
        
        acceleration = Force / Mass;
        Debug.Log (BackWall.transform.position);
        CollisionDetection ();
    }
    
    // Update is called once per frame
    void Update () {
        float deltaTime = Time.deltaTime;
        this.Position = CalculateNewPosition(deltaTime);
        this.Velocity = CalculateNewVelocity(deltaTime);
        this.transform.position = this.Position;
        
        CollisionDetection ();
    }
    
    private Vector3 CalculateNewPosition(float deltaTime)
    {
        return this.Position + CalculateNewVelocity(deltaTime / 2) * deltaTime;
    }
    
    private Vector3 CalculateNewVelocity(float deltaTime)
    {
        return this.Velocity + this.acceleration * deltaTime;
    }
    
    private void CollisionDetection(){
        
        float floor_bound_y = Floor.transform.position.y + Floor.transform.localScale.y / 2;
        float ceiling_bound_y = Ceiling.transform.position.y - Ceiling.transform.localScale.y / 2;
        float rightWall_bound_x = RightWall.transform.position.x - RightWall.transform.localScale.x / 2;
        float leftWall_bound_x = LeftWall.transform.position.x + LeftWall.transform.localScale.x / 2;
        float frontWall_bound_z = FrontWall.transform.position.z - FrontWall.transform.localScale.z / 2;
        float backWall_bound_z = BackWall.transform.position.z + BackWall.transform.localScale.z / 2;
        
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        Vector3[] local_vertices = this.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] world_vertices = new Vector3[local_vertices.Length];
        for (int i = 0; i < local_vertices.Length; i++) {
            world_vertices [i] = localToWorld.MultiplyPoint3x4(local_vertices[i]);
        }
        
        float sum_x = 0f, sum_y = 0f, sum_z = 0f;
        for (int i = 0; i < local_vertices.Length; i++) {
            sum_x += world_vertices [i].x;
            sum_y += world_vertices [i].y;
            sum_z += world_vertices [i].z;
        }
        
        Vector3 center = new Vector3 (sum_x / world_vertices.Length, sum_y / world_vertices.Length, sum_z / world_vertices.Length);
        Debug.Log (center);
        float radius = 0f;
        for (int i = 0; i < world_vertices.Length; i++) {
            float distance = Mathf.Pow ((world_vertices [i].x - center.x), 2) + Mathf.Pow ((world_vertices [i].y - center.y), 2) + Mathf.Pow ((world_vertices [i].z - center.z), 2);
            if (distance > Mathf.Pow(radius, 2))
            radius = Mathf.Sqrt(distance);
        }
        
        if (Mathf.Abs ((float)(center.y - floor_bound_y)) <= radius || center.y < floor_bound_y) {
            Vector3 newVelocity = this.Velocity;
            Vector3 newPosition = this.Position;
            newVelocity.y = -newVelocity.y;
            newPosition.y = floor_bound_y + radius;
            this.Position = newPosition;
            this.Velocity = newVelocity;
        }
        if (Mathf.Abs ((float)(center.y - ceiling_bound_y)) <= radius) {
            Vector3 newVelocity = this.Velocity;
            newVelocity.y = -newVelocity.y;
            this.Velocity = newVelocity;
        }
        if (Mathf.Abs ((float)(center.x - rightWall_bound_x)) <= radius) {
            Vector3 newVelocity = this.Velocity;
            newVelocity.x = -newVelocity.x;
            this.Velocity = newVelocity;
        }
        if (Mathf.Abs ((float)(center.x - leftWall_bound_x)) <= radius) {
            Vector3 newVelocity = this.Velocity;
            newVelocity.x = -newVelocity.x;
            this.Velocity = newVelocity;
        }
        if (Mathf.Abs ((float)(center.z - frontWall_bound_z)) <= radius) {
            Vector3 newVelocity = this.Velocity;
            newVelocity.z = -newVelocity.z;
            this.Velocity = newVelocity;
        }
        if (Mathf.Abs ((float)(center.z - backWall_bound_z)) <= radius) {
            Vector3 newVelocity = this.Velocity;
            newVelocity.z = -newVelocity.z;
            this.Velocity = newVelocity;
        }
        
        
        
    }
    
}
