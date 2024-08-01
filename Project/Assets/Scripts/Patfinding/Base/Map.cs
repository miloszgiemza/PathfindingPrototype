using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public class Map
    {
        private float defaultMovementCost = 0f;

        public struct Field
        {
            public bool traversable;
            public float cost;

            public Field(bool traversableValue, float costValue)
            {
                traversable = traversableValue;
                cost = costValue;
            }
        }

        public Field[,] MapData => mapData; 

        private Field[,] mapData;

        public Map(int width, int height, int obstaclesNumber)
        {
            mapData = new Field[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    mapData[x, y] = new Field(true, defaultMovementCost);
                }
            }

            GnerateObstacles(obstaclesNumber, width, height);
        }

        public void ReGenerateMapFromPool(int width, int height, int obstaclesNumber)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    mapData[x, y] = new Field(true, defaultMovementCost);
                }
            }

            GnerateObstacles(obstaclesNumber, width, height);
        }

        private void GnerateObstacles(int obstaclesNumber, int mapWidth, int mapHeight)
        {
            obstaclesNumber = Mathf.Clamp(obstaclesNumber, 0, mapWidth * mapHeight);

            List<PositionInGrid> avaliablePositionsForObstacles = new List<PositionInGrid>();

            for(int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapHeight; j++)
                {
                    PositionInGrid newAvaliablePositionForObstacle = new PositionInGrid(i, j);
                    avaliablePositionsForObstacles.Add(newAvaliablePositionForObstacle);
                }
            }

            for (int i = 0; i < obstaclesNumber; i++)
            {
                System.Random rand = new System.Random();

                int newObstaclePosition = rand.Next(0, avaliablePositionsForObstacles.Count-1);
                mapData[avaliablePositionsForObstacles[newObstaclePosition].X, avaliablePositionsForObstacles[newObstaclePosition].Z].traversable = false;
                avaliablePositionsForObstacles.RemoveAt(newObstaclePosition);
            }
        }

        public void SwitchMapDataPositionTraversableNonTraversable(Vector2 position, bool traversable)
        {
            mapData[(int)position.x, (int)position.y].traversable = traversable;
        }

        public void UpdateMapData(Vector2 position, bool traversableValue, int movementCostValue)
        {
            mapData[(int)position.x, (int)position.y].traversable = traversableValue;
            mapData[(int)position.x, (int)position.y].cost = movementCostValue;
        }
    }
}
