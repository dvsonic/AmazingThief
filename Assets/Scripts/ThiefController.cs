using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ThiefController : MonoBehaviour {

	// Use this for initialization
    public enum JUMP_STATE { FALL_INIT,ON_GROUND,FIRST_JUMP,SECOND_JUMP,DEAD};
    private JUMP_STATE jumpState;
    public float speed;
    public float jumpSpeed;
    public float jumpSpeed2;
    public BlockFactory[] factoryList;
    public GameObject endlessBlock;
    public GameObject guide;
    public AudioSource run;
    public AudioSource jump;
    public AudioSource dead;
    public AudioSource bonus;
    public AudioSource ground;
	void Start () {
        jumpState = JUMP_STATE.FALL_INIT;
        _isFirstJump = false;
        _hitList = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (jumpState)
            {
                case JUMP_STATE.ON_GROUND:
                case JUMP_STATE.FIRST_JUMP:
                case JUMP_STATE.FALL_INIT:
                    Jump();
                    break;
            }
        }

#else
        if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
        {
            switch (jumpState)
            {
            case JUMP_STATE.ON_GROUND:
            case JUMP_STATE.FIRST_JUMP:
            case JUMP_STATE.FALL_INIT:
                    Jump();
                    break;
            }
        }
#endif
    }

    void FixedUpdate()
    {
        if (jumpState != JUMP_STATE.FALL_INIT && jumpState != JUMP_STATE.DEAD)
        {
            gameObject.transform.position += new Vector3(speed * Time.deltaTime, 0);
            updateBG();
        }
        if (!_isFirstJump)
            endlessBlock.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }
    public Transform bgFar;
    public Transform bgNear;
    public Transform bgFront;
    private void updateBG()
    {
        if (bgFar)
            bgFar.position -= new Vector3(0.1f * speed * Time.deltaTime, 0, 0);
        if (bgNear)
            bgNear.position -= new Vector3(0.2f * speed * Time.deltaTime, 0, 0);
        if (bgFront)
            bgFront.position -= new Vector3(0.6f * speed * Time.deltaTime, 0);
    }

    private bool _isFirstJump;
    private void Jump()
    {
        if (run)
            run.Stop();
        if (jump)
            jump.Play();
        if (!_isFirstJump)
        {
            _isFirstJump = true;
            if (endlessBlock)
                endlessBlock.SendMessage("StopScroll");
            if (guide)
                guide.SetActive(false);
        }
       
        if (jumpState == JUMP_STATE.ON_GROUND || jumpState == JUMP_STATE.FALL_INIT)
        {
            gameObject.rigidbody2D.velocity = new Vector2(0, jumpSpeed);
            jumpState = JUMP_STATE.FIRST_JUMP;
            GetComponent<Animator>().SetBool("isJump", true);
        }
        else if (jumpState == JUMP_STATE.FIRST_JUMP)
        {
            gameObject.rigidbody2D.velocity = new Vector2(0, jumpSpeed2);
            jumpState = JUMP_STATE.SECOND_JUMP;
            GetComponent<Animator>().SetBool("isJump", true);
        }
    }
    private List<GameObject> _hitList;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (jumpState == JUMP_STATE.FALL_INIT)
        {
            if (run && !run.isPlaying)
                run.Play();
            return;
        }
        if(coll.transform.parent && coll.transform.parent.gameObject == endlessBlock)
        {
            if (ground)
                ground.Play();
            if (run && !run.isPlaying)
                run.Play();
            GetComponent<Animator>().SetBool("isJump", false);
            jumpState = JUMP_STATE.ON_GROUND;
            if (_hitList.IndexOf(endlessBlock) < 0)
            {
                foreach (BlockFactory bf in factoryList)
                {
                    bf.CreatBlock();
                }
                _hitList.Add(endlessBlock);
            }

        }
        if(coll.gameObject.tag == "BottomBlock")
        {
            if (ground)
                ground.Play();
            if (run && !run.isPlaying)
                run.Play();
            GetComponent<Animator>().SetBool("isJump", false);
            if (coll.contacts[0].normal.Equals(new Vector2(0, 1)))
            {
                if (_hitList.IndexOf(coll.gameObject) < 0)
                {
                    jumpState = JUMP_STATE.ON_GROUND;
                    foreach (BlockFactory bf in factoryList)
                    {
                        bf.CreatBlock();
                    }
                    _hitList.Add(coll.gameObject);
                }

            }
            else
            {
                if (dead)
                    dead.Play();
                GetComponent<Animator>().SetBool("isDead", true);
                jumpState = JUMP_STATE.DEAD;
                speed = 0;
                //gameObject.rigidbody2D.GetComponent<BoxCollider2D>().enabled = false;
                //gameObject.rigidbody2D.velocity = new Vector2(0, -5);
                Camera.main.GetComponent<CameraShake>().Shake();
                StartCoroutine(GameEnd());
            }

        }
    }

     void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "BottomBlock")
        {
            if (run)
                run.Stop();
        }
        if (coll.transform.parent && coll.transform.parent.gameObject == endlessBlock)
        {
            if (run)
                run.Stop();
         }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Banana")
        {
            if (bonus)
                bonus.Play();
            GameData.score++;
            coll.gameObject.GetComponent<Animator>().SetBool("isBom", true);
        }
    }

    private IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(1.0f);
        Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas)
            canvas.SendMessage("DestroyAD", SendMessageOptions.DontRequireReceiver);
        Application.LoadLevel("Result");
    }
}
