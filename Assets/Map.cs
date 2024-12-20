using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Dimensions de la map
    public int mapWidth = 10; 
    public int mapHeight = 10; 

    // Sprites pour les tuiles
    public Sprite grassSprite;
    public Sprite dirtSprite; 
    public Sprite chestSprite; 

    // Taille des tuiles
    public float tileSize = 1.0f;

    // Matrice pour stocker les tuiles
    private GameObject[,] tiles;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        tiles = new GameObject[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // Créer une tuile (GameObject) pour chaque position
                GameObject tile = new GameObject($"Tile_{x}_{y}");
                tile.transform.position = new Vector2(x * tileSize, y * tileSize);
                tile.transform.parent = this.transform;

                // Ajouter un SpriteRenderer
                SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();

                // Choisir le sprite aléatoirement
                renderer.sprite = (Random.value > 0.5f) ? grassSprite : dirtSprite;

                // Stocker la tuile
                tiles[x, y] = tile;
            }
        }

        PlaceChest();
    }

    void PlaceChest()
    {
        // Sélectionner une position aléatoire
        int chestX = Random.Range(0, mapWidth);
        int chestY = Random.Range(0, mapHeight);

        // Récupérer la tuile correspondante
        GameObject tile = tiles[chestX, chestY];

        // Créer un GameObject pour le coffre
        GameObject chest = new GameObject($"Chest_{chestX}_{chestY}");
        chest.transform.position = tile.transform.position; // Même position que la tuile
        chest.transform.parent = tile.transform;           // Attaché comme enfant de la tuile

        // Ajouter un SpriteRenderer au coffre
        SpriteRenderer renderer = chest.AddComponent<SpriteRenderer>();
        renderer.sprite = chestSprite; // Sprite du coffre

        // Ajouter un BoxCollider2D pour permettre la détection de collision
        BoxCollider2D collider = chest.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Marquer comme trigger pour détecter les collisions avec le PNJ

        // Facultatif : Ajuster l'ordre de tri pour que le coffre soit visible
        renderer.sortingOrder = 1; // Au-dessus de la tuile
    }


    // Fonction pour obtenir une position aléatoire dans la carte (en termes de coordonnées de tuiles)
    public Vector2 GetRandomTilePosition()
    {
        // Générer des indices aléatoires dans les limites de la carte
        int randomX = Random.Range(0, mapWidth);
        int randomY = Random.Range(0, mapHeight);

        // Retourner la position des tuiles, ajustée pour tenir compte de la taille des tuiles
        return new Vector2(randomX * tileSize, randomY * tileSize);
    }
}
