using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Transform farBG, middleBG;
    public float minHeight, maxHeight;

    private Vector2 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }


    void Update()
    {
        //transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);

        transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

        farBG.position = farBG.position + new Vector3(amountToMove.x, amountToMove.y, 0f);
        middleBG.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;

        lastPos = transform.position;
    }
}
