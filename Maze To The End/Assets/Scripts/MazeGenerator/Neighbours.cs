using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Neighbours {
    public Cell cell; 
    public Direction direction;

    public enum Direction {
        North,
        South,
        East,
        West
    }

	public Neighbours (Cell cell, Direction direction) {
        this.cell = cell;
        this.direction = direction;
    }
}
