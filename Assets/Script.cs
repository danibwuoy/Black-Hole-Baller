using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject level;
    public GameObject image;
    private Vector3 gravity;
    public Rigidbody2D rigidbody;
    private bool isLevelFinished;
    private bool isDead;
    private Transform finish;

    [Range(0, 250)]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        gravity = Physics.gravity;
        rigidbody = GetComponent<Rigidbody2D>();
        isLevelFinished = false;
        isDead = false;
        //this.rigidbody.gravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera)
        {
            Vector3 vector3 = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 10f);
            //image.transform.position = new Vector3(vector3.x, vector3.y, -1);
            mainCamera.transform.position = vector3;
        }

        if (!isLevelFinished && !isDead)
        {
            HandleControls();
        }

        if (finish)
        {
            //Vector3 scale = new Vector3(0.1f, 0.1f, 0);
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(0.1f, 0.1f, 0), Time.deltaTime);
            this.transform.position = Vector3.Lerp(this.transform.position, finish.position, Time.deltaTime);

            if (this.transform.localScale.x <= 0.4f && this.transform.localScale.y <= 0.4f)
            {
                if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }

                else if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1)
                {
                    SceneManager.LoadScene(0);
                }
            }

        }

        else if (isDead)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(0.1f, 0.1f, 0), Time.deltaTime);
            // this.transform.position = Vector3.Lerp(this.transform.position, finish.position, Time.deltaTime);

            if (this.transform.localScale.x <= 0.4f && this.transform.localScale.y <= 0.4f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void HandleControls()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 direction = level.transform.forward;
            level.transform.RotateAround(this.transform.position, direction, speed * Time.deltaTime);
            Invoke("DisableGravity", 0.5f);
            //DisableGravity();
            //Physics2D.gravity = Vector2.zero;
            //this.rigidbody.gravityScale = 0f;
            //Invoke("DisableGravity", 0.5f);

            return;

            //Vector3 destination = this.transform.rotation.eulerAngles - new Vector3(0, 0, 5);
            //gameObject.transform.Rotate(Vector3.left * Time.deltaTime, Space.World);
            //gameObject.transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, destination, Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 direction = -1 * level.transform.forward;
            //level.transform.RotateAround(this.transform.position, direction, speed * Time.deltaTime);
            level.transform.RotateAround(this.transform.position, direction, speed * Time.deltaTime);
            Invoke("DisableGravity", 0.5f);
            //DisableGravity();
            //Invoke("DisableGravity", 0.5f);
            //Physics2D.gravity = Vector2.zero;
            //this.rigidbody.gravityScale = 0f;

            return;
            //Vector3 destination = this.transform.rotation.eulerAngles + new Vector3(0, 0, 5);
            //gameObject.transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, destination, Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            CancelInvoke("DisableGravity");
        }


        //Physics2D.gravity = Vector2.one;
        //this.rigidbody.gravityScale = 0.5f;
        //rigidbody.constraints = RigidbodyConstraints2D.None;
        EnableGravity();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLevelFinished)
        {
            if (collision.gameObject.tag == "DeathCollider")
            {
                Debug.Log("DeathCollider");
                isDead = true;
                FreezePositions();
                //SceneManager.LoadScene(0);
                //Application.loadedLevel();
                return;
            }


            rigidbody.constraints = RigidbodyConstraints2D.None;
            EnableGravity();
            CancelInvoke();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Finish")
        {
            Debug.Log("Finish");
            finish = col.transform;

            isLevelFinished = true;
            FreezePositions();
            //Application.loadedLevel();
            return;
        }

        rigidbody.constraints = RigidbodyConstraints2D.None;
        EnableGravity();
        CancelInvoke();
    }

    void EnableGravity()
    {
        rigidbody.constraints = RigidbodyConstraints2D.None;
        //Physics2D.gravity = gravity;
    }

    void DisableGravity()
    {
        //Physics2D.gravity = new Vector2(0.0f, 0.01f);
        FreezePositions();
    }

    void FreezePositions()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }
}