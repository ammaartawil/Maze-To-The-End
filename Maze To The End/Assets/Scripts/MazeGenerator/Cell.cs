public class Cell 
{
    public bool west, north, east, south;    // Mur Ouest, Nord, Est et Sud.
    public bool visited;                       // Permet de savoir si oui ou non la cellule à était visiter.

    public int xPos, zPos;                      // Position en X et en Z.
    
    // Constructeur.
    public Cell (bool west, bool north, bool east, bool south, bool visited)
    {
        this.west = west;
        this.north = north;
        this.east = east;
        this.south = south;
        this.visited = visited;
    }

}
