using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textScore;
    [SerializeField]
    private TMP_Text textBest;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textBest.text = "Best Score : " + GameManager.singleton.best;
        textScore.text = GameManager.singleton.score.ToString();

    }
}
