using DeepRedAI.Parser;
using UnityEngine;

namespace DeepRedAI.Showdown
{
    public abstract class ClientState : MonoBehaviour
    {
        protected ShowdownClient _context;

        public virtual void EnterState(ShowdownClient context)
        {
            _context = context;
            gameObject.SetActive(true);
        }

        public virtual void LeaveState()
        {
            gameObject.SetActive(false);
        }

        public abstract void ReceiveMessage(ServerMessage message);
    }
}
