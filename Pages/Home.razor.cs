using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;
using System.Net.Http.Json;
using System.Text.Json;
using IC_Eval_FE.FragmentFactory;
using IC_Eval_FE.Services.Helper_Functions;


namespace IC_Eval_FE.Pages
{
    public partial class Home : ComponentBase
    {

        protected bool success;
        private bool isRequired;
        private string formTitle = string.Empty;
        string[] errors = { };
        protected string jsonResponse;
        MudTextField<string> pwField1;
        MudForm form;

        UserBindings userBindings = new();

        Validation validation = new Validation();

        private List<FormField> formFields = new List<FormField>();

        private Dictionary<string, object> formDataToJson = new Dictionary<string, object>();

        private FieldRenderer fieldRenderer = new FieldRenderer();

        protected override async Task OnInitializedAsync()
        {
            var response = await Http.GetFromJsonAsync<FormModel>(jsonResponse);

            try
            {
                if (response != null)
                {
                    formTitle = response.Title;
                    formFields = response.Fields;

                    // Check if the email field exists and if it's required
                    var emailField = response.Fields.FirstOrDefault(f => f.Type == "email");

                    if (emailField != null)
                    {
                        // Set a flag if the email field is required
                        isRequired = emailField.Required;
                        // You can now use isEmailRequired flag to conditionally handle email validation or form behavior
                    }
                    else
                    {
                        // Handle the case where no email field exists if necessary
                        isRequired = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching from server: {ex.Message}");
            }


        }

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private string PasswordMatch(string arg)
        {
            if (pwField1.Value != arg)
                return "Passwords don't match";
            return null;
        }

        // Determine type of Mud fragment required
        private Type GetFieldComponentType(string type)
        {
            return type switch
            {
                "text" => typeof(MudTextField<string>),
                "email" => typeof(MudTextField<string>),
                "number" => typeof(MudNumericField<int>),
                "dropdown" => typeof(MudSelect<string>),
                "checkbox" => typeof(MudCheckBox<bool>),
                "date" => typeof(MudDatePicker),
                "datetime" => typeof(MudDatePicker),
                "time" => typeof(MudTimePicker),
                "textarea" => typeof(MudTextField<string>),
                "range" => typeof(MudSlider<int>),
                "color" => typeof(MudColorPicker),
                "radio" => typeof(MudRadioGroup<string>),

                _ => typeof(MudTextField<string>)
            };
        }

        // Dynamically add attributes
        private RenderFragment RenderField(FormField field) => builder =>
        {

            builder.OpenComponent(0, GetFieldComponentType(field.Type));
            builder.AddAttribute(1, "Label", field.Label);
            builder.AddAttribute(2, "Required", field.Required);
            builder.AddAttribute(3, "Class", "mud-width-full");


            switch (field.Type)
            {
                case "text":
                    fieldRenderer.RenderTextField(builder, field, userBindings, formDataToJson);
                    break;

                case "email":
                    fieldRenderer.RenderEmailField(builder, field, userBindings, formDataToJson);
                    break;

                case "number":
                    fieldRenderer.RenderNumberField(builder, field, userBindings, formDataToJson);
                    break;

                case "dropdown":
                    fieldRenderer.RenderDropdownField(builder, field, userBindings, formDataToJson);
                    break;

                case "checkbox":
                    fieldRenderer.RenderCheckboxField(builder, field, userBindings, formDataToJson);
                    break;

                case "date":
                    fieldRenderer.RenderDateField(builder, field, userBindings, formDataToJson);
                    break;

                case "datetime":
                    fieldRenderer.RenderDateTimeField(builder, field, userBindings, formDataToJson);
                    break;

                case "time":
                    fieldRenderer.RenderTimeField(builder, field, userBindings, formDataToJson);
                    break;

                case "range":
                    fieldRenderer.RenderRangeField(builder, field, userBindings, formDataToJson);
                    break;

                case "radio":
                    fieldRenderer.RenderRadioField(builder, field, userBindings, formDataToJson);
                    break;

                case "color":
                    fieldRenderer.RenderColorField(builder, field, userBindings, formDataToJson);
                    break;

                default:
                    break;
            }


            builder.CloseComponent();
        };

        private void HandleValidSubmit()
        {

            bool isEmailValid = validation.ValidateEmail(userBindings.UserEmail);
            

            if (!isEmailValid && isRequired)
            {
                
                jsonResponse = "Invalid Email!";
                return;
            }

            form.Validate();
            if (form.IsValid)
            {

                jsonResponse = JsonSerializer.Serialize(formDataToJson);

            }

        }

        public async Task FetchFormConfig()
        {
            try
            {

                var response = await Http.GetFromJsonAsync<FormModel>("data");

                if (response != null)
                {

                    formTitle = response.Title;
                    formFields = response.Fields;


                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error fetching form config: {ex.Message}");
            }
        }


    }
}