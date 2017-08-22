using DeepRedAI.Parser;
using UnityEngine;

namespace DeepRedAI.Showdown
{
    public abstract class ClientState : MonoBehaviour
    {
        public virtual void EnterState(ShowdownClient context)
        {
            gameObject.SetActive(true);
        }

        public virtual void LeaveState(ShowdownClient context)
        {
            gameObject.SetActive(false);
        }

        public abstract void ReceiveMessage(ShowdownClient context, ServerMessage message);
    }
}
