using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services
{
    public interface IAsyncState
    {
         UniTask Enter();
         UniTask Exit();
    }
}