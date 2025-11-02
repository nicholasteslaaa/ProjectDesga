using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject position;
    public GameObject[] location;
    public int idx = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (Transform child in location[idx].transform)
            {
                child.GetComponent<Animator>().Play("Hiding");
            }
            idx += 1;
            location[idx].SetActive(true);
            location[idx - 1].SetActive(false);
            other.transform.position = position.transform.position;
        }
    }
}
