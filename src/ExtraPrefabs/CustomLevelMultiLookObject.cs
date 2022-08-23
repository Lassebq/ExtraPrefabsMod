using Localisation;
using System.Collections.Generic;
using UnityEngine;

namespace ExtraPrefabs
{
	public class CustomLevelMultiLookObject : MonoBehaviour, ILocalisationAware
	{
		public bool isInitialized;

		public GameObject[] entityLooks;

		public int[] nameLocalisations;

		protected MMenu lookMenu;
		public List<string> names
		{
			get
			{
				List<string> list = new List<string>(entityLooks.Length);
				for (int i = 0; i < entityLooks.Length; i++)
				{
					if(i < nameLocalisations.Length)
					{
						int id = nameLocalisations[i];
						if (id != 0)
						{
							list.Insert(i, LocalisationManager.GetTranslation(id));
							continue;
						}
					}
					list.Insert(i, entityLooks[i].name);
				}
				return list;
			}
		}

		public void Awake()
		{
			if (!isInitialized)
			{
				GenericEntity ge = GetComponent<GenericEntity>();
				if (entityLooks.Length > 0)
				{
					lookMenu = ge.AddMenu("look", 0, names);
					lookMenu.ValueChanged += UpdateLook;
					UpdateLook(lookMenu.Value);
				}
				isInitialized = true;
			}
		}

		public void OnLocalisationChange()
		{
			if (lookMenu != null)
			{
				lookMenu.Items = names;
			}
		}

		private void UpdateLook(int index)
		{
			EntityVisualController component = GetComponent<EntityVisualController>();
			bool flag = component != null;
			if (flag)
			{
				component.Restore();
			}
			index = ((index < entityLooks.Length) ? index : 0);
			for (int i = 0; i < entityLooks.Length; i++)
			{
				entityLooks[i].SetActive(index == i);
			}
			if (flag && entityLooks.Length > 0)
			{
				GameObject entityGO = entityLooks[index];
				component.Init(entityGO);
			}
		}

	}
}
