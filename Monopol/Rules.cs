﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
    enum Spaces
    {
        GoToJail, AvailableProperty, OwnedProperty, Bisys, Nothing, MyProperty
    }
    static class Rules
    {
        /// <summary>
        /// Analyserar spelets nuvarande situation och kollar om det stämmer överrens med reglerna.
        /// T.ex. om en spelare får köpa den egendom som spelaren står på.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="game"></param>
        static public void CheckState(Player player, Game game)
        {
            player.AllowPlayerToBuyProperty(false);
            Debug.WriteLine("Plats: " + game.board[player.position].name + ", " + PositionType(player, game).ToString());

            if (player.cash < 0)
            {
                kickPlayer(player, game);
            }

            if (PositionType(player, game) == Spaces.GoToJail)
            {
                game.BustPlayer(player);
            }

            else if (PositionType(player, game) == Spaces.AvailableProperty)
            {
                player.AllowPlayerToBuyProperty(game.board[player.position] as Property);
            }

            else if (PositionType(player, game) == Spaces.OwnedProperty)
            {
                player.PayOpponent(game.findPlayer((game.board[player.position] as Property).owner), (game.board[player.position] as Property).rent);
            }

            else if (PositionType(player, game) == Spaces.Bisys)
            {
                game.newBisys();
                player.cash += (game.currentBisys.value);
                Debug.WriteLine(game.currentBisys.message + " " + game.currentBisys.value + " kr");
            }

        }

        static private void kickPlayer(Player player, Game game)
        {
            player.active = false;
            Debug.WriteLine(player.name + " har förlorat!");
            foreach (Space s in game.board)
            {
                if (s is Property)
                {
                    if (((Property)s).owner == player.name)
                        ((Property)s).owner = "";
                }
            }
        }

        static bool CheckIfJail(Player player, Game game)
        {

            return true;
        }

        static Spaces PositionType(Player player, Game game)
        {
            if (game.board[player.position].GetType() == typeof(GoToJail))
                return Spaces.GoToJail;

            else if (game.board[player.position].GetType() == typeof(Property))
            {
                Property p = (Property)game.board[player.position];
                if (p.owner == player.name)
                    return Spaces.MyProperty;
                else if (p.owner == "")
                    return Spaces.AvailableProperty;
                else
                    return Spaces.OwnedProperty;
            }

            else if (game.board[player.position].GetType() == typeof(BisysSpace))
                return Spaces.Bisys;

            else
                return Spaces.Nothing;
        }

    }
}
