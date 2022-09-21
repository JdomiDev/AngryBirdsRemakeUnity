using UnityEngine;
using System.Collections;
using Assets.Scripts;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{

    [SerializeField] private AudioClip[] soundListAmbient;
    [SerializeField] private AudioClip[] soundListCollision;
    [SerializeField] private AudioClip[] soundListDie;
    [SerializeField] public AudioClip[] selectSound;
    [SerializeField] private AudioClip[] soundListBoost;

    [SerializeField] private Sprite[] spriteListAmbient;
    [SerializeField] private Sprite[] spriteListThrown;
    [SerializeField] private Sprite[] spriteListCollide;
    [SerializeField] private Sprite[] spriteListBoost;

    [SerializeField] public Collider2D slingShotCol;

    [SerializeField] private ParticleSystem particle;
    
    [SerializeField] public int birdType;
    [SerializeField] public int birdOrder;

    [SerializeField] private GameObject bombParticlePrefab;
    [SerializeField] private GameObject collideParticlePrefab;
    [SerializeField] private GameObject bombExplosionParticlePrefab;
    //[SerializeField] private GameObject particleObject;
    [SerializeField] private GameObject egg;



    [SerializeField] public Color32 color;

    [SerializeField] public Color32 outlineColor;

    [HideInInspector]
    private Vector3 cachedScale;

    [HideInInspector]
    public float BirdColliderRadiusNormal;

    // 0 Red
    // 1 Chuck
    // 2 blues
    // 3 bomb
    // 4 Matilda
    // 5 terence


    public bool Thrown = false;
    public bool Collided = false;
    public bool Boosted = false;
    private bool boost = false;
    public bool Exploded = false;


    [HideInInspector]
    private SpriteRenderer spriteRenderer;

    [HideInInspector]
    private Rigidbody2D rb;

    [HideInInspector]
    private AudioSource audio;

    [HideInInspector]
    private CircleCollider2D collider;


    private bool isAdded = false;

    private void Start()
    {
        if (!gameObject.name.Contains("Clone"))
        {
            BirdColliderRadiusNormal = collider.radius;
            rb.isKinematic = true;
            State = BirdState.BeforeThrown;
            StartCoroutine(animationDelay());
            collider.radius = Constants.BirdColliderRadiusBig;
        }
    }
    private void Awake()
    {

        cachedScale = transform.localScale;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        collider = GetComponent<CircleCollider2D>();

    }

    private void FixedUpdate()
    {

        if (State == BirdState.Thrown && rb.velocity.sqrMagnitude <= Constants.MinVelocity)
        {
            if (birdType != 3)
            {
                StartCoroutine(DestroyAfter(5));
            }
        }
    }

    private void Update()
    {

        //disable collider on inactive birds
        if(gameObject != GameManager.Birds[GameManager.currentBirdIndex] && !Thrown) 
        {
            collider.enabled = false;
            slingShotCol.enabled = false;
        }


        //enable collider
        else
        {
            collider.enabled = true;
            slingShotCol.enabled = true;
        }

        if(GameManager.CurrentGameState == GameState.Won && !Thrown && !Collided && !isAdded)
        {
            isAdded = true;
            GameManager.AliveBirds.Add(gameObject);
        }
        // change collide sprite
        if(Collided) spriteRenderer.sprite = spriteListCollide[0];

        // resets progress
        if(Input.GetKeyDown("m")) PlayerPrefs.DeleteAll();

        // chuck
        if (birdType == 1 && Input.GetMouseButtonDown(0) && Collided == false && State == BirdState.Thrown && Boosted == false) chuck();
        //blues
        if (birdType == 2 && Input.GetMouseButton(0) && Collided == false && State == BirdState.Thrown && Boosted == false) blues();

        // bomb
        if (birdType == 3 && State == BirdState.Thrown && Boosted == false && Input.GetMouseButtonDown(0)) StartCoroutine(ExplosionDamage(2f, 6000f));

        // matilda
        if (birdType == 4 && Input.GetMouseButtonDown(0) && Collided == false && State == BirdState.Thrown && Boosted == false) matilda();
    }


    // bird voids

    private void chuck()
    {
        boost = true;

        Boosted = true;

        rb.velocity *= 2.0f;
        spriteRenderer.sprite = spriteListBoost[0];

        audio.PlayOneShot(soundListBoost[0]);

        particle.Play();

        //GameObject collideParticleObject = Instantiate(collideParticlePrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation);
        //Destroy(collideParticleObject, 1f);
    }

    private void blues()
    {

        boost = true;

        Boosted = true;

        List<GameObject> birdList = new List<GameObject>();

        GameObject duplicate1 = Instantiate(gameObject, new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation) as GameObject;
        GameObject duplicate2 = Instantiate(gameObject, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation) as GameObject;

        birdList.Add(duplicate1);
        birdList.Add(duplicate2);

        foreach (var bird in birdList)
        {
            bird.GetComponent<Bird>().Boosted = true;
            bird.GetComponent<Bird>().OnThrow();
        }

        duplicate1.GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x, rb.velocity.y - 3f);
        duplicate2.GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x, rb.velocity.y + 3f);

        audio.PlayOneShot(soundListBoost[0]);

        //GameObject collideParticleObject = Instantiate(collideParticlePrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation);
        //Destroy(collideParticleObject, 1f);
    }
    private void matilda()
    {
        PathPoints.instance.CreateCurrentPathPoint(transform.position, 4);

        Boosted = true;

        if (rb.velocity.y <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y * -1f) + 20);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y) + 20);
        }
        GameObject eggObject = Instantiate(egg, new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
        spriteRenderer.sprite = spriteListBoost[0];

        AudioPlayer.audio.PlayOneShot(soundListBoost[0]);

        particle.Play();
    }


    // other voids

    public void OnThrow()
    {
        slingShotCol.enabled = false;
        if(gameObject.name.Contains("Clone") == false)
        {
            PathPoints.instance.Clear();
        }
        StartCoroutine(CreatePathPoints());

        spriteRenderer.sprite = spriteListThrown[0];
        audio.Play();
        rb.isKinematic = false;
        collider.radius = BirdColliderRadiusNormal;
        State = BirdState.Thrown;
        Thrown = true;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(birdType == 3)
        {
            if (collision.gameObject.tag == "Brick" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pig" && Thrown)
            {
                StartCoroutine(ExplosionDamage(2f, 6000f));
            }
            rb.gravityScale = 0.5f;

            Collided = true;
        }
        if(collision.gameObject.tag == "Pig" || collision.gameObject.tag == "Brick" || collision.gameObject.tag == "Ground" && Thrown)
        {
            rb.gravityScale = 0.5f;

            spriteRenderer.sprite = spriteListCollide[0];
            audio.PlayOneShot(soundListCollision[Random.Range(0, 8)]);
            if(collision.gameObject.tag != "destroyer")
            {
                particle.Play();
            }
            Collided = true;
        }
    }

    
    private void OnDestroy()
    {
        Exploded = true;
    }



    // enums

    private IEnumerator CreatePathPoints()
    {
        while (true)
        {
            if (Collided) break;

            if (boost)
            {
                PathPoints.instance.CreateCurrentPathPoint(transform.position, 4);
                Boosted = false;
            }
            else
            {
                PathPoints.instance.CreateCurrentPathPoint(transform.position, 1);
            }
            yield return new WaitForSeconds(PathPoints.instance.timeInterval);
        }
    }

    private IEnumerator animationDelay()
    {
        while (1 == 1)
        {
            if (GameManager.CurrentGameState == GameState.Won || GameManager.CurrentGameState == GameState.Won)
            {
                break;
            }
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            if (Thrown == false && Collided == false)
            {
                if (Random.Range(0, 2) == 1)
                {
                    spriteRenderer.sprite = spriteListAmbient[1];
                    audio.PlayOneShot(soundListAmbient[Random.Range(0, 12)]);
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.9f, transform.localScale.z);
                    yield return new WaitForSeconds(0.25f);
                    transform.localScale = cachedScale;
                    spriteRenderer.sprite = spriteListAmbient[0];
                }
                else
                {
                    spriteRenderer.sprite = spriteListAmbient[2];
                    yield return new WaitForSeconds(0.25f);
                    spriteRenderer.sprite = spriteListAmbient[0];
                }
            }
            else if (Thrown == true && Collided == false && Boosted == false && birdType != 3)
            {
                spriteRenderer.sprite = spriteListThrown[0];
            }
            if (GameManager.Birds[GameManager.currentBirdIndex] != gameObject && State == BirdState.BeforeThrown && Random.Range(0, 2) == 1)
            {
                if (rb.velocity.magnitude != 0)
                {
                    gameObject.transform.DOJump(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), 0.3f, 1, 0.3f);
                }
            }
            else if (GameManager.Birds[GameManager.currentBirdIndex] != gameObject && State == BirdState.BeforeThrown)
            {
                if (rb.velocity.magnitude != 0)
                {
                    gameObject.transform.DOJump(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), 0.3f, 1, 0.3f);
                    gameObject.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
                }
            }
        }
    }
    private IEnumerator DestroyAfter(float seconds)
    {
        Exploded = true;
        yield return new WaitForSeconds(seconds);
        AudioPlayer.audio.PlayOneShot(soundListDie[0]);
        GameObject collideParticleObject = Instantiate(collideParticlePrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation);
        Destroy(collideParticleObject, 1f);
        Destroy(gameObject);
    }

    private IEnumerator ExplosionDamage(float radius, float force)
    {
        if (Boosted == false)
        {
            Boosted = true;

            foreach (Sprite sprite in spriteListBoost)
            {
                spriteRenderer.sprite = sprite;

                yield return new WaitForSeconds(0.6f);
            }
            Vector2 center = gameObject.transform.position;

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

            foreach (var hitCollider in hitColliders)
            {

                float distance = Vector2.Distance(hitCollider.gameObject.transform.position, gameObject.transform.position);

                if (hitCollider.gameObject.tag == "Pig")
                {
                    Destroy(hitCollider.gameObject);
                    if (rb != null)
                    {
                        //Rigidbody2DExtension.AddExplosionForce(hitCollider.GetComponent<Rigidbody2D>(), force, gameObject.transform.position, radius + 2f);
                    }
                }
                if (hitCollider.gameObject.tag == "Brick")
                {
                    hitCollider.gameObject.GetComponent<Brick>().currentHealth = hitCollider.gameObject.GetComponent<Brick>().currentHealth - (200 - (distance * 100));
                    if (rb != null)
                    {
                        Rigidbody2DExtension.AddExplosionForce(hitCollider.GetComponent<Rigidbody2D>(), force - (distance * 1000), gameObject.transform.position, radius + 4f);
                    }
                }
            }
            AudioPlayer.audio.PlayOneShot(soundListBoost[0]);
            GameObject bombParticleObject = Instantiate(bombParticlePrefab, transform.position, transform.rotation);
            bombParticleObject.GetComponent<ParticleSystem>().Play();
            Destroy(bombParticleObject, 1f);

            GameObject explosionParticleObject = Instantiate(bombExplosionParticlePrefab, transform.position, transform.rotation);
            explosionParticleObject.GetComponent<ParticleSystem>().Play();
            Destroy(explosionParticleObject, 1f);


            Destroy(gameObject);
        }
    }









    public BirdState State
    {
        get;
        private set;
    }
}
