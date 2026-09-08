#if MIRROR
using System;
using Mirror;
using UnityEngine;

namespace BitDuc.EnhancedTimeline.Demos.Multiplayer
{
    public class ServerFighterInput: NetworkBehaviour, FighterInput
    {
        public bool Left => CheckInput(InputCommand.Left);
        public bool Right => CheckInput(InputCommand.Right);
        public bool Attack => CheckInputDown(InputCommand.Attack);
        public bool Block => CheckInputDown(InputCommand.Block);

        [Flags]
        enum InputCommand
        {
            None = 0,
            Left = 1,
            Right = 2,
            Attack = 4,
            Block = 5
        }

        InputCommand previousInput = InputCommand.None;
        InputCommand synchronizedInput;

        void Update()
        {
            if (isServer && !isClient)
                return;

            var input =
                GetInput(KeyCode.A, InputCommand.Left) | 
                GetInput(KeyCode.D, InputCommand.Right) |
                GetInput(KeyCode.G, InputCommand.Attack) |
                GetInput(KeyCode.F, InputCommand.Block);

            if (previousInput == input)
                return;

            SendInput(input);
            previousInput = input;
        }

        [Command]
        void SendInput(InputCommand input)
        {
            previousInput = synchronizedInput;
            synchronizedInput = input;
        }

        static InputCommand GetInput(KeyCode code, InputCommand command) =>
            Input.GetKey(code) ? command : InputCommand.None;

        bool CheckInput(InputCommand toCheck) =>
            (synchronizedInput & toCheck) > 0;

        bool CheckInputDown(InputCommand toCheck) =>
            (previousInput & toCheck) == 0 && CheckInput(toCheck);
    }
}
#endif
