using DeepRedAI.Parser;
using UnityEngine;

namespace DeepRedAI.Showdown
{
    public abstract class ClientState : MonoBehaviour
    {
        public virtual void EnterState()
        {
            gameObject.SetActive(true);
        }

        public virtual void LeaveState()
        {
            gameObject.SetActive(false);
        }

        public abstract void ReceiveMessage(ShowdownClient context, ServerMessage message);
    }
}
