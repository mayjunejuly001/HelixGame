using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void HitDeathPart()
    {
        GameManager.singleton.RestartLevel();
    }
}
