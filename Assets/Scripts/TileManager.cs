using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using Utility;
using Utils;

/*
 * TileManager.cs
 *
 * Author  : Thomas Norman
 * Created : April 2013
 * Version : 1.0
 *
 * NOTE: This code is not great, it originated as a result of a proof of concept.  So if
 *       for some reason this has made it into the game and you're reading this message,
 *       and you're not a programmer on the team, I apologise for its quality!  And if you
 *       are on team royale... feel free to sex it up and delete this note if I don't get
 *       around to it first. - Tom
 *
 * This class, when attached to a game object in a scene, will generate tiled terrain using
 * tiled geometry.  The "tiles" are models that must follow several rules.
 *
 * Rule 1.
 *    The tiles must all have square bases and their widths must be uniform across all
 *    tiles, ie.
 *          for all tiles t1,t2: t1.x == t1.z and t1.x = t2.x
 *          (x and z are width and length, y (not mentioned) is height)
 *
 * The y axis, height, should be split into two different considerations: "total height",
 * which is exactly what it sounds like, and "floor height", which is the distance from
 * the bottom of the tile to the "surface" of the tile that the player is expected to
 * stand on. Also to consider is that tiles are generated and lined up at their
 * bottom-most bounds, as if they were all resting on a flat surface.
 *
 * Rule 2.
 *    The edges of all tiles must be uniform in floor height.  This ensures that all tiles
 *    "line up" and create no unsightly edges/steps in terrain between each tile.
 *
 * In terms of total height, tiles have no limits.  A tile may have a towering skyscraper on
 * it. Tiles may also have any "floor height" within the edges, ie. they can dip or rise or
 * whatever in the centre of each tile, as long as any surfaces the player is expected to
 * run on line up with adjacent tiles.
 *
 * Tiles are divided into currently (v1.0) two groups: "floor" tiles, and "wall" tiles. A
 * street is composed of floor tiles bounded by one wall tile on each side.  There is no
 * limits on what is considered a floor or wall tile, only that it is recommended that a wall
 * tile prevents movement off the edge of the screen.  Wall tiles may still have walkable
 * street level geometry just like a floor tile.  Currently (v1.0), wall tiles are expected
 * to be "aligned" to be on the left side of the street, and walls generated on the right
 * hand side of the street are taken from the same set and rotated 180 degrees.
 *
 * Tiles are generated as the player gets close enough to their expected position, and are
 * automatically hidden from view/resources when the player gets far enough away.  The choice of
 * tile generated is random from the appropriate set of tiles.  The tile generation currently is
 * in a single completely linear direction, with the opposite direction expected to be walled off
 * and considered to be the "start" of the level.
 *
 * Now that the class is explained....
 *
 * PARAMETERS:
 *
 *    playerGameObject  The player game object.  This object is the centre of the world as
 *                      far as tile generation and visibility is concerned.
 *
 *    referenceTile     This tile is used as a reference for the tile manager, in that all
 *                      generated tiles are assumed to have the same x and z lengths and
 *                      are all lined up with this tile's bottom edge.  Its position also
 *                      determines the "start" of the level.  It need not have a visible
 *                      texture, as it is not considered to be part of the set of generated
 *                      tiles and will thus have a new tile generated at its location.
 *
 *    floorTiles        The set of all tiles that may be generated in the "middle" of the
 *                      street.
 *
 *    wallTiles         The set of all tiles that may be generated on the edges of the street.
 *
 *    tilesAcross       How wide the street is, in tiles (ie. 5 = 1 wall + 3 floor + 1 wall)
 *
 *    visibleRange      How far away tiles are generated/hidden/made visible as the player
 *                      moves, in reference tile widths.
 *
 *
 * Potential expansions:
 * - Split "wall tiles" into "left wall tiles" and "right wall tiles", to allow for better
 *   texture tiling potential.
 * - Affix a weighting to each tile, determining the relative chances of it being selected
 *   out of its set during generation.
 * - More complex tiling, allowing tiles to limit what tiles can be placed adjacent, making
 *   for much more interesting/complex structures made up of multiple tiles.
 * - Support for tile sizes which are multiples of the reference tile, allowing for sections
 *   to be made up of a mixture of small and large tiles and tiles with non-square bases.
 *
 *
 */
