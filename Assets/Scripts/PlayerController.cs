using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public int playerIndex;

    private Transform respawnPoint;
    private MenuController menuController;
    private ScoreHandler scoreHandler;
    private AudioSource pop;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = GameObject.Find("RespawnPoint").transform;
        menuController = GameObject.Find("Canvas").GetComponent<MenuController>();
        scoreHandler = GameObject.Find("Canvas/CountPanel").GetComponent<ScoreHandler>();
        pop = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
    }

    private void Update()
    {
        if(transform.position.y < -10)
        {
            Respawn();
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        menuController.AddCountText(playerIndex, count);
        if(scoreHandler.Score >= 12)
        {
            //winTextObject.SetActive(true);
            menuController.WinGame();
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            scoreHandler.Score += 1;
            pop.Play();

            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //Respawn();
            EndGame();
        }
    }

    void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        transform.position = respawnPoint.position;
    }

    void EndGame()
    {
        menuController.LoseGame();
        gameObject.SetActive(false);
    }
}
