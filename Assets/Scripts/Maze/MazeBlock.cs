using UnityEngine;

namespace BallBattle.Maze
{
	public class MazeBlock : MonoBehaviour
	{
		[SerializeField] private GameObject leftWall_;
		[SerializeField] private GameObject rightWall_;
		[SerializeField] private GameObject frontWall_;
		[SerializeField] private GameObject backWall_;
		[SerializeField] private GameObject unvisitedBlock_;

		public bool IsVisited { get; private set; }
		public int WidthIndex { get; private set; }
		public int DepthIndex { get; private set; }

		public void SetBlockIndex(int _x, int _y)
		{
			WidthIndex = _x;
			DepthIndex = _y;
		}

		public void Visit()
		{
			IsVisited = true;
			unvisitedBlock_.SetActive(false);
		}

		public void Unvisit()
		{
			IsVisited = false;
			unvisitedBlock_.SetActive(true);
			leftWall_.SetActive(true);
			rightWall_.SetActive(true);
			frontWall_.SetActive(true);
			backWall_.SetActive(true);
		}

		public void ClearLeftWall()
		{
			leftWall_.SetActive(false);
		}

		public void ClearRightWall()
		{
			rightWall_.SetActive(false);
		}

		public void ClearFrontWall()
		{
			frontWall_.SetActive(false);
		}

		public void ClearBackWall()
		{
			backWall_.SetActive(false);
		}
	}
}
