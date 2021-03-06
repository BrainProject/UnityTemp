﻿using UnityEngine;
using System.Collections;

namespace Frogger
{
    public enum VehicleType
    {
        Car,
        Boat
    }

    public class VehicleSpawner : MonoBehaviour
    {
        //public int roadIndex = 0;
        public int minimalSpawnDelay;
        public float roadLineSpeed;
        public VehicleType type;
        public float difficulty;



        FrogLevelManager thisLevelManager;
        int randomer;
        float lastSpawnTime;

        void Start()
        {
            difficulty = (1.5f * MGC.Instance.selectedMiniGameDiff);
            roadLineSpeed += difficulty;
            thisLevelManager = FrogLevelManager.Instance;
            randomer = Random.Range(0, Mathf.Clamp(minimalSpawnDelay * 2 - MGC.Instance.selectedMiniGameDiff, 1, minimalSpawnDelay * 2));
            lastSpawnTime = Time.time;
            //for(int i=0; i<MGC.Instance.selectedMiniGameDiff; ++i)
                SpawnNewVehicle();
        }

        void Update()
        {
            if ((Time.time - (lastSpawnTime + randomer + minimalSpawnDelay)) > 0)
            {
                lastSpawnTime = Time.time;
                randomer = Random.Range(0, Mathf.Clamp(minimalSpawnDelay * 2 - MGC.Instance.selectedMiniGameDiff, 1, minimalSpawnDelay * 2));
                SpawnNewVehicle();
            }
        }

        void SpawnNewVehicle()
        {
            switch (type)
            {
                case VehicleType.Car:
                    {
                        if (thisLevelManager.carPrefabs.Count > 0)
                        {
                            GameObject newCar = Instantiate(thisLevelManager.GetRandomCarPrefab(), this.transform.position, this.transform.rotation) as GameObject;
                            newCar.GetComponent<Rigidbody>().velocity = transform.right * roadLineSpeed;
                        }
                        break;
                    }
                case VehicleType.Boat:
                    {
                        if (thisLevelManager.boatPrefabs.Count > 0)
                        {
                            GameObject newBoat = Instantiate(thisLevelManager.GetRandomBoatPrefab(), this.transform.position, this.transform.rotation) as GameObject;
                            newBoat.GetComponent<Rigidbody>().velocity = transform.right * roadLineSpeed;
                        }
                        break;
                    }
            }
        }
    }
}