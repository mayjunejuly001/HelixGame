using UnityEngine;

public class CamController : MonoBehaviour
{

    public BallController target;
    private float offset;

    private void Awake()
    {
        offset = transform.position.y - target.transform.position.y;
    }

    private void Update()
    {
        Vector3 curPos = transform.position;

        curPos.y = target.transform.position.y + offset;
        transform.position = curPos;
    }

}

