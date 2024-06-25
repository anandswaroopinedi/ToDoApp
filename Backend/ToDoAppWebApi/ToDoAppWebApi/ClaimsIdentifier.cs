namespace ToDoAppWebApi
{
    public static class ClaimsIdentifier
    {
        public static int getIdFromToken(HttpContext context)
        {
            var id = context.User.FindFirst(x => x.Type == "Id");
            if(id == null)
            {
                return 0;
            }
            else
            {
                return Int32.Parse(id.Value);
            }
            
        }
    }
}
