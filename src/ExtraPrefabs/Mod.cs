using ObjectTypes;
using Modding;
using UnityEngine;

namespace ExtraPrefabs
{
	public class Mod : ModEntryPoint
	{
		public ModAssetBundle Bundle;
		public override void OnLoad()
		{
			Bundle = ModResource.GetAssetBundle("extraprefabs");
			ModConsole.RegisterCommand("listprefabs", ListPrefabs, "Lists prefabs");
			ModConsole.RegisterCommand("findprefab", FindPrefab, "Displays prefab IDs with similar name");
			RegisterPrefabs();
		}
		public static void ListPrefabs(string[] args)
		{
			foreach(System.Collections.Generic.KeyValuePair<int, LevelPrefab> prefab in PrefabMaster.LevelPrefabs[9])
			{
				ModConsole.Log(prefab.Key + " " + prefab.Value.name);
			}
		}
		public static void FindPrefab(string[] args)
		{
			if(args.Length > 0)
			{
				string searchName = args[0];
				foreach (System.Collections.Generic.KeyValuePair<int, LevelPrefab> prefab in PrefabMaster.LevelPrefabs[9])
				{
					if (prefab.Value.name.ToLower().Contains(searchName.ToLower()))
					{
						ModConsole.Log(prefab.Key + " " + prefab.Value.name);
					}
				}
			}
			else
			{
				ModConsole.Log("Not enough arguments");
			}
		}
		public override void OnEntityPrefabCreation(int entityId, GameObject prefabObj)
		{
			LevelPrefab prefab = prefabObj.GetComponent<LevelPrefab>();
			GameObject entityLook;

			switch (entityId)
			{
				case (PrefabConstants.SandboxArch):
				{
					prefab.name = "Sandbox Arch";
					entityLook = CustomPrefab(InstantiatePrefab("SandboxArch"));
					ReplaceModdedPrefab(prefab, entityLook);
					break;
				}
				case (PrefabConstants.DragonSkull):
				{
					prefab.name = "Dragon Skull";
					entityLook = CustomPrefab(InstantiatePrefab("FossilSkull"));
					ReplaceModdedPrefab(prefab, entityLook);
					entityLook = CustomPrefab(InstantiatePrefab("DragonSkull"));
					RegisterPrefabLook("DragonSkull2", LocalisationConstants.Valfross, prefab, entityLook);
					break;
				}
				case (PrefabConstants.DragonWing):
				{
					prefab.name = "Dragon Wing";
					entityLook = CustomPrefab(InstantiatePrefab("FossilWing"));
					ReplaceModdedPrefab(prefab, entityLook);
					entityLook = CustomPrefab(InstantiatePrefab("DragonWing"));
					RegisterPrefabLook("DragonWing2", LocalisationConstants.Valfross, prefab, entityLook);
					break;
				}
				case (PrefabConstants.FrozenTree):
				{
					prefab.name = "Frozen Tree";
					prefab.inflammable = true;
					prefab.destructable = false;
					prefab.stayKinematic = true;
					prefab.ignorePhysics = false;
					entityLook = CustomPrefab(InstantiatePrefab("BigDeadTree"));
					ReplaceModdedPrefab(prefab, entityLook);
					break;
				}
			}
		}