public class TileManager : MonoBehaviour {

    // Public variables
    public GameObject playerGameObject; // The object to draw tiles around.
    public int tilesAcross = 5;         // Width of street (in tiles)
    public int visibleRange = 30;       // Distance to render (in tiles)
    public int startAreaSize = 10;      // How many rows of tiles to generate behind the player
	
    // Internal variables
    private GameObject[] floorTiles;   // List of all floor tiles
    private GameObject[] wallTiles ;   // List of all wall tiles
    private GameObject referenceTile; // An object representing a regular tile's size, the origin of where the tiles are going to be placed.
    private DoubleKeyDictionary<int, int, GameObject> tiles;    // Maps coords to tiles
    private int maxVisibleRow = -1;     // The maximum row the player can "see"
    private int minVisibleRow = 1;      // The minimum row the player can "see"
    private int playerLastRow = 0, playerLastCol = 0;   // Row/col the player was last on
    private float refSizeX, refSizeZ, refStartX, refStartZ; // Reference tile "constants"

    // testing
    public GUIStyle style = new GUIStyle();
    void OnGUI () {
        GUI.Label(new Rect(20,40,80,20), "(" + playerLastRow.ToString() + "," + playerLastCol.ToString() + ")" , style);
        GUI.Label(new Rect(20,60,80,20), "visible min="+minVisibleRow.ToString()+", max="+maxVisibleRow.ToString() , style);
    }

    /* ==========================================================================================
     * On Initialisation
     * ========================================================================================== */
    void Start () {
        // Look in our children for Tiles, walls and our referenceTile and initialize our variables with them
        List<GameObject> _floorTiles = new List<GameObject>();   // List of all floor tiles
        List<GameObject> _wallTiles  = new List<GameObject>();   // List of all wall tiles
        foreach (Transform child in transform) {
            if      (child.gameObject.tag == "Tile")    _floorTiles.Add(child.gameObject);
            else if (child.gameObject.tag == "Wall")    _wallTiles.Add( child.gameObject);
            else if (child.gameObject.tag == "World")   referenceTile = child.gameObject ;
            //else                                        Debug.Log("A child with a weird label appeared. Label: " + child.gameObject.tag);
        }
        floorTiles = _floorTiles.ToArray();
        wallTiles  =  _wallTiles.ToArray();

        // Populate reference tile info, because the reference tile should never change and these
        // fields add brevity to some code while saving on computation.
        refSizeZ = referenceTile.collider.bounds.size.z;
        refSizeX = referenceTile.collider.bounds.size.x;
        // These two fields are because the position of the reference tile is actually its exact
        // centre.  This is fine for some things but not others (ie. determining which tile player
        // is on)
        refStartZ = referenceTile.transform.position.z - refSizeZ / 2f;
        refStartX = referenceTile.transform.position.x - refSizeX / 2f;


        // Initialise tiles map.. err, dictionary.
        // It uses two coordinates (z,x) as the key and maps to a tile, where z and x are multiples
        // of the reference tile's z/x.
        // For reference: Z is each row, X is each column.
        tiles = new DoubleKeyDictionary<int, int, GameObject>();

        // Initialise last row/col of player
        int[] playerCoords = getPlayerCoordinates();
        playerLastRow = playerCoords[0];
        playerLastCol = playerCoords[1];

        // Initialise minimum visible row
        minVisibleRow = playerCoords[0] - visibleRange;

        // Generate initial tiles
        maxVisibleRow = -startAreaSize;
        while (playerCoords[0] + visibleRange > maxVisibleRow) {
            maxVisibleRow++;
            for (int col = 0; col < tilesAcross; col++) {
                generateTerrainAt(maxVisibleRow, col);
            }
        }

        /* TODO:
         * Generation of tiles occurs during update, but some generation should occur here to flesh
         * out the "starting area", with the walled end of the street being the dungeon the player
         * has broken out of etc.
         * This could be done as one contiguous "tile" that is multiple reference tiles in width,
         * and could thus be implemented at the same time as support for multiple tile sizes.
         */
    }

