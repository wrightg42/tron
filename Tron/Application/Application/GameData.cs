﻿using System;
// GameData.cs
// <copyright file="GameData.cs"> This code is protected under the MIT License. </copyright>
using Tron;

namespace Application
{
    public static class GameData
    {
        /// <summary>
        /// Gets or sets the game information.
        /// </summary>
        public static TronGame Tron { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current game is a local multiplayer one.
        /// </summary>
        public static bool LocalTwoPlayer { get; set; }

        /// <summary>
        /// Gets or sets the id number for the online player you represent.
        /// </summary>
        public static int OnlinePlayerId { get; set; }

        /// <summary>
        /// Sends the move command to a player.
        /// </summary>
        /// <param name="direction"> The direction to move. </param>
        /// <param name="player"> The player inputting the move. </param>
        public static void ChangeDirection(Direction direction, int player)
        {
            // Change the id of the player being redirected if not local two player
            if (!LocalTwoPlayer)
            {
                // If the player key set is that of player two don't do anything
                if (player != 0)
                {
                    return;
                }

                player = OnlinePlayerId;
            }

            Tron.Cars[player].ChangeDirection(direction);
        }

        /// <summary>
        /// Boosts a player.
        /// </summary>
        /// <param name="player"> The player inputting the boost. </param>
        public static void Boost(int player)
        {
            // Change the id of the player being boosted if not local two player
            if (!LocalTwoPlayer)
            {
                // If the player key set is that of player two don't do anything
                if (player != 0)
                {
                    return;
                }

                player = OnlinePlayerId;
            }

            Tron.Cars[player].Boost();
        }
    }
}