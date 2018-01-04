using Newtonsoft.Json.Linq;

namespace AssetActions
{
    public interface IAssetAction
    {
        string GetName();
        void CallAction(Interactable target, Player source);
    }
}