		public void RegisterPrefabs()
		{
			LevelPrefab prefab;
			GameObject entityLook;

			// Prefabs and prefab looks
			#region Bush

			prefab = GetPrefab((int)Foliage.Bush);
			GetEntityLook(prefab, 1).SetActive(false); // Fix ghost prefab

			entityLook = CustomPrefab(InstantiatePrefab("Bush"));
			RegisterPrefabLook("Bush Frozen", LocalisationConstants.Blue, prefab, entityLook);
			#endregion

			#region FirTree

			prefab = GetPrefab((int)Foliage.FirTree);
			entityLook = GetEntityLook(prefab, 1);
			// Fix Snowy Tree collision particles
			if (entityLook != null)
			{
				ParticleOnCollide particleCollide = prefab.transform.GetComponent<ParticleOnCollide>();
				ParticleSystem p = entityLook.transform.FindChild("Particle System").GetComponent<ParticleSystem>();
				if (particleCollide.extraParticles == null)
				{
					particleCollide.extraParticles = p;
				}
			}
			#endregion

			#region Tree

			prefab = GetPrefab((int)Foliage.Tree);
			entityLook = Object.Instantiate(prefab.transform.GetChild(0).gameObject);
			MeshRenderer[] meshes = entityLook.GetComponentsInChildren<MeshRenderer>(true);
			Material dotaBush = Bundle.LoadAsset<Material>("DotaBushDarker 1");
			foreach (MeshRenderer mesh in meshes)
			{
				if (mesh.name.Contains("DotaBush"))
				{
					mesh.material = dotaBush;
				}
			}
			FixShaderReferences(entityLook);
			RegisterPrefabLook("BigTree Calm", LocalisationConstants.Dark, prefab, entityLook);
			entityLook = Object.Instantiate(prefab.transform.GetChild(0).gameObject);
			meshes = entityLook.GetComponentsInChildren<MeshRenderer>(true);
			dotaBush = Bundle.LoadAsset<Material>("DotaBushDarker");
			foreach (MeshRenderer mesh in meshes)
			{
				if (mesh.name.Contains("DotaBush"))
				{
					mesh.material = dotaBush;
				}
			}
			FixShaderReferences(entityLook);
			RegisterPrefabLook("BigTree Calm 1", LocalisationConstants.Normal, prefab, entityLook);
			RenamePrefabLook(prefab, 0, LocalisationConstants.Dark);
			#endregion

			#region IvyLeaves

			prefab = GetPrefab((int)Foliage.IvyLeaves);
			entityLook = CustomPrefab(InstantiatePrefab("IvyFrozen"));
			RegisterPrefabLook("Frozen", LocalisationConstants.Blue, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("IvyWhite"));
			RegisterPrefabLook("White", LocalisationConstants.White, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("IvyDark"));
			RegisterPrefabLook("Dark", LocalisationConstants.Dark, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("IvyFrozen 1"));
			RegisterPrefabLook("Frozen 1", LocalisationConstants.Snowy, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("IvyDesert"));
			RegisterPrefabLook("Desert", LocalisationConstants.Desert, prefab, entityLook);
			#endregion

			#region SimpleWall

			prefab = GetPrefab((int)Brick.SimpleWall);
			entityLook = CustomPrefab(InstantiatePrefab("SimpleWall"));
			RegisterPrefabLook("SimpleWall2", LocalisationConstants.Variant2, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("SimpleWallFlat"));
			RegisterPrefabLook("SimpleWall3", LocalisationConstants.Variant3, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("SimpleWallWindow"));
			RegisterPrefabLook("SimpleWall4", LocalisationConstants.Variant4, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("SimpleWallWindowFlat"));
			RegisterPrefabLook("SimpleWall5", LocalisationConstants.Variant5, prefab, entityLook);
			RenamePrefabLook(prefab, 0, LocalisationConstants.Variant1);
			#endregion

			#region BigRock

			prefab = GetPrefab((int)Foliage.BigRock);
			entityLook = CustomPrefab(InstantiatePrefab("SandboxRock"));
			RegisterPrefabLook("Ancient", prefab, entityLook);
			#endregion

			#region ArcherStatues

			prefab = GetPrefab((int)Brick.ArcherStatues);
			entityLook = UnityEngine.Object.Instantiate(prefab.transform.GetChild(0).gameObject);
			MeshRenderer[] meshes2 = entityLook.GetComponentsInChildren<MeshRenderer>(true);
			Material mat = Bundle.LoadAsset<Material>("BillowMountainStandard 1");
			foreach (MeshRenderer mesh in meshes2)
			{
				mesh.material = mat;
			}
			FixShaderReferences(entityLook);
			RegisterPrefabLook("Ancient", prefab, entityLook);
			#endregion

			#region DragonRibs

			prefab = GetPrefab(2051); // Not present in Buildings enum smh
			prefab.transform.GetChild(0).gameObject.SetActive(false);
			entityLook = CustomPrefab(InstantiatePrefab("DragonRibcage"));
			RegisterPrefabLook("Ribs", LocalisationConstants.Valfross, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("FossilRibcage"));
			ReplacePrefabLook(prefab, 0, entityLook);
			#endregion

			#region GrassTuft

			prefab = GetPrefab((int)Foliage.GrassTuft);
			entityLook = CustomPrefab(InstantiatePrefab("GrassTuft"));
			RegisterPrefabLook("GrassTuft", LocalisationConstants.Small, prefab, entityLook);

			entityLook = CustomPrefab(InstantiatePrefab("Grass Bush"));
			RegisterPrefabLook("Grass Bush", LocalisationConstants.Bush, prefab, entityLook);
			#endregion

			#region GrassPatch

			prefab = GetPrefab((int)Foliage.GrassPatch);
			entityLook = CustomPrefab(InstantiatePrefab("GroundGrass"));
			RegisterPrefabLook("GrassTuft", LocalisationConstants.Dark, prefab, entityLook);
			#endregion

			#region MonkIdolStatue

			prefab = GetPrefab(2055);
			GameObject monkIdol = CustomPrefab(Bundle.LoadAsset<GameObject>("Idol Statue")); // Box collider weirdness when using this as entityLook 
			entityLook = Object.Instantiate(prefab.transform.GetChild(0).gameObject);
			entityLook.GetComponent<MeshFilter>().mesh = monkIdol.GetComponent<MeshFilter>().mesh;
			RegisterPrefabLook("Idol Statue", LocalisationConstants.Variant2, prefab, entityLook);
			RenamePrefabLook(prefab, 0, LocalisationConstants.Variant1);
			#endregion

			#region Insignia

			prefab = GetPrefab((int)Virtual.Insignia);
			entityLook = Object.Instantiate(prefab.transform.GetChild(0).gameObject);
			entityLook.transform.GetChild(0).GetComponent<MeshRenderer>().material = Bundle.LoadAsset<Material>("DeliveryInsignia");
			FixShaderReferences(entityLook);
			RegisterPrefabLook("Delivery", prefab, entityLook);
			#endregion

			#region LargeHill

			prefab = GetPrefab((int)Foliage.LargeHill);
			prefab.transform.GetChild(1).SetParent(prefab.transform.GetChild(0));
			entityLook = Object.Instantiate(prefab.transform.GetChild(0).gameObject);
			entityLook.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = Bundle.LoadAsset<Material>("GrassFlat Darker");
			FixShaderReferences(entityLook);
			RegisterPrefabLook("Darker", LocalisationConstants.Dark, prefab, entityLook);
			#endregion

			// Other fixes
			#region Fixes

			GetPrefab((int)Weather.Wind).gameObject.AddComponent<WindVisuals>();
			FixFlameForPrefab(GetPrefab((int)Foliage.Bush));
			FixFlameForPrefab(GetPrefab((int)Foliage.Tree)); 
			#endregion
		}

