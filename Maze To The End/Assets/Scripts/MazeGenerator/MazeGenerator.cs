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
        	Debug.Log("Doing " + x + " " + y);
        Cell currentCell = cells[x, y]; //Definit la cellule courante
        neighbours = new List<Neighbours>(); //Initialise la liste
        if(currentCell.visited == true) return;
        currentCell.visited = true;

        if(x + 1 < size && cells[x + 1, y].visited == false)
        { //Si on est pas a la largeur limite max du laby et que la cellule de droite n'est pas visiter alors on peut aller a droite
            neighbours.Add(new Neighbours(cells[x + 1, y], Neighbours.Direction.East)); //Ajoute la cellule voisine de droite dans la liste des voisins
        }

        if(y + 1 < size && cells[x, y + 1].visited == false)
        { //Si on est pas a la longueur limite du laby et que la cellule du bas n'est pas visiter alors on peut aller en bas
            neighbours.Add(new Neighbours(cells[x, y + 1], Neighbours.Direction.South)); //Ajoute la cellule voisine du bas dans liste des voisins
        }

        if(x - 1 >= 0 && cells[x - 1, y].visited == false)
        { //Si on est pas a la largeur limite mini du laby et que la cellule de gauche n'est pas visiter alors on peut aller a gauche
            neighbours.Add(new Neighbours(cells[x - 1, y], Neighbours.Direction.West)); //Ajoute la cellule voisine de gauche dans la liste des voisins
        }

        if(y - 1 >= 0 && cells[x, y - 1].visited == false)
        { //Si on est pas a la longueur limite mini du laby et que la cellule du haut n'est pas visiter alors on peut aller en haut
            neighbours.Add(new Neighbours(cells[x, y - 1], Neighbours.Direction.North)); //Ajoute la cellule voisine du haut dans la liste des voisins
        }

        if(neighbours.Count == 0) return;  // Si il y a 0 voisins dans la liste on sort de la méthode.

        neighbours.Shuffle(); // Melange la liste de voisins

        foreach(Neighbours selectedcell in neighbours)
        {
            if(selectedcell.direction == Neighbours.Direction.East)
            { // A droite
                if(selectedcell.cell.visited) continue;
                currentCell.east = true; //Detruit le mur de droite de la cellule courante
                selectedcell.cell.west = true; //Detruit le mur de gauche de la cellule voisine choisie
                GenerateMaze(x + 1, y); //Relance la fonction avec la position de la cellule voisine
            }

            else if(selectedcell.direction == Neighbours.Direction.South)
            { // En bas
                if(selectedcell.cell.visited) continue;
                currentCell.south = true; //Detruit le mur du bas de la cellule courante
                selectedcell.cell.north = true; //Detruit le mur du haut de la cellule voisine choisie
                GenerateMaze(x, y + 1); //Relance la fonction avec la position de la cellule voisine
            }
            else if(selectedcell.direction == Neighbours.Direction.West)
            { // A gauche
                if(selectedcell.cell.visited) continue;
                currentCell.west = true; //Detruit le mur de gauche de la cellule courante
                selectedcell.cell.east = true; //Detruit le mur de droite de la cellule voisine choisie
                GenerateMaze(x - 1, y); //Relance la fonction avec la position de la cellule voisine
            }
            else if(selectedcell.direction == Neighbours.Direction.North)
            { // En haut
                if(selectedcell.cell.visited) continue;
                currentCell.north = true; //Detruit le mur du haut de la cellule courante
                selectedcell.cell.south = true; //Detruit le mur du bas de la cellule voisine choisie
                GenerateMaze(x, y - 1); //Relance la fonction avec la position de la cellule voisine
            }
        }


    }

    void InitVisualCell ()
    {
		// Get a random cell to use for the end cell
		Cell endCell = cells[(int)Random.Range (0, size), (int)Random.Range(0, size)];
		print (endCell.xPos + " " + endCell.zPos);

        // Initialise mes cellules visuel et detruit les murs en fonction des cellules virtuel
        foreach(Cell cell in cells)
        {
			VisualCell visualCellInst;
			if (endCell.xPos == cell.xPos && endCell.zPos == cell.zPos) {
				visualCellInst = Instantiate (endCellPrefab, new Vector3 (cell.xPos * 3, 0, size * 3f - cell.zPos * 3), Quaternion.identity) as VisualCell;
			} else {
				visualCellInst = Instantiate (visualCellPrefab, new Vector3 (cell.xPos * 3, 0, size * 3f - cell.zPos * 3), Quaternion.identity) as VisualCell;
			}
			visualCellInst.transform.parent = transform;
            visualCellInst.north.gameObject.SetActive(!cell.north);
            visualCellInst.south.gameObject.SetActive(!cell.south);
            visualCellInst.east.gameObject.SetActive(!cell.east);
            visualCellInst.west.gameObject.SetActive(!cell.west);

            visualCellInst.transform.name = cell.xPos.ToString() + "_" + cell.zPos.ToString();
        }

    }
}
