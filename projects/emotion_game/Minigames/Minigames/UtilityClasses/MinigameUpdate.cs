using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minigames.SingeltonClasses;
using Minigames.PhysicsLogicClasses;
using Microsoft.Xna.Framework.Input;


namespace Minigames.UtilityClasses
{
    class MinigameUpdate
    {
        //
        //updator classes instances
        //
        PuzzlePhysics _puzzle;

        public MinigameUpdate()
        {
            //
            //initialize update objects
            //
            _puzzle = new PuzzlePhysics();

        }

        public void Update(KeyboardState key)
        {
            if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.puzzle_TAG)
            {
                _puzzle.Update(key);   
            }
        }
    }
}
