namespace Application.Common.Security;
public static class Permissions
{
    /// <summary>
    /// These will generate constant property value of perticular module like Faculties
    /// </summary>
    /// <param name="module">The name of the module</param>
    /// <returns></returns>
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return new List<string>
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete"
        };
    }

    /// <summary>
    /// These will generate list of modules name
    /// </summary>
    /// <returns>List of string</returns>
    public static List<string> GetAllNestedModule()
    {
        Type permissionType = typeof(Permissions);
        Type[] nestedTypes = permissionType.GetNestedTypes();
        List<string> result = [];
        foreach (var type in nestedTypes)
        {
            result.Add(type.Name);
        }
        return result;
    }

    /// <summary>
    /// The will generate list of modules type
    /// </summary>
    /// <returns>Array of Type</returns>
    public static Type[] GetAllNestedModuleType()
    {
        Type permissionType = typeof (Permissions);
        Type[] nestedTypes = permissionType.GetNestedTypes();
        return nestedTypes;
    }
}
