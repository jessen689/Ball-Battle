using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BallBattle.Maze
{
	public class MazeGenerator : MonoBehaviour
	{
		[SerializeField] private MazeBlock mazeBlockPrefab_;

		[SerializeField] private int mazeWidth_;

		[SerializeField] private int mazeDepth_;

		[SerializeField] private Transform startingPointSpawn;

		private MazeBlock[,] mazeGrid;
		private bool isFirstTime = true;
		private MazeBlock firstGeneratedBlock;

		private const float MAZE_BLOCK_SIZE = .8f;
		private const int ENDPOINT_X_INDEX = 3;
		private const int ENDPOINT_Y_INDEX = 13;

		private void Awake()
		{
			mazeGrid = new MazeBlock[mazeWidth_, mazeDepth_];
		}

		public void InitializeMaze()
		{
			if (isFirstTime)
			{
				isFirstTime = false;

				for (int x = 0; x < mazeWidth_; x++)
				{
					for (int y = 0; y < mazeDepth_; y++)
					{
						mazeGrid[x, y] = Instantiate(mazeBlockPrefab_, new Vector3((x * MAZE_BLOCK_SIZE) + startingPointSpawn.position.x, (y * MAZE_BLOCK_SIZE) + startingPointSpawn.position.y, 0), Quaternion.identity, startingPointSpawn);
						mazeGrid[x, y].SetBlockIndex(x, y);
					}
				}
			}
			else
				ResetMaze();

			firstGeneratedBlock = mazeGrid[mazeWidth_ / 2, 0]; //dibagi 2 biar mulai dari tengah bawah, index 0 = paling kiri/paling bawah
			GenerateMaze(null, firstGeneratedBlock);
		}

		private void ResetMaze()
		{
			for (int x = 0; x < mazeWidth_; x++)
			{
				for (int y = 0; y < mazeDepth_; y++)
				{
					mazeGrid[x, y].Unvisit();
				}
			}
		}

		public void HideMaze()
		{
			startingPointSpawn.gameObject.SetActive(false);
		}

		private void GenerateMaze(MazeBlock _previousBlock, MazeBlock _currentBlock)
		{
			startingPointSpawn.gameObject.SetActive(true);
			_currentBlock.Visit();
			ClearWalls(_previousBlock, _currentBlock);

			MazeBlock nextBlock;

			do
			{
				nextBlock = GetNextUnvisitedBlock(_currentBlock);

				if (nextBlock != null)
				{
					GenerateMaze(_currentBlock, nextBlock);
				}
			} while (nextBlock != null);
		}

		public MazeBlock GetFirstGeneratedBlock()
		{
			return firstGeneratedBlock;
		}

		public MazeBlock GetRandomBlock()
		{
			return mazeGrid[Random.Range(0, mazeWidth_), Random.Range(0, mazeDepth_)];
		}

		private MazeBlock GetNextUnvisitedBlock(MazeBlock _currentBlock)
		{
			var unvisitedBlocks = GetUnvisitedBlocks(_currentBlock);

			return unvisitedBlocks.OrderBy(block => Random.Range(1, 10)).FirstOrDefault();
		}

		private IEnumerable<MazeBlock> GetUnvisitedBlocks(MazeBlock _currentBlock)
		{
			int x = _currentBlock.WidthIndex;
			int y = _currentBlock.DepthIndex;

			if (x + 1 < mazeWidth_)
			{
				var blockToRight = mazeGrid[x + 1, y];

				if (blockToRight.IsVisited == false)
				{
					yield return blockToRight;
				}
			}

			if (x - 1 >= 0)
			{
				var blockToLeft = mazeGrid[x - 1, y];

				if (blockToLeft.IsVisited == false)
				{
					yield return blockToLeft;
				}
			}

			if (y + 1 < mazeDepth_)
			{
				var blockToFront = mazeGrid[x, y + 1];

				if (blockToFront.IsVisited == false)
				{
					yield return blockToFront;
				}
			}

			if (y - 1 >= 0)
			{
				var blockToBack = mazeGrid[x, y - 1];

				if (blockToBack.IsVisited == false)
				{
					yield return blockToBack;
				}
			}
		}

		private void ClearWalls(MazeBlock _previousBlock, MazeBlock _currentBlock)
		{
			if (_currentBlock.WidthIndex == ENDPOINT_X_INDEX && _currentBlock.DepthIndex == ENDPOINT_Y_INDEX)
				_currentBlock.ClearFrontWall();

			//if(_currentBlock.WidthIndex == 0)
			//	_currentBlock.ClearLeftWall();

			//if(_currentBlock.WidthIndex == mazeWidth_ - 1)
			//	_currentBlock.ClearRightWall();

			//if (_currentBlock.DepthIndex == 0)
			//	_currentBlock.ClearBackWall();

			//if (_currentBlock.DepthIndex == mazeDepth_ - 1)
			//	_currentBlock.ClearFrontWall();

			if (_previousBlock == null)
				return;

			if (_previousBlock.WidthIndex < _currentBlock.WidthIndex)
			{
				_previousBlock.ClearRightWall();
				_currentBlock.ClearLeftWall();
			}
			else if (_previousBlock.WidthIndex > _currentBlock.WidthIndex)
			{
				_previousBlock.ClearLeftWall();
				_currentBlock.ClearRightWall();
			}
			else if (_previousBlock.DepthIndex < _currentBlock.DepthIndex)
			{
				_previousBlock.ClearFrontWall();
				_currentBlock.ClearBackWall();
			}
			else if (_previousBlock.DepthIndex > _currentBlock.DepthIndex)
			{
				_previousBlock.ClearBackWall();
				_currentBlock.ClearFrontWall();
			}
		}
	}
}
