using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLogic : MonoBehaviour
{

    private LineRenderer lr;
    private EdgeCollider2D coll;
    private GameObject[] anchors;
    public GameObject anchorPrefab;
    private int value = 5;
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

        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");

        List<Vector2> copy = new List<Vector2>();
        foreach (Vector3 point in ObjectsToVector3Array(anchors))
        {
            copy.Add(point);
        }

        RaycastHit2D target = new RaycastHit2D();
        while(target.transform == null)
        {
            if (angley == 2) anglex += 0.01f;
            if (anglex == 2) angley -= 0.01f;
            if (angley == -2) anglex -= 0.01f;
            if (anglex == -2) angley += 0.01f;

            Vector3 pointer = new Vector3(player.position.x + anglex, player.position.y + 0.01f + angley, player.position.z);

            Vector3 direction = pointer - player.position;

            target = Physics2D.Raycast(player.position, direction, 10f);
            directionAb = direction;
        }

        anchors[0] = target.transform.gameObject;
        anchors[0].gameObject.layer = LayerIgnoreRaycast;

        for(int i = 1; i <= 6; i++)
        {
            target = new RaycastHit2D();
            while (target.transform == null)
            {
                if (angley == 2) anglex += 0.01f;
                if (anglex == 2) angley -= 0.01f;
                if (angley == -2) anglex -= 0.01f;
                if (anglex == -2) angley += 0.01f;

                Vector3 previous = player.position + directionAb;

                Vector3 pointer = new Vector3(anchors[i - 1].transform.position.x + anglex, anchors[i - 1].transform.position.y + 0.01f + angley, player.position.z);

                Vector3 direction = pointer - anchors[i-1].transform.position;

                target = Physics2D.Raycast(player.position, direction, 10f);
                directionAb = direction;
            }

            anchors[i] = target.transform.gameObject;
            anchors[i].gameObject.layer = LayerIgnoreRaycast;
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
