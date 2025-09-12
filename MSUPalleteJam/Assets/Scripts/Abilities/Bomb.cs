using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionTime;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private LayerMask _targets;
    [SerializeField] private LayerMask _ignorables; 


    private void Awake()
    {
        Invoke(nameof(Explode), _explosionTime); 
    }




    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position, _explosionRadius, _targets); 

        foreach(Collider2D hit in hits)
        {
            Vector2 direction = ((Vector2)(hit.transform.position - transform.position)).normalized;
            float dist = Vector3.Distance(transform.position, hit.transform.position);

            if (Physics.Raycast((Vector2)transform.position, direction, dist, ~(_targets | _ignorables))) continue; // something is in the way of the blast
            else
            {
                Explodable explodeScript = hit.GetComponent<Explodable>();

                if (explodeScript == null)
                {
                    Debug.LogError("ERROR, EXPLOSION HIT EXPLODABLE OBJECT YET NO EXPLODE SCRIPT WAS ATTATCHED, REFUSING TO EXPLODE OBJ...");
                    continue;
                }
                else
                {
                    explodeScript.Explode();
                    continue;
                }
            }
        }
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _explosionRadius);


    }
}
