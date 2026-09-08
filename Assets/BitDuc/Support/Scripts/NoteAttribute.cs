using UnityEngine;

namespace BitDuc.Support
{
    public class NoteAttribute : PropertyAttribute
    {
        public readonly string Message;

        public NoteAttribute(string message)
        {
            Message = message;
        }
    }
}
