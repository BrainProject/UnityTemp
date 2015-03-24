using UnityEngine;
using System.Collections;

/// <summary>
/// Spehre gameobject script
/// </summary>
public class LondonToweSphereScript : MonoBehaviour, System.IComparable<LondonToweSphereScript>
{

    private Vector3 lastPosition;
    private bool clicked = false;
    private bool lastClicked = false;
    private bool moveX = true;
    private float lastPolePolesition;
    public LondonTowerGameManager gameManager;

    //ratio of moving spehre by mouse - to scene
    private float moveConstant = (Screen.height / 9.3f);
    public bool start = true;
    public string idColor;
    public int currentPoleID;
    public int orderOnPole;

    // Update is called once per frame
    void Update()
    {
        if (transform.rigidbody.velocity.y > 0)
        {
            transform.rigidbody.velocity = new Vector3();
        }
        if (transform.position.y <= 0)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 6, this.transform.position.z);
        }
        if (clicked)
        {
            Vector3 distance = Input.mousePosition - lastPosition;

            if (moveX)
            {
                //9,-1 are bordes when sphere can be moved
                this.transform.position = new Vector3(Mathf.Min(9, Mathf.Max(-1, this.transform.position.x + distance.x / moveConstant)), this.transform.position.y + distance.y / moveConstant, this.transform.position.z);
            }
            else
            {
                //9,-1 are bordes when sphere can be moved + nax can only move down/up becaus id on the pole
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + distance.y / moveConstant, this.transform.position.z);
            }
            lastPosition = Input.mousePosition;
            if (lastClicked)
            {
                lastClicked = !lastClicked;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clicked = false;
                    this.rigidbody.useGravity = true;
                }
            }
        }
        else
        {
            int xPosition = (((int)this.transform.position.x) / 4 * 4);
            float rozdil = this.transform.position.x - xPosition;
            if (rozdil > 2)
            {
                xPosition = xPosition + 4;
            }
            if (gameManager == null)
            {
                Debug.Log("no game manager, add gammanager in editor");
            }

            if (moveX && gameManager.IsPoleFull(xPosition))
            {
                // Debug(xPosition);
                Debug.Log(lastPolePolesition);
                //    Debug.Log(currentPoleID + "poleID");
                this.transform.position = (new Vector3((currentPoleID - 1) * 4, this.transform.position.y + 1, this.transform.position.z));
            }
            else
            {
                this.transform.position = (new Vector3(xPosition, this.transform.position.y, this.transform.position.z));
            }
        }
    }

    void OnMouseDown()
    {
        if (LondonTowerGameManager.gameState == LondonTowerGameState.game)
        {
            if (gameManager.OnTop(currentPoleID, orderOnPole) && !clicked)
            {
                clicked = true;
                lastPosition = Input.mousePosition;
                this.rigidbody.useGravity = false;
                lastClicked = true;
            }
        }
    }

    public void EnableXMove(bool enableX)
    {
        moveX = enableX;
    }

    /// <summary>
    /// set position of last pole,where sphere was
    /// </summary>
    /// <param name="polePosition"></param>
    public void SetLastPolePosition(float polePosition)
    {
        this.lastPolePolesition = polePosition;
    }

    /// <summary>
    /// return spehre to last pole
    /// </summary>
    /// <param name="poleHeight"> how big pole is, so spehre can be move directly on top of pole</param>
    public void BackToLastPole(float poleHeight)
    {
        this.transform.position = new Vector3(lastPolePolesition, poleHeight, this.transform.position.z);
    }

    /// <summary>
    /// set game data about sphere, pole id, order on pole and if game 
    /// </summary>
    /// <param name="poleID"></param>
    /// <param name="orderOnPole"></param>
    /// <param name="game"></param>
    public void SetPoleData(int poleID, int orderOnPole, bool game)
    {
        currentPoleID = poleID;
        if (start)
        {
            start = false;
        }
        else
        {
            this.orderOnPole = orderOnPole;
        }
    }

    /// <summary>
    /// set color/material
    /// </summary>
    /// <param name="id"></param>
    /// <param name="color"></param>
    public void setIdColor(string id, Color color)
    {
        this.idColor = id;
        renderer.material.color = color;
    }

    public int CompareTo(LondonToweSphereScript other)
    {
        if (other == null) return 1;
        return orderOnPole.CompareTo(other.orderOnPole);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LondonTowePoleScript>() != null)
        {
            //Debug.Log(" sphere is hitted by pole");
            if (clicked)
            {
                clicked = false;
                this.rigidbody.useGravity = true;
            }
        }
    }
}
