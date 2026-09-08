using UnityEngine;

namespace BitDuc.EnhancedTimeline.Demos.SpellCaster
{
    public class Spell : MonoBehaviour
    {
        public State State { get; private set; }

        [SerializeField] Transform leftHand;
        [SerializeField] Transform rightHand;
        [SerializeField] float passTime = .2f;
        [SerializeField] float throwTime = 1f;

        float time = 0f;

        void Update()
        {
            switch (State)
            {
                case State.Casting:
                    transform.position = (leftHand.position + rightHand.position) * 0.5f;
                    break;
                case State.Passing:
                    time += Time.deltaTime;
                    var bothHands = (leftHand.position + rightHand.position) * 0.5f;
                    var t = time / passTime;
                    transform.position = Vector3.Lerp(bothHands, rightHand.position, t);

                    if (time >= passTime)
                        State = State.Throwing;
                    break;
                case State.Throwing:
                    time += Time.deltaTime;
                    transform.position = rightHand.position;

                    if (time >= throwTime)
                        State = State.None;

                    break;
            }
        }

        public void Cast()
        {
            State = State.Casting;
        }

        public void Pass()
        {
            time = 0f;
            State = State.Passing;
        }
    }
}
