public class KeysUtil
{
    public static string GetPropertyKeys(string name)
    {
        var id = GameStateModel.Single.SelectedPlaneId;
        return id + name;
    }

    public static string GetPropertyKeys(int id, string name)
    {
        return id + name;
    }

    public static string GetNewKey(PropertyItem.ItemKey key, string propertyName)
    {
        return GetPropertyKeys(propertyName + key);
    }
}