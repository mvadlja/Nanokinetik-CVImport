using AspNetUI.Views;
using Ready.Model;

namespace AspNetUI.Support
{
    public static class TypePkHelper
    {
        public static int? GetTypeValue(IType_PKOperations typeOperations, string[] groups, string value, string defaultGroup = "", string defaultValue = Constant.UnknownValue)
        {
            int? typePk = null;
            foreach (var group in groups)
            {
                var type = typeOperations.GetEntityByGroup(group, value);
                if (type != null)
                {
                    typePk = type.type_PK;
                    break;
                }
            }

            var _defaultGroup = !string.IsNullOrWhiteSpace(defaultGroup) ? defaultGroup : groups.Length > 0 ? groups[0] : string.Empty;
            if (!typePk.HasValue && !string.IsNullOrWhiteSpace(_defaultGroup))
            {
                var defaultType = typeOperations.GetEntityByGroup(_defaultGroup, defaultValue);
                if (defaultType != null)
                {
                    typePk = defaultType.type_PK;
                }
            }

            return typePk;
        }
    }
}