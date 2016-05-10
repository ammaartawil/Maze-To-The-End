public class Cell 
{
    public bool west, north, east, south; // If true, there should not be a wall in that direction
	public bool visited; // Whether we've processed this cell or not

    public int xPos, zPos; // Coordinates in the grid
    
    public Cell (bool west, bool north, bool east, bool south, bool visited) {
        this.west = west;
        this.north = north;
        this.east = east;
        this.south = south;
        this.visited = visited;
    }

}
