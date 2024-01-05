using UnityEngine;

public class GhostTrailEffect : MonoBehaviour
{
    public float spawnInterval = 1.0f;
    public float lifeTime = 5.0f;
    private bool isActivated = false;
    public float effectTime;
    Player playerScript;

    private void Start()
    {
        InvokeRepeating("CreateGhostTrail", 0f, spawnInterval);
        playerScript = GetComponent<Player>();
    }

    private void Update()
    {
        if (isActivated)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0f)
            {
                isActivated = false;
                lifeTime = 0f;
            }
        }

        

    }

    public void ActivateGhostTrail()
    {
        isActivated = true;
        lifeTime = effectTime;
    }

    private void CreateGhostTrail()
    {
        if (isActivated)
        {
            SpriteRenderer ghostTrail = CreateGhostSprite();
            Destroy(ghostTrail.gameObject, lifeTime);
        }
    }

    private SpriteRenderer CreateGhostSprite()
    {
        GameObject ghostTrailObject = new GameObject("GhostTrail");
        
        SpriteRenderer ghostTrailRenderer = ghostTrailObject.AddComponent<SpriteRenderer>();
        ghostTrailRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        ghostTrailRenderer.color = new Color(1f, 1f, 1f, 0.6f);
        ghostTrailRenderer.flipX = transform.localScale.x < 0;

        ghostTrailObject.transform.position = transform.position;
        ghostTrailObject.transform.rotation = transform.rotation;

        return ghostTrailRenderer;
    }
}
