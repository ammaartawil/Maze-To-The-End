using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour 
{
    public int size; 

    public VisualCell visualCellPrefab;
	public VisualCell endCellPrefab;

	// 2D array storing the cells of our maze
    public Cell[,] cells; 

	private List<Neighbours> neighbours; // List of each cell's neighbours

    public void Init () {
		cells = new Cell[size, size];

		// Create all the cells
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                cells[i, j] = new Cell(false, false, false, false, false);
                cells[i, j].xPos = i;
                cells[i, j].zPos = j;
            }
        }

        Vector2 startCell = RandomCell(); // Get a random cell to start our generation at
		GenerateMaze((int)startCell.x, (int)startCell.y);  // Generate the maze from this position

        InitVisualCell(); // Visualizes the cells
    }

	// Returns the position of a random cell
	Vector2 RandomCell () {
		return new Vector2((int)UnityEngine.Random.Range(0, size), (int)UnityEngine.Random.Range(0, size));
    }
		
    void GenerateMaze (int x, int y) {
        Cell currentCell = cells[x, y]; 
        neighbours = new List<Neighbours>(); //Initialise la liste
		if (currentCell.visited == true) {
			// If we've already been in the cell, we already have neighbours
			return;
		}
        currentCell.visited = true;

        if(x + 1 < size && cells[x + 1, y].visited == false) { 
			// If we can go right and the right cell has not been visited yet
			// Add it as the East neighbour
            neighbours.Add(new Neighbours(cells[x + 1, y], Neighbours.Direction.East)); 
		}

		if (y + 1 < size && cells [x, y + 1].visited == false) {
			// If we can go down and the down cell has not been visited yet
			// Add it as the South neighbour
			neighbours.Add (new Neighbours (cells [x, y + 1], Neighbours.Direction.South)); 
		}

		if (x - 1 >= 0 && cells [x - 1, y].visited == false) { 
			// If we can go left and the left cell has not been visited yet
			// Add it as the West neighbour
			neighbours.Add (new Neighbours (cells [x - 1, y], Neighbours.Direction.West)); 
		}

		if (y - 1 >= 0 && cells [x, y - 1].visited == false) { 
			// If we can go up and the up cell has not been visited yet
			// Add it as the North neighbour
			neighbours.Add (new Neighbours (cells [x, y - 1], Neighbours.Direction.North)); 
		}

		if(neighbours.Count == 0) {
			// If we have no neighbours, nothing left to do
			return; 
		}

		// Randomly reorder our neighbours
		// Need to do it randomly, as otherwise we just have a boring long corridor
		neighbours.Shuffle();

        foreach(Neighbours selectedCell in neighbours) {
			// Already visited then skip
			if (selectedCell.cell.visited) {
				continue;
			}

			if(selectedCell.direction == Neighbours.Direction.East){
                currentCell.east = true; // Destroys the east wall of the cell
				selectedCell.cell.west = true; // Destroys the west wall of the selectedCell
                GenerateMaze(x + 1, y); // Recurse on the selectedCell
			} else if(selectedCell.direction == Neighbours.Direction.South) { 
                currentCell.south = true; 
				selectedCell.cell.north = true; 
                GenerateMaze(x, y + 1); 
			} else if(selectedCell.direction == Neighbours.Direction.West) { 
                currentCell.west = true; 
				selectedCell.cell.east = true;
                GenerateMaze(x - 1, y); 
			} else if(selectedCell.direction == Neighbours.Direction.North) { // En haut
                currentCell.north = true; 
				selectedCell.cell.south = true; 
                GenerateMaze(x, y - 1); 
            }
        }
    }

    void InitVisualCell () {
		// Get a random cell to use for the end cell
		Cell endCell = cells[(int)Random.Range (0, size), (int)Random.Range(0, size)];

		// Initialize the cells and destroy the relevant walls
		foreach(Cell cell in cells) {
			VisualCell visualCellInst;
			// If cell is our selected end cell, we want to instantiate a different prefab
			if (endCell.xPos == cell.xPos && endCell.zPos == cell.zPos) {
				visualCellInst = Instantiate (endCellPrefab, new Vector3 (cell.xPos * 3, 0, size * 3f - cell.zPos * 3), Quaternion.identity) as VisualCell;
			} else {
				visualCellInst = Instantiate (visualCellPrefab, new Vector3 (cell.xPos * 3, 0, size * 3f - cell.zPos * 3), Quaternion.identity) as VisualCell;
			}
				
			visualCellInst.transform.parent = transform;

			// Delete the relevant walls
            visualCellInst.north.gameObject.SetActive(!cell.north);
            visualCellInst.south.gameObject.SetActive(!cell.south);
            visualCellInst.east.gameObject.SetActive(!cell.east);
            visualCellInst.west.gameObject.SetActive(!cell.west);

			// Give a name so we can easily tell what cell is what when debugging
            visualCellInst.transform.name = cell.xPos.ToString() + "-" + cell.zPos.ToString();
        }
    }
}

// Based on http://stackoverflow.com/questions/273313/randomize-a-listt
static class ListExtensions {

	public static IList<T> Shuffle<T> (this IList<T> list) { 

		int n = list.Count;  

		while (n > 1) {  
			n--;  
			int k = UnityEngine.Random.Range(0, n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		} 

		return list;
	}
}