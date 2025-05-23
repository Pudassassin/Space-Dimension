using UnityEngine;

public class GroundSighter : MonoBehaviour
{
    public float heightFromGround = 0.2f;

    // Update is called once per frame
    void Update()
    {
        // lock the height of the ground indicator(s)
        Vector3 pos = transform.position;
        pos.y = heightFromGround;
        transform.position = pos;
    }
}
