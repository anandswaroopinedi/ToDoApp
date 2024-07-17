namespace ToDoAppWebApi
{
    public static class ClaimsIdentifier
    {
        public static int GetIdFromToken(this HttpContext context)
        {
            var idClaim = context.User.Claims.FirstOrDefault(x => x.Type == "Id");

            if (idClaim == null || !int.TryParse(idClaim.Value, out int id))
            {
                return 0; 
            }

            return id;
        }
    }
}
