using UnityEngine;
using TMPro;
public class PlayerGameManager : MonoBehaviour
{
    int rescued = 0;
    int totalNpc;
    public TextMeshProUGUI rescuedStatus;

    void Start()
    {
        totalNpc = GameObject.FindGameObjectsWithTag("NPC").Length;
    }

    void Update()
    {
        rescuedStatus.text = rescued.ToString()+"/"+totalNpc.ToString();
    }

    public int getNumRescued()
    {
        return rescued;
    }
    public void setNumRescued(int inpt)
    {
        rescued = inpt;
    }
}
