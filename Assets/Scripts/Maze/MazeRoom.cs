using UnityEngine;
using System.Collections.Generic;

public class MazeRoom : ScriptableObject
{
	public MazeRoomSettings settings;
    public int settingsIndex;
	private List<MazeCell> _cells = new List<MazeCell>();

	public void Add(MazeCell cell)
    {
		cell.room = this;
		_cells.Add(cell);
	}

    public void Assimilate(MazeRoom room)
    {
		for(int i = 0; i < room._cells.Count; i++)
			Add(room._cells[i]);
	}
}