using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLogic : MonoBehaviour
{

    private LineRenderer lr;
    private EdgeCollider2D coll;
    private PolygonCollider2D area;
    private GameObject[] anchors;
    public GameObject anchorPrefab;
    private Transform player;

    public float thickness;

    // Start is called before the first frame update
    void Start()
    {
        lr = this.GetComponent<LineRenderer>();
        coll = this.GetComponent<EdgeCollider2D>();
        area = this.GetComponent<PolygonCollider2D>();
        anchors = new GameObject[6];
        player = GameObject.Find("Player").transform;

        //get inital anchors
        for (int i = 0; i < this.transform.childCount; i++)
        {
            anchors[i] = this.transform.GetChild(i).gameObject;
        }

        lr.SetPositions(ObjectsToVector3Array(anchors));

        updateCollision();
        updateArea();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        FindObjectOfType<Oxygen>().loseOxygen();
    }

    public Vector3[] ObjectsToVector3Array(GameObject[] input)
    {
        Vector3[] output = new Vector3[input.Length + 1];
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = input[i].transform.position;
        }

        output[input.Length] = output[0];

        return output;
    }

    public void updateAnchor(GameObject Bullet)
    {
        GameObject newAnchor = Instantiate(anchorPrefab, Bullet.transform.position, Bullet.transform.rotation);
        Destroy(anchors[nearest(newAnchor)]);
        anchors[nearest(newAnchor)] = newAnchor;
        lr.SetPositions(ObjectsToVector3Array(anchors));
        updateCollision();
        updateArea();

        FindObjectOfType<AudioManager>().Play("Blitz");

        Destroy(Bullet);
    }

    private int nearest(GameObject na)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector2 compare = na.transform.position;

        foreach(GameObject g in anchors)
        {
            float dist = Vector2.Distance(g.transform.position, compare);
            if(dist < minDist)
            {
                tMin = g;
                minDist = dist;
            }
        }

        List<GameObject> list = new List<GameObject>();

        foreach (GameObject g in anchors) list.Add(g);

        return list.IndexOf(tMin);
    }

    private void updateCollision()
    {
        List<Vector2> edges = new List<Vector2>();
        foreach (Vector3 point in ObjectsToVector3Array(anchors))
        {
            edges.Add(point);
        }

        coll.SetPoints(edges);
    }

    private void updateArea()
    {
        Vector2[] verticies = new Vector2[6];
        for(int i = 0; i < 6; i++)
        {
            verticies[i] = ObjectsToVector3Array(anchors)[i];
        }

        //move every vertex closer to the center
        Vector2 center = Vector2.zero;

        foreach(Vector2 corner in verticies)
        {
            center += corner;
        }
        center = center / verticies.Length;

        for (int i = 0; i < 6; i++)
        {
            Vector2 corner = verticies[i];
            verticies[i] = Vector2.MoveTowards(corner, center, thickness);
        }
        
        area.SetPath(0, verticies);
    }
}
