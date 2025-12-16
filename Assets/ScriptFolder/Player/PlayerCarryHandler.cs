using UnityEngine;


public class PlayerCarryHandler : MonoBehaviour
{
    public GameObject[] npcFollowed = { null, null, null };
    GameObject npcCarried = null;
    public PlayerComponentManager playerComponentManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setFollowed(GameObject npc)
    {
        for (int i = 0; i < npcFollowed.Length; i++)
        {
            if (npcFollowed[i] != null)
            {
                if (npcFollowed[i].GetInstanceID() == npc.GetInstanceID())
                {
                    return;
                }
            }
        }

        for (int i = 0; i < npcFollowed.Length; i++)
        {
            if (npcFollowed[i] == null)
            {
                npcFollowed[i] = npc;
                return;
            }
        }

    }

    public void unsetFollowed(GameObject npc)
    {
        for (int i = 0; i < npcFollowed.Length; i++)
        {
            if (npcFollowed[i] != null)
            {
                if (npcFollowed[i].GetInstanceID() == npc.GetInstanceID())
                {
                    npcFollowed[i] = null;
                }
            }
        }
    }

    public int getNumberOfFollowedNPC()
    {
        int numOfNPCs = 0;

        for (int i = 0; i < npcFollowed.Length; i++)
        {
            if (npcFollowed[i] != null)
            {
                numOfNPCs += 1;
            }
        }

        return numOfNPCs;
    }

    public GameObject[] getNpcsFollowed()
    {
        return npcFollowed;
    }

    public void rescued(GameObject rescuedNPC)
    {
        PlayerGameManager player = playerComponentManager.getPlayerGameManager();
        player.setNumRescued(player.getNumRescued() + 1);
        GameObject delNpc = rescuedNPC.gameObject;
        unsetFollowed(rescuedNPC);
        Destroy(delNpc);
    }
    
    

}
