using UnityEngine;

namespace ExtraPrefabs
{
	public class WindVisuals : SimBehaviour
	{
		protected override void Start()
		{
			if(!StatMaster.Mode.levelEdit)
			{
				WindEntity entity = GetComponent<WindEntity>();
				entity.UpdateVisuals();
			}
		}
	}
}
