using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Neighbours 
{
    public Cell cell;           // Cellule.
    public Direction direction; // Direction à prendre lors de la suppression de mur.
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

	public Neighbours (Cell cell, Direction direction)
    {
        this.cell = cell;
        this.direction = direction;
    }
}
