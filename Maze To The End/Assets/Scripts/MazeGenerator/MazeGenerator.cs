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

	// For random walls
	public List<Material> walls;
	private int wallNumber;

	public Cell endCell;

	public int playerDistance;

	private GameManager gameManager;

	void Start() {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
	}

    public void Init () {
		// Random size of maze
		size = UnityEngine.Random.Range(10, 15);
		//size = 5;
		cells = new Cell[size, size];

		// Random wall
		wallNumber = UnityEngine.Random.Range(0, walls.Count-1);

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
		endCell = cells[(int)Random.Range (0, size), (int)Random.Range(0, size)];

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

			// Colour the walls
			visualCellInst.north.gameObject.GetComponent<Renderer> ().material = walls[wallNumber];
			visualCellInst.south.gameObject.GetComponent<Renderer> ().material = walls[wallNumber];
			visualCellInst.east.gameObject.GetComponent<Renderer> ().material = walls[wallNumber];
			visualCellInst.west.gameObject.GetComponent<Renderer> ().material = walls[wallNumber];

			// Give a name so we can easily tell what cell is what when debugging
            visualCellInst.transform.name = cell.xPos.ToString() + "-" + cell.zPos.ToString();
        }
    }

	public Cell getPlayerStart(int playerNumber) {
		// Get a random cell
		Cell cell = cells [(int)Random.Range (0, size), (int)Random.Range (0, size)];


		List<Cell> route = mazeSearch (cell, new List<Cell> (), new List<Cell>());

		print (route.Count-1);

		// Don't want to start at the end
		if (endCell.xPos == cell.xPos && endCell.zPos == cell.zPos) {
			return getPlayerStart (playerNumber);
		}

		int goal = playerDistance;
		// If we have a goal distance, and we don't equal that goal, try again
		if (playerNumber == 2) {
			goal = playerDistance - Mathf.Abs(gameManager.player1Wins - gameManager.player2Wins);
			print ("new goal is " + goal);
		}

		if (playerDistance != 0 && route.Count - 1 != goal) {
				return getPlayerStart (playerNumber);
		}

		// If we have a short route, let's try again
		if (playerDistance == 0 && route.Count - 1 < 15) {
				return getPlayerStart (playerNumber);
		}

		playerDistance = route.Count - 1;

		return cell;
	}
		
	public List<Cell> mazeSearch(Cell c, List<Cell> found, List<Cell> seen) {
		if (seen.Contains (c)) {
			return null;
		}
		found.Add (c);
		seen.Add (c);

		//print (c.xPos + " " + c.zPos);
		//print ("END IS " + endCell.xPos + " " + endCell.zPos);
		if (c.xPos == endCell.xPos && c.zPos == endCell.zPos) {
			return found;
		}

		if (c.east && c.xPos < size-1) {
			//print ("EAST = " + (c.xPos + 1));
			List<Cell> seen2 = new List<Cell> (seen);
			List<Cell> found2 = mazeSearch(cells[c.xPos + 1, c.zPos], new List<Cell>(found), seen2);
			if (found2 != null) {
				return found2;
			}
		}
		if (c.west && c.xPos > 0) {
			//print ("WEST = " + (c.xPos - 1));
			List<Cell> seen2 = new List<Cell> (seen);
			List<Cell> found2 = mazeSearch(cells[c.xPos - 1, c.zPos], new List<Cell>(found), seen2);
			if (found2 != null) {
				return found2;
			}
		}		
		if (c.north && c.zPos > 0) {
			//print ("NORTH = " + (c.zPos - 1));
			List<Cell> seen2 = new List<Cell> (seen);
			List<Cell> found2 = mazeSearch(cells[c.xPos, c.zPos - 1], new List<Cell>(found), seen2);
			if (found2 != null) {
				return found2;
			}
		}		
		if (c.south && c.zPos < size-1) {
			//print ("SOUTH = " + (c.zPos + 1));
			List<Cell> seen2 = new List<Cell> (seen);
			List<Cell> found2 = mazeSearch(cells[c.xPos, c.zPos + 1], new List<Cell>(found), seen2);
			if (found2 != null) {
				return found2;
			}
		}
		return null;
	}
}

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