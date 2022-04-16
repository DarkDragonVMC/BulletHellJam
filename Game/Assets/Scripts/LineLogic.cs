using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLogic : MonoBehaviour
{

    private LineRenderer lr;
    private EdgeCollider2D coll;
    private GameObject[] anchors;
    public GameObject anchorPrefab;
    private int value = 2;
    private Transform player;
    private float anglex = 0f;
    private float angley = 2f;
    private Vector3 directionAb;

    // Start is called before the first frame update
    void Start()
    {
        lr = this.GetComponent<LineRenderer>();
        coll = this.GetComponent<EdgeCollider2D>();
        anchors = new GameObject[6];
        player = GameObject.Find("Player").transform;

        //get inital anchors
        for (int i = 0; i < this.transform.childCount; i++)
        {
            anchors[i] = this.transform.GetChild(i).gameObject;
        }

        lr.SetPositions(ObjectsToVector3Array(anchors));

        updateCollision();
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
        value++;
        if (value > 5) value = 0;
        Destroy(anchors[value]);
        GameObject newAnchor = Instantiate(anchorPrefab, Bullet.transform.position, Bullet.transform.rotation);
        anchors[value] = newAnchor;
        rearrange();
        lr.SetPositions(ObjectsToVector3Array(anchors));
        updateCollision();
    }

    private void rearrange()
    {
        GameObject[] points = anchors;

        for (int i = 0; i < points.Length; i++)
        {

            Vector2 first = points[i].transform.position;
            Vector2 second = points[i + 1].transform.position;
            Vector2 origin = player.position;

            if(first == second)
            {
                anchors[i] = points[i];
                anchors[i + 1] = points[i + 1];
                break;
            }

            Vector2 firstOffset = first - origin;
            Vector2 secondOffset = second - origin;

            float angle1 = Mathf.Atan2(firstOffset.x, firstOffset.y);
            float angle2 = Mathf.Atan2(secondOffset.x, secondOffset.y);

            if(angle1 < angle2)
            {
                anchors[i] = points[i];
                anchors[i + 1] = points[i + 1];
                break;
            }

            if(angle1 > angle2)
            {
                anchors[i] = points[i + 1];
                anchors[i + 1] = points[i];
                break;
            }

            if(firstOffset.sqrMagnitude < secondOffset.sqrMagnitude)
            {
                anchors[i] = points[i];
                anchors[i + 1] = points[i + 1];
                break;
            } else
            {
                anchors[i] = points[i + 1];
                anchors[i + 1] = points[i];
                break;
            }
        }
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
}
