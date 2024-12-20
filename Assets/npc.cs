using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float speed = 2.0f; // Vitesse de déplacement
    public Sprite[] downSprites;  // Sprites de marche vers le bas
    public Sprite[] upSprites;    // Sprites de marche vers le haut
    public Sprite[] leftSprites;  // Sprites de marche vers la gauche
    public Sprite[] rightSprites; // Sprites de marche vers la droite

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private float animationTimer = 0f;
    private int animationFrame = 0;
    private float animationSpeed = 0.2f; // Temps entre les frames de l'animation

    private Vector2 targetPosition;  // Position cible
    private Vector2 direction;       // Direction du mouvement

    private Map map; // Référence à la carte

    void Start()
    {
        // Initialiser les composants
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.isKinematic = true;

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        // Ajouter un Collider2D au PNJ
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;  // Définir le collider comme trigger (si tu veux une interaction par trigger)

        // Référencer la carte
        map = FindObjectOfType<Map>(); // Trouver la carte dans la scène

        // Positionner le PNJ à une position de départ
        transform.position = map.GetRandomTilePosition();

        // Choisir une première destination aléatoire
        targetPosition = map.GetRandomTilePosition();

        // Définir l'ordre de tri pour le PNJ
        spriteRenderer.sortingOrder = 1;  // S'assurer que le PNJ est devant la carte
    }

    void Update()
    {
        MoveToTarget();
        AnimateNPC();
    }

    void MoveToTarget()
    {
        // Déplacer le PNJ vers la cible
        direction = (targetPosition - (Vector2)transform.position).normalized;

        // Si la différence en Y est plus grande que la différence en X, se déplacer verticalement
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Forcer un mouvement horizontal
            direction.y = 0;
        }
        else
        {
            // Forcer un mouvement vertical
            direction.x = 0;
        }

        // Appliquer la vitesse dans la direction choisie
        rb.velocity = direction * speed;

        // Si le PNJ est proche de la position cible, choisir une nouvelle cible
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = map.GetRandomTilePosition();
        }
    }

    void AnimateNPC()
    {
        // Mettre à jour l'animation toutes les `animationSpeed` secondes
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;

            // Faire défiler les frames
            animationFrame = (animationFrame + 1) % 4; // Modifier selon le nombre de frames

            // Déterminer la direction et assigner le sprite correspondant
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Déplacement horizontal
                spriteRenderer.sprite = direction.x > 0 ? rightSprites[animationFrame] : leftSprites[animationFrame];
            }
            else
            {
                // Déplacement vertical
                spriteRenderer.sprite = direction.y > 0 ? upSprites[animationFrame] : downSprites[animationFrame];
            }
        }
    }

    // Détection de la collision avec le coffre
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Chest"))  // Vérifie si l'objet avec lequel on entre en collision est un coffre
        {
            // Code pour interagir avec le coffre (par exemple, collecter un objet)
            Debug.Log("Le PNJ a trouvé un coffre!");
            // Tu peux aussi désactiver le coffre, changer son état, ou effectuer d'autres actions ici
        }
    }
}
