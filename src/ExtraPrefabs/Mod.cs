using ObjectTypes;
using Modding;
using UnityEngine;
using Modding.Levels;

namespace ExtraPrefabs
{
	public class Mod : ModEntryPoint
	{
		public override void OnLoad()
		{
			Mods.OnModLoaded += guid =>
			{
				if (guid.Equals(new System.Guid("11d90bb3-0fad-4c32-911a-94794ab23a39")))
				{
					RegisterPrefabs();
				}
			};
		}

		public override void OnEntityPrefabCreation(int entityId, GameObject prefabObj)
		{
			ModAssetBundle Bundle = ModAssetBundle.GetAssetBundle("extraprefabs");

			LevelPrefab prefab = prefabObj.GetComponent<LevelPrefab>();
			GameObject entityLook;

			switch (entityId)
			{
				case (PrefabConstants.SandboxArch):
				{
					entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("SandboxArch"));
					ReplaceModdedPrefab(prefab, entityLook);
					break;
					}
				case (PrefabConstants.DragonSkull):
				{
					entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("DragonSkull"));
					ReplaceModdedPrefab(prefab, entityLook);
					break;
				}
			}
		}

		public void RegisterPrefabs()
		{
			// Variable declaration
			ModAssetBundle Bundle = ModAssetBundle.GetAssetBundle("extraprefabs");

			LevelPrefab prefab;
			GameObject entityLook;

			// Prefabs and prefab looks
			#region Bush

			prefab = GetPrefab((int)Foliage.Bush);
			GetEntityLook(prefab, 1).SetActive(false); // Fix ghost prefab

			entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("Bush"));
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
			entityLook = UnityEngine.Object.Instantiate(prefab.transform.FindChild("BigTree 1").gameObject);
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
			RegisterPrefabLook("BigTree Calm", LocalisationConstants.Normal, prefab, entityLook);
			RenamePrefabLook(prefab, 0, LocalisationConstants.Wind);
			#endregion

			#region IvyLeaves

			prefab = GetPrefab((int)Foliage.IvyLeaves);
			entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("IvyFrozen"));
			RegisterPrefabLook("Frozen", LocalisationConstants.Blue, prefab, entityLook);

			entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("IvyFrozen 1"));
			RegisterPrefabLook("Frozen 1", LocalisationConstants.Snowy, prefab, entityLook);
			#endregion

			#region SimpleWall

			prefab = GetPrefab((int)Brick.SimpleWall);
			entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("SimpleWall"));
			RegisterPrefabLook("SimpleWall2", LocalisationConstants.Variant2, prefab, entityLook);

			entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("SimpleWallFlat"));
			RegisterPrefabLook("SimpleWall3", LocalisationConstants.Variant3, prefab, entityLook);

			entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("SimpleWallWindow"));
			RegisterPrefabLook("SimpleWall4", LocalisationConstants.Variant4, prefab, entityLook);

			entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("SimpleWallWindowFlat"));
			RegisterPrefabLook("SimpleWall5", LocalisationConstants.Variant5, prefab, entityLook);
			RenamePrefabLook(prefab, 0, LocalisationConstants.Variant1);
			#endregion

			#region BigRock

			prefab = GetPrefab((int)Foliage.BigRock);
			entityLook = CustomPrefab(Bundle.LoadAsset<GameObject>("SandboxRock"));
			RegisterPrefabLook("SandboxRock", LocalisationConstants.AncientStone, prefab, entityLook);
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
			RegisterPrefabLook("SandboxArcherStatue", LocalisationConstants.AncientStone, prefab, entityLook);
			#endregion

			// Other fixes
			#region Fixes

			FixFlameForPrefab(GetPrefab((int)Foliage.Bush));
			FixFlameForPrefab(GetPrefab((int)Foliage.Tree)); 
			#endregion
		}

		public void ReplaceModdedPrefab(LevelPrefab prefab, GameObject entity)
		{
			EntityVisualController evc = prefab.GetComponent<EntityVisualController>();
			Object.DestroyImmediate(prefab.transform.GetChild(0).gameObject);
			GameObject o = (GameObject)Object.Instantiate(entity, prefab.transform);
			evc.renderers = null;
			evc.outlines = o.GetComponentsInChildren<cakeslice.Outline>(true);
		}

		public GameObject CustomPrefab(GameObject obj)
		{
			FixShaderReferences(obj);
			AddOutline(obj);
			return obj;
		}

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

		public void RegisterPrefabLook(string name, LevelPrefab prefab, GameObject entityLook)
		{
			RegisterPrefabLook(name, RegisterLocalizedName(name), prefab, entityLook);
		}

		public void RegisterPrefabLook(string name, int locId, LevelPrefab prefab, GameObject entityLook)
		{
			if(entityLook == null)
			{
				Debug.LogError("entityLook cannot be null");
				return;
			}
			GenericEntity ge = prefab.GetComponent<GenericEntity>();
			LevelMultiLookObject entity = ge as LevelMultiLookObject;
			if (entity == null && ge.GetType() == typeof(GenericEntity)) // Is normal GenericEntity
			{
				entity = prefab.gameObject.AddComponent<LevelMultiLookObject>(); // new LevelMultiLookObject

				// Copy random shit because I don't know how to change GenericEntity to LevelMultiLookObject

				entity.breakForce = ge.breakForce;
				entity.infoType = ge.infoType;
				entity.isBuildBlock = ge.isBuildBlock;
				entity.visualController = ge.visualController;
				entity.prefab = ge.prefab;
				entity.entity = ge.entity;
				entity.entity.behaviour = entity;
				entity.noRigidbody = ge.noRigidbody;
				entity.MeshRenderer = ge.MeshRenderer;
				entity.stripped = ge.stripped;
				entity.physTile = ge.physTile;
				entity.childBodies = ge.childBodies;
				entity.Rigidbody = ge.Rigidbody;
				entity.SimPhysics = ge.SimPhysics;
				if (entity.breakForce.Length > 0)
				{
					entity.breakForce[0].basicInfo = entity;
					entity.breakForce[0].HasBasicInfo = true;
				}
				entity.entityLooks = new GameObject[] { prefab.transform.GetChild(0).gameObject };
				entity.nameLocalisations = new int[] { (int)LocalisationConstants.Normal };
				Object.DestroyImmediate(ge); // Removing old GenericEntity
			}
			if (entity != null) // Is LevelMultiLookObject
			{
				EntityVisualController evc = prefab.GetComponent<EntityVisualController>();
				entityLook.transform.SetParent(prefab.transform);
				entityLook.name = name;
				entityLook.SetActive(false); // Make it so that it doesn't show up in the ghost prefab
				ArrayUtil.AddValue(ref entity.entityLooks, entityLook);
				ArrayUtil.AddValue(ref entity.nameLocalisations, locId);
				ArrayUtil.AddValues(ref evc.outlines, entityLook.GetComponentsInChildren<cakeslice.Outline>(true));
				if (entity.breakForce.Length > 0)
				{
					ArrayUtil.AddValue(ref entity.breakForce[0].objsToDestroy, entityLook);
				}
			}
			else
			{
				Debug.LogError("Prefab already has custom behavior");
			}
		}

		// May be unstable
		public static int RegisterLocalizedName(string name)
		{
			int locId = 1;
			try
			{
				while (Localisation.LocalisationManager.Instance.GetDefaultTranslationFile().ContainsTranslation(locId))
				{
					locId++;
				}
				TranslationEntry entry = new TranslationEntry
				{
					TranslationID = locId,
					Translation = name
				};
				foreach (TranslationFile translation in Localisation.LocalisationManager.Instance.availableLanguages)
				{
					translation.AddTranslation(entry);
				}
			}
			catch
			{
				// Bruh moment
			}
			return locId;
		}
		private static void RenamePrefabLook(LevelPrefab prefab, int index, int newLocId)
		{
			GenericEntity ge = prefab.GetComponent<GenericEntity>();
			if (ge is LevelMultiLookObject levelMultiLook)
			{
				levelMultiLook.nameLocalisations[index] = newLocId;
			}
		}
		private static GameObject GetEntityLook(LevelPrefab prefab, int index)
		{
			GenericEntity ge = prefab.GetComponent<GenericEntity>();
			if (ge is LevelMultiLookObject levelMultiLook)
			{
				return levelMultiLook.entityLooks[index];
			}
			return null;
		}
		private static LevelPrefab GetPrefab(int val)
		{
			return PrefabMaster.LevelPrefabs[9].GetValue(val);
		}

	}
}
