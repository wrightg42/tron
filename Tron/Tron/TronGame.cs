﻿// TronGame.cs
// <copyright file="TronGame.cs"> This code is protected under the MIT License. </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tron
{
    /// <summary>
    /// A class that holds all the game data.
    /// </summary>
    public class TronGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TronGame" /> class.
        /// </summary>
        /// <param name="players"> How many players will be in the game. </param>
        public TronGame(int players)
        {
            // TODO: Write constructor
        }

        /// <summary>
        /// Gets or sets the car list. 
        /// </summary>
        public Car[] Cars { get; set; }

        /// <summary>
        /// Gets or sets the grid.
        /// </summary>
        public CellValues[][] Grid { get; set; }

        /// <summary>
        /// Start the game.
        /// </summary>
        public void InitializeGame()
        {
            this.InitializeGrid();
            this.InitializePlayers();
        }

        /// <summary>
        /// Initializes the grid.
        /// </summary>
        public void InitializeGrid()
        {
            // Set x size
            this.Grid = new CellValues[100][];
            
            // Set y size in each row
            for (int i = 0; i < 100; i++)
            {
                this.Grid[i] = new CellValues[100];
            }
        }

        /// <summary>
        /// Initializes the players.
        /// </summary>
        public void InitializePlayers()
        {
            // TODO: Write player initalization
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < this.Cars.Length; i++)
            {
                // Update the cars
                CellValues[][] gridCopy = this.Grid;
                this.Cars[i].Move(ref gridCopy);
                this.Grid = gridCopy;
            }
        }
    }
}
