namespace GymManagementPL.Comments
{
    public class Comments
    {
        // Deference between ViewBag, ViewData and TempData in ASP.NET MVC : 

        // ViewData → Dictionary<string, object>
        // Used to pass data from Controller to View (same request only)
        // Requires casting when reading values

        // ViewBag → Dynamic wrapper around ViewData
        // Also passes data from Controller to View (same request only)
        // Easier syntax, no casting needed

        // TempData → Dictionary<string, object> stored in Session
        // Used to pass data between requests (after Redirect)
        // Automatically cleared after being read (unless you Keep or Peek)

        //========================================================================

        // 🧩 ASP.NET Core Tag Helpers

        // asp-for → binds an HTML element to a specific model property.
        // It automatically sets the name, id, and value based on the model.
        // Example: <input asp-for="UserName" /> 
        // This creates <input id="UserName" name="UserName" value="@Model.UserName" />

        // asp-validation-for → displays validation messages for that model property.
        // It works with ModelState and DataAnnotations (like [Required], [EmailAddress], etc.)
        // Example: <span asp-validation-for="UserName"></span>
        // This will show an error message if UserName fails validation.

        // asp-validation-for → displays validation messages for that specific property.
        // Works with DataAnnotations (like [Required], [StringLength], etc.)
        // Example: <span asp-validation-for="UserName"></span>

        // asp-items → used with <select> to populate dropdown options from a collection (like SelectList).
        // Example: <select asp-for="CountryId" asp-items="Model.Countries"></select>

        //========================================================================

    }
}