		// Adds outlines and fixes shaders
		public GameObject CustomPrefab(GameObject obj)
		{
			FixShaderReferences(obj);
			AddOutline(obj);
			return obj;
		}

		// Adds cakeslice.Outline to a game object
		public GameObject AddOutline(GameObject obj)
		{
			foreach(MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>(true))
			{
				cakeslice.Outline outline = mesh.gameObject.AddComponent<cakeslice.Outline>();
				outline.enabled = false;
			}
			return obj;
		}

		// Shaders inside of asset bundles seem to be broken
		public GameObject FixShaderReferences(GameObject obj)
		{
			Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer renderer in renderers)
			{
				foreach (Material material in renderer.materials)
				{
					material.shader = Shader.Find(material.shader.name);
				}

			}
			return obj;
		}

		public void FixFlameForPrefab(LevelPrefab prefab)
		{
			FireController[] fireControllers = prefab.GetComponentsInChildren<FireController>(true);
			if(fireControllers == null || fireControllers.Length == 0)
			{
				Debug.LogError("No FireController component in " + prefab.name);
				return;
			}
			foreach (FireController fireController in fireControllers)
			{
				fireController.fireParticles = null;
				fireController.additionalFireParticles = new ParticleSystem[0];
				ParticleSystem[] particles = fireController.GetComponentsInChildren<ParticleSystem>(true);
				foreach(ParticleSystem particle in particles)
				{
					if(particle.name.Equals("Fire Particles"))
					{
						if(fireController.fireParticles == null)
						{
							fireController.fireParticles = particle;
						} else
						{
							ArrayUtil.AddValue(ref fireController.additionalFireParticles, particle);
						}

					}
				}
			}
		}
		public void ReplaceModdedPrefab(LevelPrefab prefab, GameObject entityLook)
		{
			if (entityLook == null)
			{
				Debug.LogError("entityLook cannot be null");
				return;
			}
			EntityVisualController evc = prefab.GetComponent<EntityVisualController>();
			prefab.GetComponent<GenericEntity>().MeshRenderer = entityLook.GetComponentInChildren<MeshRenderer>(true);
			Object.DestroyImmediate(prefab.transform.GetChild(0).gameObject);
			entityLook.transform.SetParent(prefab.transform);
			evc.renderers = null;
			evc.outlines = entityLook.GetComponentsInChildren<cakeslice.Outline>(true);
			entityLook.SetActive(true);
		}
		public void RegisterPrefabLook(string name, LevelPrefab prefab, GameObject entityLook)
		{
			RegisterPrefabLook(name, 0, prefab, entityLook);
		}
		public void RegisterPrefabLook(string name, int locId, LevelPrefab prefab, GameObject entityLook)
		{
			if (entityLook == null)
			{
				Debug.LogError("entityLook cannot be null");
				return;
			}
			GenericEntity ge = prefab.GetComponent<GenericEntity>();
			LevelMultiLookObject entity = ge as LevelMultiLookObject;
			EntityVisualController evc = prefab.GetComponent<EntityVisualController>();
			entityLook.transform.SetParent(prefab.transform);
			entityLook.name = name;
			if (entity != null) // Is LevelMultiLookObject
			{
				entityLook.SetActive(false); // Make it so that it doesn't show up in the ghost prefab
				ArrayUtil.AddValue(ref entity.entityLooks, entityLook);
				ArrayUtil.AddValue(ref entity.nameLocalisations, locId);
				ArrayUtil.AddValues(ref evc.outlines, entityLook.GetComponentsInChildren<cakeslice.Outline>(true));
				if (ge.breakForce.Length > 0)
				{
					ArrayUtil.AddValue(ref ge.breakForce[0].objsToDestroy, entityLook);
				}
			}
			else
			{
				CustomLevelMultiLookObject customEntity = prefab.GetComponent<CustomLevelMultiLookObject>();
				bool init = false;
				if (customEntity == null)
				{
					customEntity = prefab.gameObject.AddComponent<CustomLevelMultiLookObject>();
					init = true;
				}
				entityLook.SetActive(init); // Make it so that it doesn't show up in the ghost prefab
				if (init)
				{
					customEntity.entityLooks = new GameObject[] { prefab.transform.GetChild(0).gameObject };
					customEntity.nameLocalisations = new int[] { LocalisationConstants.Normal };
				}
				ArrayUtil.AddValue(ref customEntity.entityLooks, entityLook);
				ArrayUtil.AddValue(ref customEntity.nameLocalisations, locId);
				ArrayUtil.AddValues(ref evc.outlines, entityLook.GetComponentsInChildren<cakeslice.Outline>(true));
				if (ge.breakForce.Length > 0)
				{
					ArrayUtil.AddValue(ref ge.breakForce[0].objsToDestroy, entityLook);
				}
			}
		}
		public static void RenamePrefabLook(LevelPrefab prefab, int index, int newLocId)
		{
			GenericEntity ge = prefab.GetComponent<GenericEntity>();
			if (ge is LevelMultiLookObject levelMultiLook)
			{
				levelMultiLook.nameLocalisations[index] = newLocId;
				return;
			}
			CustomLevelMultiLookObject entity = prefab.GetComponent<CustomLevelMultiLookObject>();
			if (entity)
			{
				entity.nameLocalisations[index] = newLocId;
			}
		}
		public static void ReplacePrefabLook(LevelPrefab prefab, int index, GameObject entityLook)
		{
			entityLook.transform.SetParent(prefab.transform);
			GenericEntity ge = prefab.GetComponent<GenericEntity>();
			EntityVisualController evc = prefab.GetComponent<EntityVisualController>();
			if (ge is LevelMultiLookObject levelMultiLook)
			{
				levelMultiLook.entityLooks[index] = entityLook;
				ArrayUtil.AddValues(ref evc.outlines, entityLook.GetComponentsInChildren<cakeslice.Outline>(true));
				return;
			}
			CustomLevelMultiLookObject entity = prefab.GetComponent<CustomLevelMultiLookObject>();
			if (entity)
			{
				entity.entityLooks[index] = entityLook;
				ArrayUtil.AddValues(ref evc.outlines, entityLook.GetComponentsInChildren<cakeslice.Outline>(true));
			}
		}
		private static GameObject GetEntityLook(LevelPrefab prefab, int index)
		{
			GenericEntity ge = prefab.GetComponent<GenericEntity>();
			if (ge is LevelMultiLookObject levelMultiLook)
			{
				return levelMultiLook.entityLooks[index];
			}
			CustomLevelMultiLookObject entity = prefab.GetComponent<CustomLevelMultiLookObject>();
			if (entity)
			{
				return entity.entityLooks[index];
			}
			return null;
		}
		private static LevelPrefab GetPrefab(int val)
		{
			return PrefabMaster.LevelPrefabs[9].GetValue(val);
		}
		public GameObject InstantiatePrefab(string name)
		{
			GameObject prefab = Bundle.LoadAsset<GameObject>(name);
			prefab.SetActive(false);
			GameObject prefabNew = Object.Instantiate(prefab);
			prefabNew.name = prefab.name;
			return prefabNew;
		}
		public GameObject InstantiatePrefab(string name, Transform parent)
		{
			GameObject prefab = Bundle.LoadAsset<GameObject>(name);
			prefab.SetActive(false);
			GameObject prefabNew = Object.Instantiate(prefab, parent) as GameObject;
			prefabNew.name = prefab.name;
			return prefabNew;
		}
		public GameObject InstantiatePrefab(string name, Vector3 position, Quaternion rotation, Transform parent)
		{
			GameObject prefab = Bundle.LoadAsset<GameObject>(name);
			prefab.SetActive(false);
			GameObject prefabNew = Object.Instantiate(prefab, position, rotation, parent) as GameObject;
			prefabNew.name = prefab.name;
			return prefabNew;
		}

	}
}
