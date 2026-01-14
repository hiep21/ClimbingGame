using System;
using System.Collections.Generic;
using NTHiep.MiniOdin;
using UnityEngine;

namespace Map
{
    public class GroundController : MonoBehaviour
    {
        [ColorHeader("<color=red>Var")]
        [SerializeField] GameObject _planePrefab;

        [ColorHeader("<color=red>Value")]
        [SerializeField] List<GameObject> _lsPlaneClone;

        [Button]
        public void TestSpawnPlane(int numberSpawn)
        {
            _lsPlaneClone.ForEach(f => Destroy(f));

            for (int i = 0; i < numberSpawn; i++)
            {
                GameObject planceClone = SpawnPlane();
                _lsPlaneClone.Add(planceClone);
            }
            
        }

        GameObject SpawnPlane()
        {
            GameObject planeClone = Instantiate(_planePrefab, _planePrefab.transform.parent);
            planeClone.SetActive(true);
            return planeClone;
        }

    }
}