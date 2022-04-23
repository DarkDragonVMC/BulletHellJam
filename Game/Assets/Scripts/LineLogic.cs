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

    public float size;
    public float maxSize;
    public Vector2 center;

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
        if(collision.tag == "Player") FindObjectOfType<Oxygen>().inside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") FindObjectOfType<Oxygen>().inside = false;
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
        updateSize(newAnchor);
        if (size > maxSize) limitSize(newAnchor);

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
        
        area.SetPath(0, verticies);
    }

    private Vector2 updateCenter()
    {
        Vector2[] verticies = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            verticies[i] = ObjectsToVector3Array(anchors)[i];
        }

        //move every vertex closer to the center
        center = Vector2.zero;

        foreach (Vector2 corner in verticies)
        {
            center += corner;
        }
        center = center / verticies.Length;
        return center;
    }

    private float updateSize(GameObject newAnchor)
    {
        List<Vector2> list = new List<Vector2>();
        foreach(Vector3 v in ObjectsToVector3Array(anchors))
        {
            list.Add(v);
        }


        float temp = 0;
        int i = 0;
        for(; i < list.Count; i++)
        {
            if(i != list.Count - 1)
            {
                float mulA = list[i].x * list[i + 1].y;
                float mulB = list[i + 1].x * list[i].y;
                temp = temp + (mulA - mulB);
            } else
            {
                float mulA = list[i].x * list[0].y;
                float mulB = list[0].x * list[i].y;
                temp = temp + (mulA - mulB);
            }
        }

        temp *= 0.5f;
        size = Mathf.Abs(temp);
        return size;
    }

    private void limitSize(GameObject newAnchor)
    {
        List<GameObject> toMove = getToMove(newAnchor.transform.position);

        //Move each anchor closer to the center
        while(updateSize(newAnchor) > maxSize)
        {
            foreach(GameObject g in toMove)
            {
                updateCenter();
                g.transform.position = Vector2.MoveTowards(g.transform.position, center, 0.1f);
            }
        }

        lr.SetPositions(ObjectsToVector3Array(anchors));
        updateCollision();
        updateArea();
    }

    private List<GameObject> getToMove(Vector2 compare)
    {
        List<GameObject> toReturn = new List<GameObject>();

        //Get the 3 anchors the are the furthest away from our new anchor
        GameObject tMax = null;
        float maxDist = 0;

        foreach (GameObject g in anchors)
        {
            float dist = Vector2.Distance(g.transform.position, compare);
            if (dist > maxDist)
            {
                tMax = g;
                maxDist = dist;
            }
        }

        toReturn.Add(tMax);

        List<GameObject> copy = new List<GameObject>();
        foreach (GameObject g in anchors) copy.Add(g);
        copy.Remove(tMax);

        //Run again
        tMax = null;
        maxDist = 0;

        foreach (GameObject g in copy)
        {
            float dist = Vector2.Distance(g.transform.position, compare);
            if (dist > maxDist)
            {
                tMax = g;
                maxDist = dist;
            }
        }

        toReturn.Add(tMax);
        copy.Remove(tMax);

        //And again
        tMax = null;
        maxDist = 0;

        foreach (GameObject g in copy)
        {
            float dist = Vector2.Distance(g.transform.position, compare);
            if (dist > maxDist)
            {
                tMax = g;
                maxDist = dist;
            }
        }

        toReturn.Add(tMax);

        return toReturn;
    }

}
