using UnityEngine;
using System.Collections;

public class LondonTowePoleScript : MonoBehaviour
{
    public int capacity = 5;
    public int id;
    public LondonTowerGameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LondonToweSphereScript>() != null)
        {
            // Debug.Log("sphere hit the pole");
            LondonToweSphereScript sphere = other.gameObject.GetComponent<LondonToweSphereScript>();
            if (sphere != null)
            {
                sphere.EnableXMove(false);
                sphere.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
                sphere.transform.position = new Vector3(this.transform.position.x, sphere.transform.position.y, sphere.transform.position.z);
                sphere.SetLastPolePosition(this.transform.position.x);
                sphere.SetPoleData(id, capacity, LondonTowerGameState.game == LondonTowerGameManager.gameState);
                capacity--;
                gameManager.SetOnTop(sphere);
                if (gameManager.CheckWin())
                {
                    gameManager.GameEnd();
                    // Debug.Log("win");
                }
                else
                {
                    //  Debug.Log("not win");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LondonToweSphereScript>() != null)
        {
            Debug.Log("kulička opustila tyč");
            LondonToweSphereScript sphere = other.gameObject.GetComponent<LondonToweSphereScript>();
            if (sphere != null)
            {
                sphere.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
                sphere.EnableXMove(true);
                capacity++;
            }
        }
    }

    /// <summary>
    /// pole cant add any sphere
    /// </summary>
    /// <returns></returns>
    public bool IsFull()
    {
        return capacity == 0;
    }

    public void SetCapacity(int capacity)
    {
        this.capacity = capacity;
        transform.position = new Vector3(transform.position.x, capacity - 3.7f, transform.position.z);
    }
}
