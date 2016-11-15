using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileFunction : MonoBehaviour {

    public float maxDistance = 10.0f;

    private Vector3 prevPos;
    private float totalDistanceTravelled = 0.0f;
    private GameObject[] localColliders;

    private List<Collider> collisionIgnoreList = new List<Collider>();

    public void BuildCollisionIgnoreList(Transform[] ignoreList)
    {
        if (collisionIgnoreList != null)
        {
            collisionIgnoreList = new List<Collider>();
        }

        for (int i = 0; i < ignoreList.Length; i++)
        {
            CollectAllGameObjects(ignoreList[i]);
        }
    }

    public void BuildCollisionIgnoreList(Transform ignoreList)
    {
        if (collisionIgnoreList != null)
        {
            collisionIgnoreList = new List<Collider>();
        }

        CollectAllGameObjects(ignoreList);
    }

    void CollectAllGameObjects(Transform container)
    {
        if (container.childCount > 0)
        {
            for (int i = 0; i < container.childCount; i++)
            {
                if (container.GetChild(i).GetComponent<Collider>() != null)
                {
                    collisionIgnoreList.Add(container.GetChild(i).GetComponent<Collider>());
                }

                CollectAllGameObjects(container.GetChild(i));
            }
        }
    }

    //void CollectAllRenderers(Transform container)
    //{
    //    if (container.childCount > 0)
    //    {
    //        for (int i = 0; i < container.childCount; i++)
    //        {
    //            if (container.GetChild(i).GetComponent<Renderer>() != null)
    //            {
    //                m_allRenderers.Add(container.GetChild(i).GetComponent<Renderer>());
    //            }

    //            CollectAllRenderers(container.GetChild(i));
    //        }
    //    }
    //}

    // Use this for initialization
    void Start()
    {

        prevPos = transform.position;

        // Build localColliders
        int totalLocalGameObjects = transform.childCount;

        localColliders = new GameObject[totalLocalGameObjects];

        for (int c = 0; c < totalLocalGameObjects; c++)
        {
            localColliders[c] = transform.GetChild(c).gameObject;
        }

        // Set IgnoreCollisions

        if (collisionIgnoreList.Count != 0)
        {
            for (int i = 0; i < collisionIgnoreList.Count; i++)
            {
                for (int l = 0; l < totalLocalGameObjects; l++)
                {
                    if (localColliders[l].GetComponent<Collider>() != null)
                    {
                        Physics.IgnoreCollision(localColliders[l].GetComponent<Collider>(), collisionIgnoreList[i]);
                    }
                }
            }
        }

    }

    // Update is called once per frame
    void Update () {

        float distanceGap = Vector3.Distance(prevPos, transform.position);
        Vector3 directionGap = transform.position - prevPos;

        totalDistanceTravelled += distanceGap;

        //Debug.Log("Distance: " + totalDistanceTravelled);

        if (totalDistanceTravelled >= maxDistance)
        {
            Debug.Log("Max distance reached!");

            DestorySelf();
        }

        CalcPhasing(directionGap, distanceGap);
    }

    void CalcPhasing(Vector3 directionFromPrev, float distanceFromPrev)
    {
        RaycastHit hit;

        // Checks if anything was hit in between physics updates
        if (Physics.Raycast(prevPos, directionFromPrev, out hit, distanceFromPrev))
        {
            // Ignore self
            if (hit.collider.gameObject == this) { return; }

            // Ignore self even more
            for (int i = 0; i < localColliders.Length; i++)
            {
                if (hit.collider.gameObject == localColliders[i])
                {
                    return;
                }
            }

            ProjectileHit(hit.collider);
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        foreach(ContactPoint constact in col.contacts)
        {
            ProjectileHit(constact.thisCollider);
        }
    }

    void ProjectileHit(Collider other)
    {

        Debug.Log("Bullet hit " + other.name + "!");

        DestorySelf();
    }

    void DestorySelf()
    {
        Debug.Log("Deleting projectile...");

        Destroy(gameObject);
    }
}
