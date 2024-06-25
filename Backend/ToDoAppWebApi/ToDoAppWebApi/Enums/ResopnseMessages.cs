using System.ComponentModel;

namespace ToDoAppWebApi.NewFolder
{
    public static class ResponseMessages
    {
        public enum Messages
        {
            [Description("Successfully completed")] Success = 1, 
            [Description("Something Went wrong")] Exception = 2
        }
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
    }
}
