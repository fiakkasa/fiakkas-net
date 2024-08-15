using Microsoft.AspNetCore.Components.Forms;

namespace ui.Tests.TestingExtensions;

public static class EditFormExtensions
{
    public static T? GetFormModel<T>(this EditForm editForm)
        where T : class
        => editForm.EditContext?.Model as T;
}
