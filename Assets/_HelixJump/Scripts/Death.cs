using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void HitDeathPart()
    {

        if (GameManager.singleton.score == 0)
        {
            return;
        }
        //GameManager.singleton.RestartLevel();
        GameManager.singleton.ShowGameOver();
    }
}
