using UnityEngine;

namespace Usable
{
    public interface IItem
    {
        void SetOwner(GameObject hands);
        void DiscardOwner(bool throwing);
        void Throw();
        void Use();
    }
}
