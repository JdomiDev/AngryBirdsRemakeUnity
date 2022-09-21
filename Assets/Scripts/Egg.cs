using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] public AudioClip[] clip;
    [SerializeField] public GameObject collideParticlePrefab;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Bird")
        {
            explode(this.gameObject.transform.position, 2f);
        }
    }

    private void explode(Vector2 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center ,radius);

        foreach(var hitCollider in hitColliders)
        {
            float distance = Vector2.Distance(hitCollider.gameObject.transform.position, gameObject.transform.position);

            if (hitCollider.gameObject.tag == "Pig")
            {
                hitCollider.gameObject.GetComponent<Pig>().currentHealth = hitCollider.gameObject.GetComponent<Pig>().currentHealth - 200 + (distance * 10);
            }
            if (hitCollider.gameObject.tag == "Brick")
            {
                hitCollider.gameObject.GetComponent<Brick>().currentHealth = hitCollider.gameObject.GetComponent<Brick>().currentHealth - 200 + (distance * 10);
            }
        }

        AudioPlayer.audio.PlayOneShot(clip[0]);

        GameObject collideParticleObject = Instantiate(collideParticlePrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation);

        Destroy(collideParticleObject, 1f);

        Destroy(gameObject);
    }
}