    /* ==========================================================================================
     * On Update
     * ========================================================================================== */
    void Update () {

        // Get the player's coordinates and determine the change in rows (if any).
        int[] playerCoords = getPlayerCoordinates();
        int rowDiff = playerCoords[0] - playerLastRow;
        int direction = (rowDiff<0)?-1:1;

        // For each difference in row, shift the "visible window" of tiles towards the player's new
        // location, hiding and showing tiles along the way.
        // Note that the use of a for loop allows a single update() call to handle the generation
        // and visibility of any number of tiles, no matter how far the player has moved since the
        // last call.
        for (int i=rowDiff; i!=0; i-=direction) {

            // For each tile on the row
            for (int col = 0; col < tilesAcross; col++)
            {
                // Generate tile if it doesn't exist in the positive direction.
                if (!tiles.ContainsKey(maxVisibleRow,col))
                {
                    generateTerrainAt(maxVisibleRow, col);
                }

                // Moving in a positive direction: show (moving positive) or hide (moving negative)
                if (tiles[maxVisibleRow,col].activeSelf == (direction<0))
                {
                    tiles[maxVisibleRow,col].SetActive(direction>0);
                }

                // Moving in a negative direction: hide (moving positive) or show (moving negative)
                if (tiles.ContainsKey(minVisibleRow,col) && tiles[minVisibleRow,col].activeSelf == (direction>0))
                {
                    tiles[minVisibleRow,col].SetActive(direction<0);
                }
            }
            // Shift visible window
            maxVisibleRow += direction;
            minVisibleRow += direction;
        }

        // Update last locations
        playerLastRow = playerCoords[0];
        playerLastCol = playerCoords[1];

        /*
         * Better visibility method, for use where streets start to snake etc (better in that it's
         * more simple to comprehend):
         *
         * Let set A be the set of visible tiles
         * Let set B be the set of tiles within vision radius of the player
         * Hide tiles in set A-B
         * Show tiles in set B-A
         *
         */
    }

    /* ==========================================================================================
     * Helper function: Generate terrain at coordinates
     * ========================================================================================== */
    void generateTerrainAt(int row, int col) {
        // Only add if it doesn't exist already
        if (!tiles.ContainsKey(row,col))
        {
            GameObject tile = null;
            // Wall or floor?
            if (col == 0 || col == tilesAcross-1)   tile =  wallTiles[Random.Range(0, wallTiles.Length)];
            else                                    tile = floorTiles[Random.Range(0,floorTiles.Length)];

            // The tile's new position is based on its row and column
            // it is relative to the reference tile.
            Vector3 newPos = Vector3.zero;
            newPos.x = col * referenceTile.collider.bounds.size.x;
            newPos.z = row * referenceTile.collider.bounds.size.z;

            // Instantiate the new tile and add it to the tiles map
            GameObject newTile = (GameObject)Instantiate(tile, newPos, referenceTile.transform.rotation);
            newTile.transform.parent = referenceTile.transform;
            newTile.transform.localPosition = newPos;
            tiles.Add(row,col,newTile);

            // Rotate if it's the right-side wall
            if (col == tilesAcross-1) {
                newTile.transform.Rotate(0f,180f,0f);
            }
        }
    }

    /* ==========================================================================================
     * Helper function: Get player's coordinates as multiples of reference tile size
     * Returns an int array holding each coordinate because who needs tuples
     * ========================================================================================== */
    int[] getPlayerCoordinates() {
        int[] coords = new int[2]; //z (row), x (col)
        coords[0] = (int)((playerGameObject.transform.collider.bounds.center.z - refStartZ) / refSizeZ);
        coords[1] = (int)((playerGameObject.transform.collider.bounds.center.x - refStartX) / refSizeX);
        return coords;
    }
}
