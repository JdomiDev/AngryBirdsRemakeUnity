using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Pig : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundListAmbient;
    [SerializeField] private AudioClip[] soundListCollision;
    [SerializeField] private AudioClip[] soundListDie;
    [SerializeField] private Sprite[] spriteListDamaged;
    [SerializeField] private int dieScore;
    [SerializeField] private GameObject dieText;

    //[SerializeField] private GameObject dieText;

    public float Health;

    [HideInInspector]
    public float currentHealth;

    Score score;

    [HideInInspector]
    private SpriteRenderer spriteRenderer;

    private AudioSource audio;

    private Vector3 velocityBeforePhysicsUpdate;

    Rigidbody2D rb;

    private void Update()
    {
        if (currentHealth <= 0)
        {
            die();
        }
    }
    private void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rb.velocity;
    }

    private void Awake()
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        score = GameObject.Find("number").GetComponent<Score>();
        score.maxScoreUpdate(dieScore);

        rb = GetComponent<Rigidbody2D>();

        audio = GetComponent<AudioSource>();


        currentHealth = Health;

        StartCoroutine(animationDelay());
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        if (GameTime.gameTime == 0) return;

        if (col.gameObject.tag == "Bird") die();

        else
        {

            float damage = 0;
            if(col.gameObject.tag == "Pig")
            {
                if (col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 1)
                {
                    damage = velocityBeforePhysicsUpdate.magnitude * 8;
                }
                else
                {
                    damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 8;
                }
            }
            else if (col.gameObject.tag == "Brick")
            {
                if(col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 1)
                {
                    damage = velocityBeforePhysicsUpdate.magnitude * 8;
                }
                else
                {
                    damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 8;
                }
            }
            else if (col.gameObject.tag == "Ground")
            {
                damage = velocityBeforePhysicsUpdate.magnitude * 8;
            }




            currentHealth -= damage;
            if (damage >= 10)
            {
                GetComponent<AudioSource>().PlayOneShot(soundListCollision[Random.Range(0, 16)]);
            }

            if (currentHealth == Health || (currentHealth / Health) * 100 >= 95f)
            {
                spriteRenderer.sprite = spriteListDamaged[0];
            }
            else if ((currentHealth / Health) * 100 >= 60f)
            {
                spriteRenderer.sprite = spriteListDamaged[2];
            }
            else
            {
                spriteRenderer.sprite = spriteListDamaged[4];
            }

            if (currentHealth <= 0)
            {
                die();
            }
        }
    }



    private void die()
    {
        AudioPlayer.audio.PlayOneShot(soundListDie[0], 2.0f);
        score.scoreUpdate(dieScore);

        GameObject DieText = Instantiate(dieText, new Vector2(transform.position.x, transform.position.y + 1f), new Quaternion(0,0,0,0));
        DieText.GetComponent<ScoreText>().animateStart(dieScore, 10 ,new Color32(0,153,25,255), new Color32(0,0,0,255));

        Destroy(gameObject);
    }
    private IEnumerator animationDelay()
    {
        while (1 == 1)
        {
            if (GameManager.CurrentGameState == GameState.Won || GameManager.CurrentGameState == GameState.Won)
            {
                break;
            }
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            if (currentHealth == Health || (currentHealth / Health) * 100 >= 95f)
            {
                //spriteRenderer.sprite = spriteListAmbient[Random.Range(0, 3)];
                spriteRenderer.sprite = spriteListDamaged[1];
                audio.PlayOneShot(soundListAmbient[Random.Range(0, 10)]);
                yield return new WaitForSeconds(0.25f);
                spriteRenderer.sprite = spriteListDamaged[0];
            }
            else if((currentHealth / Health) * 100 >= 60f)
            {
                spriteRenderer.sprite = spriteListDamaged[3];
                audio.PlayOneShot(soundListAmbient[Random.Range(0, 10)]);
                yield return new WaitForSeconds(0.25f);
                spriteRenderer.sprite = spriteListDamaged[2];
            }
            else
            {
                spriteRenderer.sprite = spriteListDamaged[5];
                audio.PlayOneShot(soundListAmbient[Random.Range(0, 10)]);
                yield return new WaitForSeconds(0.25f);
                spriteRenderer.sprite = spriteListDamaged[4];
            }
        }
    }
}
