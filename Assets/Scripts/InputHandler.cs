using UnityEngine;

namespace BallBattle
{
	public class InputHandler : MonoBehaviour
	{
		[SerializeField] private LayerMask whatIsField_;

		private Vector3 tempPos;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				tempPos.z -= Camera.main.transform.position.z;
				Ray ray = new Ray(Camera.main.transform.position, tempPos - Camera.main.transform.position);
				if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, whatIsField_, QueryTriggerInteraction.Ignore))
				{
					Debug.Log(hit.transform.name + " RAY!");
					if(hit.transform.TryGetComponent(out FieldHandler field))
					{
						field.TriggerSummonSoldier(tempPos);
					}
				}
			}
		}
	}
}
