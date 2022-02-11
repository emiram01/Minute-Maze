using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public Vector2Int coordinates;
    public MazeRoom room;
    private MazeCellEdge[] _edges = new MazeCellEdge[MazeDirections.Count];
    private int _edgeCount;

	public void Initialize(MazeRoom room)
    {
	    room.Add(this);
		// transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return _edges[(int) direction];
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        _edges[(int) direction] = edge;
        _edgeCount += 1;
    }

    public bool IsFull
    {
        get { return _edgeCount == MazeDirections.Count; }
    }

    public MazeDirection RandomDirection()
    {
        int skips = Random.Range(0, MazeDirections.Count - _edgeCount);
        for(int i = 0; i < MazeDirections.Count; i++)
        {
            if(_edges[i] == null)
            {
                if(skips == 0)
                {
                    return (MazeDirection) i;
                }
                skips -= 1;
            }
        }
        throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
    }
}
