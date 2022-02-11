using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public bool mazeComplete;
    [SerializeField] private MazeCell _cellPrefab;
    [SerializeField] private MazePassage _passagePrefab;
    [SerializeField] private MazeWall _wallPrefab;
    [SerializeField] private MazeDoor _doorPrefab;

    [Header("Space")]
    [SerializeField] private Vector2Int _size;
    [SerializeField] [Range(0f, 1f)] private float _doorProbablity;
    [SerializeField] private float _generationDelay;
    [SerializeField] private MazeRoomSettings[] _roomSettings;
    private List<MazeRoom> _rooms = new List<MazeRoom>();
    private MazeCell _cellInstance;
    private MazeCell[,] _cells;
    private AudioManager _audio;

    private void Start()
    {
        _audio = FindObjectOfType<AudioManager>();
    }

    public IEnumerator Generate()
    {
        StartCoroutine(nameof(PlaySound));
        WaitForSeconds delay = new WaitForSeconds(_generationDelay);
		_cells = new MazeCell[_size.x, _size.y];
        List<MazeCell> activeCells = new List<MazeCell>();
        FirstGenStep(activeCells);
        while(activeCells.Count > 0)
        {
            yield return delay;
            NextGenStep(activeCells);
        }

        mazeComplete = true;
	}

    public IEnumerator PlaySound()
    {
        WaitForSeconds delay = new WaitForSeconds(0.125f);
        while(!mazeComplete)
        {
            yield return delay;
            _audio.Play("mazeSound");
        }
	}

	private void FirstGenStep(List<MazeCell> activeCells)
    {
        MazeCell newCell = CreateCell(RandomCoordinates);
		newCell.Initialize(CreateRoom(-1));
		activeCells.Add(newCell);
	}

	private void NextGenStep(List<MazeCell> activeCells)
    {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
        if(currentCell.IsFull)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
		MazeDirection direction = currentCell.RandomDirection();
		Vector2Int coords = currentCell.coordinates + direction.ToVector2Int();
		if (ContainsCoordinates(coords))
        {
            MazeCell neighbor = GetCell(coords);
            if(neighbor == null)
            {
                neighbor = CreateCell(coords);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            } else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex) {
				CreatePassageInSameRoom(currentCell, neighbor, direction);
			} else {
                CreateWall(currentCell, neighbor, direction);
            }
		} else {
            CreateWall(currentCell, null, direction);
		}
	}

    private MazeCell CreateCell(Vector2Int coords)
    {
		MazeCell newCell = Instantiate(_cellPrefab) as MazeCell;
		_cells[coords.x, coords.y] = newCell;
        newCell.coordinates = coords;
		newCell.name = "Maze Cell " + coords.x + ", " + coords.y;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coords.x - _size.x * 0.5f + 0.5f, 0f, coords.y - _size.y * 0.5f + 0.5f);
        return newCell;
    }

    private MazeRoom CreateRoom(int indexToExclude)
    {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = Random.Range(0, _roomSettings.Length);
		if(newRoom.settingsIndex == indexToExclude)
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % _roomSettings.Length;

		newRoom.settings = _roomSettings[newRoom.settingsIndex];
		_rooms.Add(newRoom);
		return newRoom;
	}

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage prefab = Random.value < _doorProbablity ? _doorPrefab : _passagePrefab;
        MazePassage passage = Instantiate(prefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(prefab) as MazePassage;

        if(passage is MazeDoor)
			otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
		else
			otherCell.Initialize(cell.room);
        
		passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreatePassageInSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
		MazePassage passage = Instantiate(_passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(_passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
        if(cell.room != otherCell.room)
        {
			MazeRoom roomToAssimilate = otherCell.room;
			cell.room.Assimilate(roomToAssimilate);
			_rooms.Remove(roomToAssimilate);
			Destroy(roomToAssimilate);
		}
	}

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(_wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
        if(otherCell != null)
        {
            // wall = Instantiate(_wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    public MazeCell GetCell(Vector2Int coords)
    {
        return _cells[coords.x, coords.y];
    }

    public Vector2Int RandomCoordinates
    {
        get{ return new Vector2Int(Random.Range(0, _size.x), Random.Range(0, _size.y)); }
    }

    public bool ContainsCoordinates(Vector2Int coord)
    {
        return coord.x >= 0 && coord.x < _size.x && coord.y >= 0 && coord.y < _size.y;
    }
}
