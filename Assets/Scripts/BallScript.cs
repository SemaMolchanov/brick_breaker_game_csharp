using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    public Rigidbody2D rigidBody;
    public bool inPlay;
    public Transform paddle;
    public float speed;
    public Transform explosion;
    public Transform powerup;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameOver){
            return;
        }
        if (!inPlay){
            transform.position = paddle.position;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !inPlay){
            inPlay = true;
            rigidBody.AddForce(Vector2.up * speed);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Bottom")){
            Debug.Log("Ball hit the bottom of the screen");
            rigidBody.velocity = Vector2.zero;
            inPlay = false;
            gameManager.UpdateLives(-1);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.transform.CompareTag("Brick")){
            BrickScript brickScript = other.gameObject.GetComponent<BrickScript>();
            if (brickScript.hitsToBreak > 1){
                brickScript.BreakBrick();
            } else{
                int randomChance = Random.Range(1, 100 + 1);
                if (randomChance < 25){
                    Instantiate(powerup, other.transform.position, other.transform.rotation);
                }
                Transform newExplosion = Instantiate(explosion, other.transform.position, other.transform.rotation);
                Destroy(newExplosion.gameObject, 2.5f);
                gameManager.UpdateScore(brickScript.points);
                gameManager.UpdateNumberOfBricks();
                Destroy(other.gameObject);
            }
        }
    }
}
