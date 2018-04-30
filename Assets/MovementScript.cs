using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

    public float speed = 4F;
    public float jumpHeight = 8F;
    public float fallSpeed = 3F;
    private bool canJump = false;
    private GameObject Plank;
    private GameObject[] Deleteable;
    private long PlankId;
    private long CoinId;
    private GameObject Coin;
    private GameObject ScoreText;
    private int Score;
    private float TimeLoged;
    private bool StartMoving = false;


    // Use this for initialization
    void Start () {
        ScoreText = GameObject.Find("Score");
        Score = 0;
        //ScoreText = Score.ToString();
        Plank = Resources.Load("Prefab\\prefabPlank") as GameObject;
        Coin = Resources.Load("Prefab\\prefabCoin") as GameObject;
        Instantiate(Plank, new Vector3(-1, -2, -1), Quaternion.identity);
        GameObject createdPlank = (GameObject)Instantiate(Plank, new Vector3(3, 0, -1), Quaternion.identity);
        GameObject createdCoin = (GameObject)Instantiate(Coin, new Vector3(3, 0.5F, -1), Quaternion.identity);
        createdCoin.transform.SetParent(createdPlank.transform);
        Instantiate(Plank, new Vector3(-1, 2, -1), Quaternion.identity);
        Instantiate(Plank, new Vector3(Random.Range(-7, 7), 4, -1), Quaternion.identity);
        Instantiate(Plank, new Vector3(Random.Range(-7, 7), 6, -1), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("left"))
        {
            transform.position += Vector3.left * 0.15F;
        }
        if (Input.GetKey("right"))
        {
            transform.position += Vector3.right * 0.15F;
        }
        if (transform.position.y < -6)
        {
            Time.timeScale = 0;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("up") && canJump)
        {
            GetComponent<Rigidbody2D>().velocity += Vector2.up * jumpHeight;
            //GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
        }
        if (StartMoving)
        {
            Deleteable = GameObject.FindGameObjectsWithTag("Respawn");
            foreach (GameObject plnk in Deleteable)
            {
                plnk.transform.Translate(Vector3.down * Time.deltaTime * fallSpeed, Space.World);
                if (plnk.transform.position.y < -5)
                {
                    Destroy(plnk);
                    GameObject createdPlank = (GameObject)Instantiate(Plank, new Vector3(Random.Range(-7, 7), 5, -1), Quaternion.identity);
                    if(Random.value < 0.7)
                    {
                        GameObject createdCoin = (GameObject)Instantiate(Coin,createdPlank.transform.position + new Vector3(0, 0.5F, 0), Quaternion.identity);
                        createdCoin.transform.SetParent(createdPlank.transform);
                    }
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        TimeLoged = Time.time;
        canJump = false;
        transform.SetParent(null);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Air time " + (Time.time - TimeLoged).ToString());
        transform.SetParent(collision.collider.transform);
        if(collision.gameObject.name == "prefabPlank(Clone)" || collision.gameObject.name == "BottomLevel")
        {
            if (collision.gameObject.transform.position == new Vector3(3, 0, -1))
            {
                StartMoving = true;
                Destroy(GameObject.Find("BottomLevel"));
            }
            if (collision.contacts.Length > 0)
            {
                //TODO:
                //ContactPoint2D contact = collision.GetContacts();
                ContactPoint2D contact = collision.contacts[0];
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
                {
                    //collision from bottom
                    canJump = true;
                }
            }
        }
        if (collision.gameObject.name == "prefabCoin(Clone)")
        {
            Score += 1;
            Destroy(collision.gameObject);
        }
    }
}
