using Newtonsoft.Json.Linq;

namespace AssetActions
{
    public interface IAssetAction
    {
        void CallAction(Interactable target);

        string GetName();
    }
}